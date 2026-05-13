using Alumni_Portal.Auth;
using Alumni_Portal.Auth.Services;
using Alumni_Portal.Auth.Services.DTO;
using Alumni_Portal.Infrastructure.Data_Models;
using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.RAID.Services;
using Entity_Directories.Services.DTO;
using Microsoft.EntityFrameworkCore;
using Shared.Custom_Exceptions.ExceptionClasses;
using System.Text.Json;

namespace Entity_Directories.Services
{
    public class RegistrationRequestService
    {
        private readonly RegistrationDbContext _regContext;
        private readonly IndividualDbContext _individualContext;
        private readonly UserService _userService;
        private readonly EmailService _emailService;
        private readonly RegistrationOtpService _otpService;

        public RegistrationRequestService(
            RegistrationDbContext regContext,
            IndividualDbContext individualContext,
            UserService userService,
            EmailService emailService,
            RegistrationOtpService otpService)
        {
            _regContext = regContext;
            _individualContext = individualContext;
            _userService = userService;
            _emailService = emailService;
            _otpService = otpService;
        }

        // ── SUBMIT ────────────────────────────────────────────────────────────
        public async Task<int> SubmitAsync(SubmitRegistrationRequestDTO dto)
        {
            bool emailRegistered = await _individualContext.Individuals
                .AnyAsync(i => i.Individual_Email == dto.Email);
            if (emailRegistered)
                throw new ValidationException("An account with this email already exists.");

            bool pendingExists = await _regContext.Registration_Requests
                .AnyAsync(r => r.Email == dto.Email && (r.Status == "Pending" || r.Status == "Pending_OTP"));
            if (pendingExists)
                throw new ValidationException("A registration request with this email is already pending.");

            var userType = dto.UserType.ToLower();
            bool requiresOtp = userType is "student" or "supervisor";

            // Students & supervisors must use NU email
            if (requiresOtp && !dto.Email.EndsWith("@nu.edu.pk", StringComparison.OrdinalIgnoreCase))
                throw new ValidationException("Students and supervisors must use their official @nu.edu.pk email.");

            string passwordHash = PasswordHasher.Hash(dto.Password);

            string? workExpJson = dto.WorkExperience is { Count: > 0 }
                ? JsonSerializer.Serialize(dto.WorkExperience)
                : null;

            var request = new Registration_Request
            {
                User_Type = dto.UserType,
                First_Name = dto.FirstName,
                Last_Name = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Password_Hash = passwordHash,
                Roll_Number = dto.RollNumber,
                Department_ID = dto.DepartmentId,
                Department_Value = dto.DepartmentValue,
                Enrollment_Year = dto.EnrollmentYear,
                Graduation_Year = dto.GraduationYear,
                Current_Job_Company = dto.CurrentJobCompany,
                Current_Job_Role = dto.CurrentJobRole,
                Work_Experience_JSON = workExpJson,
                Designation_ID = dto.DesignationId,
                Designation_Value = dto.DesignationValue,
                Status = requiresOtp ? "Pending_OTP" : "Pending",
                Submitted_At = DateTime.UtcNow,
            };

            _regContext.Registration_Requests.Add(request);
            await _regContext.SaveChangesAsync();

            // Send OTP for students/supervisors
            if (requiresOtp)
            {
                await SendOtpEmailAsync(request.Request_ID, request.Email, request.First_Name);
            }

            return request.Request_ID;
        }

        // ── VERIFY OTP ────────────────────────────────────────────────────────
        public async Task VerifyOtpAsync(VerifyRegistrationOtpDTO dto)
        {
            var req = await _regContext.Registration_Requests.FindAsync(dto.RequestId)
                      ?? throw new ValidationException("Registration request not found.");

            if (req.Status != "Pending_OTP")
                throw new ValidationException("This request is not awaiting OTP verification.");

            if (!_otpService.Verify(req.Request_ID, dto.Otp))
                throw new ValidationException("Invalid or expired verification code.");

            // OTP good — flip to Pending and auto-approve.
            req.Status = "Pending";
            await _regContext.SaveChangesAsync();

            await ApproveAsync(req.Request_ID);
        }

        // ── RESEND OTP ────────────────────────────────────────────────────────
        public async Task ResendOtpAsync(int requestId)
        {
            var req = await _regContext.Registration_Requests.FindAsync(requestId)
                      ?? throw new ValidationException("Registration request not found.");

            if (req.Status != "Pending_OTP")
                throw new ValidationException("This request is not awaiting OTP verification.");

            await SendOtpEmailAsync(req.Request_ID, req.Email, req.First_Name);
        }

        // ── CANCEL / DELETE ───────────────────────────────────────────────────
        public async Task CancelAsync(int requestId)
        {
            var req = await _regContext.Registration_Requests.FindAsync(requestId)
                      ?? throw new ValidationException("Registration request not found.");

            if (req.Status != "Pending_OTP")
                throw new ValidationException("Only pending-OTP requests can be cancelled.");

            _regContext.Registration_Requests.Remove(req);
            await _regContext.SaveChangesAsync();
        }

        // ── INTERNAL: send the OTP email ──────────────────────────────────────
        private async Task SendOtpEmailAsync(int requestId, string email, string firstName)
        {
            var otp = _otpService.Generate(requestId);

            var subject = "FAST ALCOM — Verify Your Email";
            var body = $@"
                <div style='font-family:sans-serif;max-width:480px;margin:auto'>
                    <h2 style='color:#002147'>Hello {firstName},</h2>
                    <p>Use the code below to verify your university email and complete your registration:</p>
                    <p style='font-size:32px;font-weight:bold;letter-spacing:6px;color:#002147;text-align:center;padding:16px;background:#f3f4f6;border-radius:8px'>{otp}</p>
                    <p>This code expires in <strong>10 minutes</strong>.</p>
                    <p style='color:#6b7280;font-size:13px'>If you did not request this, you can ignore this email — your registration will be cancelled automatically.</p>
                </div>";

            await _emailService.SendMailAsync(
                client_Id: 1,
                toEmail: new[] { email },
                ccEmail: Array.Empty<string>(),
                subject: subject,
                message: body);
        }

        // ── GET PENDING (only fully verified ones) ───────────────────────────
        public async Task<List<RegistrationRequestListDTO>> GetPendingAsync()
        {
            return await _regContext.Registration_Requests
                .AsNoTracking()
                .Where(r => r.Status == "Pending") // exclude Pending_OTP
                .OrderByDescending(r => r.Submitted_At)
                .Select(r => new RegistrationRequestListDTO
                {
                    RequestId = r.Request_ID,
                    UserType = r.User_Type,
                    FirstName = r.First_Name,
                    LastName = r.Last_Name,
                    Email = r.Email,
                    Phone = r.Phone,
                    Status = r.Status,
                    SubmittedAt = r.Submitted_At,
                    RollNumber = r.Roll_Number,
                    DepartmentValue = r.Department_Value,
                    EnrollmentYear = r.Enrollment_Year,
                    GraduationYear = r.Graduation_Year,
                    CurrentJobCompany = r.Current_Job_Company,
                    CurrentJobRole = r.Current_Job_Role,
                    WorkExperienceJson = r.Work_Experience_JSON,
                    DesignationValue = r.Designation_Value,
                })
                .ToListAsync();
        }

        // ── APPROVE ───────────────────────────────────────────────────────────
        public async Task ApproveAsync(int requestId)
        {
            var req = await _regContext.Registration_Requests.FindAsync(requestId)
                      ?? throw new ValidationException("Registration request not found.");

            // Map user type to ID/value used by the system
            var (typeId, typeValue, isAlumni) = req.User_Type.ToLower() switch
            {
                "student" => (13, "Student", false),
                "alumni" => (14, "Alumni", true),
                "supervisor" => (15, "Supervisor", false),
                _ => throw new ValidationException($"Unknown user type: {req.User_Type}")
            };

            bool emailTaken = await _individualContext.Individuals
                .AnyAsync(i => i.Individual_Email == req.Email);
            if (emailTaken)
                throw new ValidationException($"An account with the email {req.Email} already exists.");

            // Build academic details
            var academics = new List<AcademicDetailsDTO>();
            if (req.User_Type.ToLower() is "student" or "alumni")
            {
                academics.Add(new AcademicDetailsDTO
                {
                    Department_ID = req.Department_ID,
                    Department_Value = req.Department_Value,
                    Enrollment_Year = req.Enrollment_Year,
                    Graduation_Year = req.Graduation_Year,
                    Is_Ongoing = req.User_Type.ToLower() == "student",
                });
            }
            else if (req.User_Type.ToLower() == "supervisor")
            {
                academics.Add(new AcademicDetailsDTO
                {
                    Department_ID = req.Department_ID,
                    Department_Value = req.Department_Value,
                    Designation_ID = req.Designation_ID,
                    Designation_Value = req.Designation_Value,
                });
            }

            var newUserDTO = new NewUserDTO
            {
                Institution_ID = req.Roll_Number ?? req.Email,
                Name = $"{req.First_Name} {req.Last_Name}",
                Email = req.Email,
                Type_ID = typeId,
                Type_Value = typeValue,
                Is_Alumni = isAlumni,
                Contact_Number = req.Phone,
                Client_ID = 1,
                Campus_ID = 1,
                Academic_Details = academics,
            };

            await _userService.CreateUser(newUserDTO);

            // Set the password hash directly on the created individual
            await _individualContext.Individuals
                .Where(i => i.Individual_Email == req.Email)
                .ExecuteUpdateAsync(s => s.SetProperty(i => i.password_hash, req.Password_Hash));

            // Add work experience for alumni
            if (req.User_Type.ToLower() == "alumni" && req.Work_Experience_JSON is not null)
            {
                var workExps = JsonSerializer.Deserialize<List<WorkExpSubmitDTO>>(req.Work_Experience_JSON);
                if (workExps is { Count: > 0 })
                {
                    var individual = await _individualContext.Individuals
                        .AsNoTracking()
                        .FirstOrDefaultAsync(i => i.Individual_Email == req.Email);

                    if (individual is not null)
                    {
                        foreach (var exp in workExps.Where(e => e.Company != null || e.Role != null))
                        {
                            await _userService.AddWorkExperienceAsync(individual.Individual_ID, new AddWorkExperienceDTO
                            {
                                Job_Title = exp.Role,
                                Company_Name = exp.Company,
                                Start_Date = ParseDate(exp.StartDate),
                                End_Date = exp.IsCurrent ? null : ParseDate(exp.EndDate),
                                Is_Current = exp.IsCurrent,
                            });
                        }
                    }
                }
            }

            // Mark approved
            req.Status = "Approved";
            req.Reviewed_At = DateTime.UtcNow;
            await _regContext.SaveChangesAsync();

            await _emailService.SendRegistrationApprovedAsync(req.First_Name, req.Last_Name, req.Email, req.User_Type);
        }

        // ── REJECT ────────────────────────────────────────────────────────────
        public async Task RejectAsync(int requestId)
        {
            var req = await _regContext.Registration_Requests.FindAsync(requestId)
                      ?? throw new ValidationException("Registration request not found.");

            req.Status = "Rejected";
            req.Reviewed_At = DateTime.UtcNow;
            await _regContext.SaveChangesAsync();

            await _emailService.SendRegistrationRejectedAsync(req.First_Name, req.Last_Name, req.Email);
        }

        private static DateTime? ParseDate(string? dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr) || dateStr.ToLower() == "present")
                return null;
            return DateTime.TryParse(dateStr, out var d) ? d : null;
        }
    }
}
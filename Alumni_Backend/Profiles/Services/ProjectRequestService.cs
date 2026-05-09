using Alumni_Portal.Engagement.Services.DTO;
using Alumni_Portal.Infrastrcuture.Data_Models;
using Alumni_Portal.Infrastructure.Persistance;
using Alumni_Portal.Infrastructure.Persistence;
using Alumni_Portal.Profiles.DTO;
using Alumni_Portal.RAID.Services;
using Microsoft.EntityFrameworkCore;
using Shared.Custom_Exceptions.ExceptionClasses;

namespace Alumni_Portal.Profiles.Services
{
    public class ProjectRequestService
    {
        private readonly EmailService _emailService;
        private readonly SharedDbContext _sharedContext;
        private readonly ProjectDbContext _projectContext;

        public ProjectRequestService(SharedDbContext sharedContext, ProjectDbContext projectContext, EmailService emailService)
        {
            _sharedContext = sharedContext;
            _projectContext = projectContext;
            _emailService = emailService;
        }

        public async Task<List<ProjectRequestDTO>> GetRequestsAsync(int projectId, CancellationToken ct = default)
        {
            return await _sharedContext.Requests
                .AsNoTracking()
                .Where(r => r.Project_ID == projectId)
                .OrderByDescending(r => r.Created_At)
                .Select(r => new ProjectRequestDTO
                {
                    Request_ID = r.Request_ID,
                    Request_Type_Value = r.Request_Type_Value,
                    Individual_ID = r.Individual_ID,
                    Individual_Name = r.Individual_Name,
                    Individual_Email = r.Individual_Email,
                    Individual_Contact_Number = r.Individual_Contact_Number,
                    Individual_LinkedIn_Url = r.Individual_LinkedIn_Url,
                    Motivation_Statement = r.Motivation_Statement,
                    Status_Value = r.Status_Value,
                    Is_Individual_Registered = r.Is_Individual_Registered,
                    Created_At = r.Created_At,
                })
                .ToListAsync(ct);
        }

        public async Task AcceptRequestAsync(int requestId, CancellationToken ct = default)
        {
            var request = await _sharedContext.Requests
                .FirstOrDefaultAsync(r => r.Request_ID == requestId, ct)
                ?? throw new Exception("Request not found.");

            bool isSupervisor = string.Equals(request.Request_Type_Value, "Supervisor", StringComparison.OrdinalIgnoreCase);
            bool isMentor = !isSupervisor;
            string role = isSupervisor ? "Supervisor" : "Mentor";

            if (isMentor)
            {
                bool mentorExists = await _projectContext.Project_Individuals
                    .AnyAsync(pi => pi.Project_ID == request.Project_ID &&
                                    pi.Individual_Role.ToLower() == "mentor", ct);

                if (mentorExists)
                    throw new ValidationException("This project already has a mentor. Remove the existing mentor before assigning a new one.");
            }

            if (request.Individual_ID.HasValue)
            {
                var member = new Project_Individuals
                {
                    Project_ID = request.Project_ID,
                    Individual_ID = request.Individual_ID.Value,
                    Individual_Role = role,
                };
                await _projectContext.Project_Individuals.AddAsync(member, ct);

                if (isMentor)
                {
                    await _projectContext.Projects
                        .Where(p => p.Project_ID == request.Project_ID)
                        .ExecuteUpdateAsync(s => s.SetProperty(p => p.Is_Mentored, true), ct);
                }

                await _projectContext.SaveChangesAsync(ct);
            }

            if (isSupervisor)
            {
                var allSupervisorRequests = await _sharedContext.Requests
                    .Where(r => r.Project_ID == request.Project_ID &&
                                r.Request_Type_Value != null &&
                                r.Request_Type_Value.ToLower() == "supervisor")
                    .ToListAsync(ct);
                _sharedContext.Requests.RemoveRange(allSupervisorRequests);
            }
            else
            {
                _sharedContext.Requests.Remove(request);
            }

            await _sharedContext.SaveChangesAsync(ct);
        }

        public async Task RejectRequestAsync(RequestRejectionDTO dto, CancellationToken ct = default)
        {
            var request = await _sharedContext.Requests
                .FirstOrDefaultAsync(r => r.Request_ID == dto.Request_ID, ct)
                ?? throw new Exception("Request not found.");

            if (string.IsNullOrWhiteSpace(dto.Individual_Email))
                throw new Exception($"Request {dto.Request_ID} has no associated email address. Rejection email not sent and request has not been deleted.");

            try
            {
                await _emailService.SendUserProjectRequestRejectionAsync(dto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to send rejection email for request {dto.Request_ID}. The request has not been deleted.", ex);
            }

            _sharedContext.Requests.Remove(request);
            await _sharedContext.SaveChangesAsync(ct);
        }
    }
}

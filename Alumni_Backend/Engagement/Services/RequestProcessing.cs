using Alumni_Portal.Engagement.Services.DTO;
using Alumni_Portal.Infrastrcuture.Data_Models;
using Shared.Custom_Exceptions.ExceptionClasses;
using Alumni_Portal.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Alumni_Portal.RAID.Login;
using Alumni_Portal.RAID.Services.Email;
using Alumni_Portal.RAID.Services;
namespace Alumni_Portal.Engagement.Services

{
    public class RequestProcessing 
    {
        private readonly SharedDbContext _context;
       private readonly LoginService _loginService;
        private readonly EmailService _emailService;
        public RequestProcessing(SharedDbContext context, LoginService loginService, EmailService emailService)
        {
            _context = context;
            _loginService = loginService;
            _emailService = emailService;
        }
        public async Task ProcessRequest(RequestDTO request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Project_ID <= 0)
                throw new ArgumentException("Invalid Project_ID");

            var entity = new Requests
            {

                Request_Type_ID = request.Request_Type_ID,
                Request_Type_Value = request.Request_Type_Value,


                Client_ID = 1,
                Campus_ID = 1,
                Client_Reference_Key = null,
                Campus_Reference_Key = null,


                Project_ID = request.Project_ID,
                Project_Academic_ID = request.Project_Academic_ID,
                Project_Name = request.Project_Name,


                Is_Individual_Registered = request.Is_Individual_Registered,
                Individual_ID = request.Individual_ID,
                Individual_Academic_ID = request.Individual_Academic_ID,
                Individual_Name = request.Individual_Name,
                Individual_Email = request.Individual_Email,
                Individual_Contact_Number = request.Individual_Contact_Number,
                Individual_LinkedIn_Url = request.Individual_LinkedIn_Url,


                Is_Organization = request.Is_Organization,
                Organization_Name = request.Organization_Name,
                Organization_Role = request.Organization_Role,


                Motivation_Statement = request.Motivation_Statement,


                Status_ID = request.Status_ID,
                Status_Value = request.Status_Value,
               

                Created_At = DateTime.UtcNow,
                Updated_At = null
            };
            try

            {

                _context.Requests.Add(entity);
                await _context.SaveChangesAsync();
                

            }


            catch (Exception ex)
            {
                throw new Exception("Failed to add Requests");
            }

            try
            {
                request.Created_At = entity.Created_At;
                var requestId = entity.Request_ID;
                await _emailService.SendRequestEmailsAsync(request, requestId);
            }

            catch (Exception ex)
            {
                throw new Exception("Failed to send notification email");
            }



        }

        public async Task<List<RequestNotificationDTO>> GetRequest(int limit, int size)
        {

            if (limit < 0 || size < 0)
            {
                throw new ValidationException("Page and limit must be greater than zero");
            }
           return  await _context.Requests
                .Select(RequestMapping.RequestToNotification())
                .OrderByDescending(r => r.Created_At).
                Skip(limit)
                .Take(size).ToListAsync();

        }


        

        

    }
}

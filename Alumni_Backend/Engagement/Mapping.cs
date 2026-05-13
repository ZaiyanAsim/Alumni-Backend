using Alumni_Portal.Engagement.Services.DTO;
using Alumni_Portal.Infrastrcuture.Data_Models;
using Alumni_Portal.Infrastructure.Data_Models;
using Azure.Core;
using System.Linq.Expressions;

namespace Alumni_Portal.Engagement
{
    public class RequestMapping
    {
        public static Expression<Func<Requests, RequestNotificationDTO>> RequestToNotification()
        {
            return request => new RequestNotificationDTO
            {
               
                Request_Type_Value = request.Request_Type_Value,

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
                
                Status_Value = request.Status_Value,


                Created_At = request.Created_At,

            };


        }

    }

}


         

using Alumni_Portal.Engagement.Services.DTO;
using Alumni_Portal.RAID.DTO;
using Alumni_Portal.RAID.Login;
using Alumni_Portal.RAID.Login.DTO;
using Alumni_Portal.RAID.Services.Email;
using Alumni_Portal.Engagement.Services.DTO;
using RabbitMQ.Client;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
namespace Alumni_Portal.RAID.Services
{
    public class EmailService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<EmailService> _logger;
        private readonly LoginService _loginService;

        private String token;
        private HttpClient client;

        public EmailService(LoginService loginService, IHttpClientFactory httpClientFactory, ILogger<EmailService> logger)
        {
            _loginService = loginService;
            _httpClientFactory = httpClientFactory;
            _logger = logger;

        }


        

        public async Task SendRequestEmailsAsync(RequestDTO dto, int requestId)
        {

            this.client = _httpClientFactory.CreateClient("AuthorizedRAIDClient");
            this.token = await _loginService.GetEmailAuthorizationToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.token);



            await SendAdminNotificationAsync(dto, requestId);

        
            await SendUserConfirmationAsync(dto, requestId);
        }



        private async Task SendAdminNotificationAsync(RequestDTO dto, int requestId)
        {
            var subject = RequestNotificationEmailBuilder.BuildSubject(dto);
            var body = RequestNotificationEmailBuilder.BuildBody(dto, requestId);

            _logger.LogInformation("Sending admin notification for request {RequestId} to {Email}", requestId, dto.Individual_Email);
            try
            {
                await SendMailAsync(6, new List<string> { dto.Individual_Email }, new List<string>(), subject, body);
                _logger.LogInformation("Admin notification sent for request {RequestId}", requestId);
            }
            catch (Exception ex)
            {
              
                _logger.LogError(ex, "CRITICAL: Failed to send admin notification for request {RequestId}", requestId);
                throw;
                
            }
        }

        private async Task SendUserConfirmationAsync(RequestDTO dto, int requestId)
        {
            if (string.IsNullOrWhiteSpace(dto.Individual_Email))
            {
                _logger.LogWarning("No user email on request {RequestId} — skipping confirmation", requestId);
                return;
            }

            var subject = RequestConfirmationEmailBuilder.BuildSubject(dto);
            var body = RequestConfirmationEmailBuilder.BuildBody(dto, requestId);

            try
            {
                await SendMailAsync(6, new List<string> { dto.Individual_Email }, new List<string>(), subject, body);
                _logger.LogInformation("User confirmation sent to {Email} for request {RequestId}", dto.Individual_Email, requestId);
            }
            catch (Exception ex)
            {
               
                _logger.LogError(ex, "Failed to send user confirmation to {Email} for request {RequestId} — request is still saved", dto.Individual_Email, requestId);
            }
        }


        public async Task SendUserProjectRequestRejectionAsync(RequestRejectionDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Individual_Email))
            {
                _logger.LogWarning("No user email on request {RequestId} — Failed to send Rejection Email");
                return;
            }

            var subject = RequestRejectionEmailBuilder.BuildSubject(dto);
            var body = RequestRejectionEmailBuilder.BuildBody(dto);

            try
            {
                await SendMailAsync(6, new List<string> { dto.Individual_Email }, new List<string>(), subject, body);
                _logger.LogInformation("User confirmation sent to {Email} for request {RequestId}", dto.Individual_Email, dto.Request_ID);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Failed to send user confirmation to {Email} for request {RequestId} — request is still saved", dto.Individual_Email, dto.Request_ID);
                throw;
            }
        }










        public async Task SendRegistrationApprovedAsync(string firstName, string lastName, string email, string userType)
        {
            if (string.IsNullOrWhiteSpace(email)) return;

            this.client = _httpClientFactory.CreateClient("AuthorizedRAIDClient");
            this.token  = await _loginService.GetEmailAuthorizationToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.token);

            var subject = RegistrationApprovalEmailBuilder.BuildSubject(firstName, lastName);
            var body    = RegistrationApprovalEmailBuilder.BuildBody(firstName, lastName, userType);

            try
            {
                await SendMailAsync(6, new List<string> { email }, new List<string>(), subject, body);
                _logger.LogInformation("Approval email sent to {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send approval email to {Email}", email);
            }
        }

        public async Task SendRegistrationRejectedAsync(string firstName, string lastName, string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return;

            this.client = _httpClientFactory.CreateClient("AuthorizedRAIDClient");
            this.token  = await _loginService.GetEmailAuthorizationToken();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.token);

            var subject = RegistrationRejectionEmailBuilder.BuildSubject(firstName, lastName);
            var body    = RegistrationRejectionEmailBuilder.BuildBody(firstName, lastName);

            try
            {
                await SendMailAsync(6, new List<string> { email }, new List<string>(), subject, body);
                _logger.LogInformation("Rejection email sent to {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send rejection email to {Email}", email);
            }
        }

        private async Task SendMailAsync(int client_Id, ICollection<string> toEmail, ICollection<string> ccEmail, string subject, string message)
        {
            
        

            var emailDTO = new EmailDTO
            {
                toEmails = toEmail,
                clientId= client_Id,
                subject = subject,
                message = message,
                ccEmails = ccEmail
            };

            var content = new StringContent(
                JsonSerializer.Serialize(emailDTO),
                Encoding.UTF8,
                "application/json");

            var response = await this.client.PostAsync("Email/send", content);
            var responseString = await response.Content.ReadAsStringAsync();

            _logger.LogInformation(
                "Email API responded {StatusCode}: {Body}",
                (int)response.StatusCode,
                responseString);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Email send failed. Status: {StatusCode}, Body: {Body}",
                    (int)response.StatusCode,
                    responseString);
                throw new Exception($"Email sending failed: {responseString}");
            }
        }
    }
}
    
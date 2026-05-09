
using System.Text.Json;
using Alumni_Portal.RAID.Login.DTO;
using System.Net.Http.Headers;
using System.Text;
using Alumni_Portal.Auth.Services.DTO;

namespace Alumni_Portal.RAID.Login
{
    public class LoginService
    {
      

        private readonly IHttpClientFactory _httpClientFactory;

        private string _accessToken;
        private DateTime _tokenExpiry;
        private readonly ILogger<LoginService> _logger;
        private HttpClient _httpClient;


        public LoginService(IHttpClientFactory httpClientFactory,ILogger<LoginService>logger)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient("AuthorizedRAIDClient");
            _logger = logger;
        }

        public async Task<string> GetEmailAuthorizationToken()
        {
            if (!string.IsNullOrEmpty(_accessToken) && _tokenExpiry > DateTime.UtcNow)
            {
                return _accessToken;
            }

         

            var loginRequest = new
            {
                email = "zaiyan.asim@fyp.com",
                password = "LZlRd6tlufnojgkcjjVdnA=="
            };

            var content = new StringContent(
                JsonSerializer.Serialize(loginRequest),
                Encoding.UTF8,
                "application/json"
            );

            var response = await this._httpClient.PostAsync("Auth/login", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(responseString);

            _accessToken = loginResponse.data.accessToken;

            // assuming your API gives expiry (if not, set manually e.g. 1 hour)
            _tokenExpiry = DateTime.UtcNow.AddMinutes(50);

            return _accessToken;
        }





        public async Task<AdminAuthInfoDataDTO?> GetAdminPermissionsAsync(string jwtToken)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "Auth/Info");
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", jwtToken);

                var response = await this._httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Admin Auth Info returned {Status}", response.StatusCode);
                    return null;
                }

                var result = await response.Content
                    .ReadFromJsonAsync<AdminAuthInfoResponseDTO>();

                if (result?.Data == null)
                {
                    _logger.LogWarning("Admin Auth Info response was null or empty");
                    return null;
                }

                // ✅ Log the full response as JSON
                _logger.LogInformation("Admin Auth Info response: {Data}",
                    JsonSerializer.Serialize(result.Data));

                // ✅ Or log specific fields
                _logger.LogInformation(
                    "Permissions fetched for {UserName} — [{Permissions}]",
                    result.Data.User_Name,
                    string.Join(", ", result.Data.FeaturePermissionKeys));

                return result.Success == true ? result.Data : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch permissions from third party");
                return null;
            }
        }


    }

}


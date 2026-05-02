
using System.Text.Json;
using Alumni_Portal.RAID.Login.DTO;
using System.Net.Http.Headers;
using System.Text;

namespace Alumni_Portal.RAID.Login
{
    public class LoginService
    {
      

        private readonly IHttpClientFactory _httpClientFactory;

        private string _accessToken;
        private DateTime _tokenExpiry;

        public LoginService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
          
        }

        public async Task<string> GetEmailAuthorizationToken()
        {
            if (!string.IsNullOrEmpty(_accessToken) && _tokenExpiry > DateTime.UtcNow)
            {
                return _accessToken;
            }

            var client = _httpClientFactory.CreateClient("AuthorizedRAIDClient");

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

            var response = await client.PostAsync("Auth/login", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(responseString);

            _accessToken = loginResponse.data.accessToken;

            // assuming your API gives expiry (if not, set manually e.g. 1 hour)
            _tokenExpiry = DateTime.UtcNow.AddMinutes(50);

            return _accessToken;
        }

    }

}


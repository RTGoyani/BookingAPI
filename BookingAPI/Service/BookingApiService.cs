using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BookingAPI.Service
{
    public class BookingApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public BookingApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GenerateTokenAsync()
        {
            var clientId = _configuration["BookingApi:ClientId"];
            var clientSecret = _configuration["BookingApi:ClientSecret"];

            var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var response = await _httpClient.PostAsync("https://api.booking.com/oauth/token", new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded"));
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(content);

            return tokenResponse.AccessToken;
        }

        public async Task<List<string>> GetCitiesAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("https://api.booking.com/cities");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var citiesResponse = JsonConvert.DeserializeObject<CitiesResponse>(content);

            return citiesResponse.Cities;
        }
    }

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }

    public class CitiesResponse
    {
        [JsonProperty("cities")]
        public List<string> Cities { get; set; }
    }
}

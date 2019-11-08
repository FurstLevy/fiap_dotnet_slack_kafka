using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Webhook.Slack.Worker.Services
{
    public class SlackService : ISlackService
    {
        private readonly ILogger<SlackService> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _serviceUrl;

        public SlackService(ILogger<SlackService> logger, HttpClient httpClient, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = httpClient;

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var baseUrl = configuration.GetSection("SlackBaseUrl").Value;
            _serviceUrl = configuration.GetSection("SlackServiceUrl").Value;

            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task PostSlackAsync(string payload)
        {
            string responseBody = null;

            try
            {
                string t = "{\"text\":\"Hello, World!\"}";
                var stringContent = new StringContent(TextToSlack(payload), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_serviceUrl, stringContent);

                responseBody = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Response:{responseBody}. Ex: {ex.Message}");
            }
        }

        private static string TextToSlack(string paylod)
        {
            var text = new
            {
                text = paylod
            };

            return JsonSerializer.Serialize(text);
        }
    }
}

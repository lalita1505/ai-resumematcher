using AI.ResumeMatcher.Models;
using AI.ResumeMatcher.Services.Interfaces;
using Google.GenAI.Types;
using System.Net.Http.Headers;
using System.Text.Json;

namespace AI.ResumeMatcher.Services
{
    public class OpenAIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public OpenAIService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<MatchResponse> GetMatchAnalysisAsync(string prompt)
        {
            var apiKey = _config["OpenAI:ApiKey"];

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var request = new
            {
                model = "gpt-4",
                messages = new[] 
                {
                    new { role = "user", content = prompt }
                }
            };

            var response = await _httpClient.PostAsJsonAsync(
                "https://api.openai.com/v1/chat/completions",
                request);

            var content = await response.Content.ReadAsStringAsync();

            // Deserialize JSON properly
            return ParseResponse(content);
        }

        public async Task<MatchResponse> AnalyzeAsync(string prompt)
        {
            var apiKey = _config["OpenAI:ApiKey"];

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var request = new
            {
                model = "gpt-4o-mini", // cost-effective
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var response = await _httpClient.PostAsJsonAsync(
                "https://api.openai.com/v1/chat/completions",
                request);

            var result = await response.Content.ReadAsStringAsync();

            return ParseResponse(result);
        }

        private MatchResponse ParseResponse(string json)
        {
            using var doc = JsonDocument.Parse(json);

            var content = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            // Extract JSON from AI response
            int start = content.IndexOf("{");
            int end = content.LastIndexOf("}");

            var cleanJson = content.Substring(start, end - start + 1);

            return JsonSerializer.Deserialize<MatchResponse>(cleanJson);
        }

        public Task<MatchResult> MatchAsync(List<Content> messages)
        {
            throw new NotImplementedException();
        }
    }
}

using AI.ResumeMatcher.Models;
using AI.ResumeMatcher.Services.Interfaces;
using Google.GenAI.Types;
using System.Text.Json;

namespace AI.ResumeMatcher.Services
{
    public class OllamaService : IAIService
    {
        private readonly HttpClient _httpClient;

        public OllamaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<MatchResponse> AnalyzeAsync(string prompt)
        {
            var request = new
            {
                model = "llama3",//"mistral"
                prompt = prompt,
                stream = false
            };

            var response = await _httpClient.PostAsJsonAsync(
                "http://localhost:11434/api/generate",
                request);

            var text = await response.Content.ReadAsStringAsync();

            // Ollama returns raw text → extract JSON manually
            var json = ExtractJson(text);

            return JsonSerializer.Deserialize<MatchResponse>(json);
        }

        public Task<MatchResponse> GetMatchAnalysisAsync(string prompt)
        {
            throw new NotImplementedException();
        }

        public Task<MatchResponse> MatchAsync(string prompt)
        {
            throw new NotImplementedException();
        }

        public Task<MatchResult> MatchAsync(List<Content> messages)
        {
            throw new NotImplementedException();
        }

        private string ExtractJson(string text)
        {
            int start = text.IndexOf("{");
            int end = text.LastIndexOf("}");
            return text.Substring(start, end - start + 1);
        }
    }
}

using AI.ResumeMatcher.Models;
using AI.ResumeMatcher.Services.Interfaces;
using Google.GenAI;
using Google.GenAI.Types;
using System.Text.Json;

namespace AI.ResumeMatcher.Services
{
    public class GeminiAIService : IAIService
    {
        private readonly IConfiguration _config;

        public GeminiAIService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<MatchResult> MatchAsync(List<Content> messages)
        {
            try
            {
                var apiKey = _config["Gemini:ApiKey"];

                if (string.IsNullOrEmpty(apiKey))
                    throw new InvalidOperationException("Gemini API key is missing");

                var _client = new Client(apiKey: apiKey);


                var response = await _client.Models.GenerateContentAsync(
                    model: "gemini-2.5-flash-lite",
                    contents: messages
                );

                if (response != null && !string.IsNullOrWhiteSpace(response.Text))
                {
                    // Clean the AI response
                    var json = ExtractJson(response.Text);

                    var result = JsonSerializer.Deserialize<MatchResult>(
                        json,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    return result ?? new MatchResult();
                }

                return new MatchResult();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        private string ExtractJson(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new InvalidOperationException("AI response is empty");

            // Remove Markdown code fences if present
            text = text.Trim();

            if (text.StartsWith("```"))
            {
                var firstBrace = text.IndexOf('{');
                var lastBrace = text.LastIndexOf('}');

                if (firstBrace >= 0 && lastBrace > firstBrace)
                    text = text.Substring(firstBrace, lastBrace - firstBrace + 1);
            }

            return text;
        }
    }
}

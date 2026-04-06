using AI.ResumeMatcher.Models;
using Google.GenAI.Types;

namespace AI.ResumeMatcher.Services.Interfaces
{
    public interface IAIService
    {
        Task<MatchResponse> AnalyzeAsync(string prompt);
        Task<MatchResponse> GetMatchAnalysisAsync(string prompt);
        Task<MatchResult> MatchAsync(List<Content> messages);
    }
}

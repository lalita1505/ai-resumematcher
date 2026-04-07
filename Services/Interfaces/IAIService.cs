using AI.ResumeMatcher.Models;
using Google.GenAI.Types;

namespace AI.ResumeMatcher.Services.Interfaces
{
    public interface IAIService
    {
        Task<MatchResult> MatchAsync(List<Content> messages);
    }
}

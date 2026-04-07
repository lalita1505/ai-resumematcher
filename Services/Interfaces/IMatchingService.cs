using AI.ResumeMatcher.Models;

namespace AI.ResumeMatcher.Services.Interfaces
{
    public interface IMatchingService
    {
        Task<MatchResult> MatchAsync(MatchRequest request);
    }
}

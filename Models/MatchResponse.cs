namespace AI.ResumeMatcher.Models
{
    public class MatchResponse
    {
        public int Score { get; set; }
        public string? Recommendation { get; set; }
        public List<RequirementResult> Requirements { get; set; } = [];
        public string? Summary { get; set; }
    }

    public class RequirementResult
    {
        public string? Requirement { get; set; }
        public string? Status { get; set; }
        public string? Evidence { get; set; }
    }

    public class MatchResult
    {
        public string? CandidateName { get; set; }
        public int MatchScore { get; set; }
        public string? Decision { get; set; }

        public List<string> MatchingSkills { get; set; } = new();
        public List<string> MissingSkills { get; set; } = new();

        public string? ExperienceSummary { get; set; }

        public List<string> Gaps { get; set; } = new();
        public List<string> Reasoning { get; set; } = new();
        public List<string> Improvements { get; set; } = new();
    }

}

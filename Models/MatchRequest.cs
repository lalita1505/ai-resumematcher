using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AI.ResumeMatcher.Models
{
    public class MatchRequest
    {
        public string? JobDescription { get; set; }
        public IFormFile? ResumeFile { get; set; }
    }
}

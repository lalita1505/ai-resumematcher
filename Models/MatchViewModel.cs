using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AI.ResumeMatcher.Models
{
    public class MatchViewModel
    {
        public MatchRequest Request { get; set; }

        [ValidateNever]
        public MatchResult Result { get; set; }
    }
}

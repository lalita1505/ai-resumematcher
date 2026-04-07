using AI.ResumeMatcher.Validations;
using System.ComponentModel.DataAnnotations;

namespace AI.ResumeMatcher.Models
{
    public class MatchRequest
    {
        [Required(ErrorMessage = "Job Description is required")]
        [MinLength(50, ErrorMessage = "Job Description must be at least 50 characters")]
        public string? JobDescription { get; set; }

        [Required(ErrorMessage = "Please upload a resume")]
        [ValidFile(new[] { ".pdf", ".docx", ".txt" }, 2 * 1024 * 1024)]
        public IFormFile? ResumeFile { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace AI.ResumeMatcher.Validations
{
    public class ValidFileAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;
        private readonly long _maxSize;

        public ValidFileAttribute(string[] extensions, long maxSize)
        {
            _allowedExtensions = extensions;
            _maxSize = maxSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var file = value as IFormFile;

            if (file == null)
                return new ValidationResult("File is required");

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!_allowedExtensions.Contains(extension))
            {
                return new ValidationResult("Invalid file type. Only PDF, DOCX, TXT allowed.");
            }

            if (file.Length > _maxSize)
            {
                return new ValidationResult("File size exceeds limit (2MB).");
            }

            return ValidationResult.Success;
        }
    }
}

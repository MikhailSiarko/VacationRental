using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Domain
{
    public class ValidationResult
    {
        protected ValidationResult(List<string> errors)
        {
            Errors = errors;
        }

        protected ValidationResult()
        {
            Errors = new List<string>();
        }
        
        public List<string> Errors { get; }

        public bool Failed => Errors != null && Errors.Any();
        
        public static ValidationResult Success() => new ValidationResult();

        public static ValidationResult Failure(List<string> errors) => new ValidationResult(errors);
        
        public static ValidationResult Failure(string error) => new ValidationResult(new List<string> { error });
    }
}
using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Domain
{
    public class Result<T>
    {
        protected Result(T value)
        {
            Value = value;
            Errors = new List<string>();
        }

        protected Result(List<string> errors, bool isValidationFailure = false)
        {
            Errors = errors;
            IsValidationFailure = isValidationFailure;
        }

        public T Value { get; }

        public List<string> Errors { get; }

        public bool HasErrors => Errors != null && Errors.Any();

        public bool IsValidationFailure { get; }

        public bool IsEmpty => Value == null;

        public static Result<T> Success(T value) => new Result<T>(value);

        public static Result<T> Failure(List<string> errors, bool isValidationFailure = false) => new Result<T>(errors, isValidationFailure);

        public static Result<T> Failure(string error, bool isValidationFailure = false) => new Result<T>(new List<string> {error}, isValidationFailure);
    }
}
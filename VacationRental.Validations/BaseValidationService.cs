using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain;
using VacationRental.Domain.Abstractions;
using VacationRental.Domain.Models;

namespace VacationRental.Validations
{
    public class BaseValidationService
    {
        protected readonly IStorage<int, Rental> Rentals;

        public BaseValidationService(IStorage<int, Rental> rentals)
        {
            Rentals = rentals;
        }

        protected ValidationResult Validate(int rentalId, int nights)
        {
            var errors = new List<string>();
            if (nights <= 0)
                errors.Add("Nights must be positive");
            if (Rentals.Get(rentalId).Value == null)
                errors.Add("Rental not found");

            if (errors.Any())
                return ValidationResult.Failure(errors);
            
            return ValidationResult.Success();
        }
    }
}
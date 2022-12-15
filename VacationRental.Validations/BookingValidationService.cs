using System;
using VacationRental.Domain;
using VacationRental.Domain.Abstractions;
using VacationRental.Domain.Abstractions.Services;
using VacationRental.Domain.Models;

namespace VacationRental.Validations
{
    public class BookingValidationService : BaseValidationService, IBookingValidationService
    {
        public BookingValidationService(IStorage<int, Rental> rentals) : base(rentals)
        {
        }

        public ValidationResult Validate(int rentalId, DateTime start, int nights)
        {
            var baseValidationResult = Validate(rentalId, nights);

            if (baseValidationResult.Failed)
                return baseValidationResult;

            return ValidationResult.Success();
        }
    }
}
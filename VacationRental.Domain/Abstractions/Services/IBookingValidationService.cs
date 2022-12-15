using System;

namespace VacationRental.Domain.Abstractions.Services
{
    public interface IBookingValidationService
    {
        ValidationResult Validate(int rentalId, DateTime start, int nights);
    }
}
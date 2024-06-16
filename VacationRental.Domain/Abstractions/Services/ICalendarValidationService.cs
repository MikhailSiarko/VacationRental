using System;

namespace VacationRental.Domain.Abstractions.Services
{
    public interface ICalendarValidationService
    {
        ValidationResult Validate(int rentalId, DateTime start, int nights);
    }
}
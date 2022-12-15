using System;
using VacationRental.Domain.Models;

namespace VacationRental.Domain.Abstractions.Services
{
    public interface ICalendarService
    {
        Result<Calendar> Get(int rentalId, DateTime start, int nights);
    }
}
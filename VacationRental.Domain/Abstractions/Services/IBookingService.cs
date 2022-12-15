using System;
using VacationRental.Domain.Models;

namespace VacationRental.Domain.Abstractions.Services
{
    public interface IBookingService
    {
        Result<Booking> Get(int id);

        Result<Booking> Create(int rentalId, DateTime start, int nights);
    }
}
using VacationRental.Domain.Models;

namespace VacationRental.Domain.Abstractions.Services
{
    public interface IRentalService
    {
        Result<Rental> Get(int id);

        Result<Rental> Create(int units, int preparationDays);

        Result<Rental> Update(int rentalId, int units, int preparationDays);
    }
}
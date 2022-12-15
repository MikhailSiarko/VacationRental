using System;
using System.Collections.Generic;
using VacationRental.Domain.Models;

namespace VacationRental.Domain.Abstractions.Services
{
    public interface IUnitAvailabilityService
    {
        Result<int> GetAvailable(Rental rental, IList<Occupation> occupations, DateTime start, int nights);

        bool Validate(Rental rental, IList<Occupation> occupations);
    }
}
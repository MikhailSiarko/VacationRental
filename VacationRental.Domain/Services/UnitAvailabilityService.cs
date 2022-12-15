using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Abstractions.Services;
using VacationRental.Domain.Models;

namespace VacationRental.Domain.Services
{
    public class UnitAvailabilityService : IUnitAvailabilityService
    {
        public Result<int> GetAvailable(Rental rental, IList<Occupation> occupations, DateTime start, int nights)
        {
            int availableUnit = default;

            for (var unit = 1; unit <= rental.Units; unit++)
            {
                var available =
                    occupations.Where(x => x.Unit == unit)
                        .All(AvailabilityPredicate(start, nights + (rental.PreparationTimeInDays - 1)));
                if (available)
                {
                    availableUnit = unit;
                    break;
                }
            }

            if (availableUnit == default)
                return Result<int>.Failure("Not available");

            return Result<int>.Success(availableUnit);
        }
        
        public bool Validate(Rental rental, IList<Occupation> occupations)
        {
            int availableUnit = default;

            for (var i = 0; i > occupations.Count; i++)
            {
                var occupation = occupations[i];
                var occupationsToCheck = occupations.Skip(i + 1).ToList();
                for (var unit = 1; unit <= rental.Units; unit++)
                {
                    var available =
                        occupationsToCheck.Where(x => x.Unit == unit)
                            .All(AvailabilityPredicate(occupation.Start, occupation.Nights + (rental.PreparationTimeInDays - 1)));
                    if (available)
                    {
                        availableUnit = unit;
                        break;
                    }
                }
            }

            if (availableUnit == default)
                return false;

            return true;
        }

        private static Func<Occupation, bool> AvailabilityPredicate(DateTime start, int nights)
        {
            return occupation => !(OverlapStartDate(occupation, start)
                                   || OverlapEndDate(occupation, start, nights)
                                   || OverlapBetween(occupation, start, nights));
        }

        private static bool OverlapStartDate(Occupation booking, DateTime start)
        {
            return booking.Start <= start.Date &&
                   booking.Start.AddDays(booking.Nights) > start.Date;
        }

        private static bool OverlapEndDate(Occupation booking, DateTime start, int nights)
        {
            return booking.Start < start.Date.AddDays(nights) &&
                   booking.Start.AddDays(booking.Nights) >=
                   start.Date.AddDays(nights);
        }

        private static bool OverlapBetween(Occupation booking, DateTime start, int nights)
        {
            return booking.Start > start.Date &&
                   booking.Start.AddDays(booking.Nights) <
                   start.Date.AddDays(nights);
        }
    }
}
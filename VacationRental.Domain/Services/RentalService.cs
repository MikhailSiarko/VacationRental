using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Abstractions;
using VacationRental.Domain.Abstractions.Services;
using VacationRental.Domain.Models;

namespace VacationRental.Domain.Services
{
    public class RentalService : IRentalService
    {
        private readonly IStorage<int, Rental> _rentals;
        private readonly IStorage<int, Booking> _bookings;
        private readonly IStorage<int, Preparation> _preparations;
        private readonly IUnitAvailabilityService _unitAvailabilityService;

        public RentalService(IStorage<int, Rental> rentals, IStorage<int, Booking> bookings,
            IStorage<int, Preparation> preparations,
            IUnitAvailabilityService unitAvailabilityService)
        {
            _rentals = rentals;
            _bookings = bookings;
            _preparations = preparations;
            _unitAvailabilityService = unitAvailabilityService;
        }

        public Result<Rental> Get(int id)
        {
            return _rentals.Get(id);
        }

        public Result<Rental> Create(int units, int preparationDays)
        {
            return _rentals.Save(new Rental
            {
                Units = units,
                PreparationTimeInDays = preparationDays
            });
        }

        public Result<Rental> Update(int rentalId, int units, int preparationDays)
        {
            var rentalResult = _rentals.Get(rentalId);

            if (rentalResult.HasErrors)
                return Result<Rental>.Failure(rentalResult.Errors);

            var bookingsResult = _bookings.Get();

            if (bookingsResult.HasErrors)
                return Result<Rental>.Failure(rentalResult.Errors);

            var occupations = new List<Occupation>();

            occupations.AddRange(bookingsResult.Value.Where(x => x.RentalId == rentalId));

            var updatedPreparations = CreateUpdatedPreparations(rentalId, preparationDays, occupations);

            var updatedRental = new Rental
            {
                Id = rentalId,
                Units = units,
                PreparationTimeInDays = preparationDays
            };

            var isValid = _unitAvailabilityService.Validate(updatedRental, occupations);

            var failedResult = Result<Rental>.Failure("Rental can't be updated");

            if (isValid)
            {
                return UpdateRental(failedResult, updatedPreparations, updatedRental);
            }

            return failedResult;
        }

        private Result<Rental> UpdateRental(Result<Rental> failedResult, List<Preparation> updatedPreparations,
            Rental updatedRental)
        {
            var preparationsResult = _preparations.Get();
            if (preparationsResult.HasErrors)
                return failedResult;

            _preparations.Delete(preparationsResult.Value.Select(x => x.Id));

            foreach (var updatedPreparation in updatedPreparations)
            {
                var savePreparationResult = _preparations.Save(updatedPreparation);
                if (savePreparationResult.HasErrors)
                    return failedResult;
            }

            var saveResult = _rentals.Update(updatedRental);

            if (saveResult.HasErrors)
                return failedResult;

            return saveResult;
        }

        private static List<Preparation> CreateUpdatedPreparations(int rentalId, int preparationDays,
            List<Occupation> occupations)
        {
            var updatedPreparations = new List<Preparation>();

            if (preparationDays > 0)
            {
                updatedPreparations = occupations.Select(o => new Preparation
                    {
                        RentalId = rentalId,
                        Nights = preparationDays,
                        Unit = o.Unit,
                        Start = o.Start.AddDays(o.Nights)
                    })
                    .ToList();

                occupations.AddRange(updatedPreparations);
            }

            return updatedPreparations;
        }
    }
}
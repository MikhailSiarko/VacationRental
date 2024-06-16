using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Abstractions;
using VacationRental.Domain.Abstractions.Services;
using VacationRental.Domain.Models;

namespace VacationRental.Domain.Services
{
    public class BookingService : IBookingService
    {
        private readonly IStorage<int, Booking> _bookings;
        private readonly IStorage<int, Preparation> _preparations;
        private readonly IStorage<int, Rental> _rentals;
        private readonly IUnitAvailabilityService _unitAvailabilityService;
        private readonly IBookingValidationService _bookingValidationService;

        public BookingService(IStorage<int, Booking> bookings,
            IStorage<int, Rental> rentals,
            IStorage<int, Preparation> preparations,
            IUnitAvailabilityService unitAvailabilityService,
            IBookingValidationService bookingValidationService)
        {
            _bookings = bookings;
            _rentals = rentals;
            _preparations = preparations;
            _unitAvailabilityService = unitAvailabilityService;
            _bookingValidationService = bookingValidationService;
        }

        public Result<Booking> Get(int id)
        {
            return _bookings.Get(id);
        }

        public Result<Booking> Create(int rentalId, DateTime start, int nights)
        {
            if (DataPreparation(rentalId, start, nights, out var rental, out var bookings,
                    out var preparations, out var result)) return result;

            var occupations = new List<Occupation>();
            occupations.AddRange(bookings
                .Where(booking => booking.RentalId == rentalId));
            occupations.AddRange(preparations.Where(preparation => preparation.RentalId == rentalId));

            var availableUnitResult =
                _unitAvailabilityService.GetAvailable(rental, occupations, start, nights);

            if (availableUnitResult.HasErrors)
                return Result<Booking>.Failure(availableUnitResult.Errors);

            var saveBookingResult = _bookings.Save(new Booking
            {
                RentalId = rentalId,
                Nights = nights,
                Start = start,
                Unit = availableUnitResult.Value
            });

            if (saveBookingResult.HasErrors)
                return saveBookingResult;

            if (rental.PreparationTimeInDays > 0)
            {
                var savePreparationResult = _preparations.Save(new Preparation
                {
                    RentalId = rentalId,
                    Nights = rental.PreparationTimeInDays,
                    Unit = availableUnitResult.Value,
                    Start = start.AddDays(nights)
                });

                if (savePreparationResult.HasErrors)
                    return Result<Booking>.Failure(savePreparationResult.Errors);
            }

            return saveBookingResult;
        }

        private bool DataPreparation(int rentalId, DateTime start, int nights, out Rental rental,
            out IEnumerable<Booking> bookings, out IEnumerable<Preparation> preparations,
            out Result<Booking> result)
        {
            var validationResult = _bookingValidationService.Validate(rentalId, start, nights);

            if (validationResult.Failed)
            {
                {
                    result = Result<Booking>.Failure(validationResult.Errors, true);
                    rental = null;
                    preparations = null;
                    bookings = null;
                    return true;
                }
            }

            var rentalResult = _rentals.Get(rentalId);

            if (rentalResult.HasErrors)
            {
                result = Result<Booking>.Failure(rentalResult.Errors);
                rental = null;
                preparations = null;
                bookings = null;
                return true;
            }

            rental = rentalResult.Value;

            var bookingsResult = _bookings.Get();

            if (bookingsResult.HasErrors)
            {
                result = Result<Booking>.Failure(bookingsResult.Errors);
                bookings = null;
                preparations = null;
                return true;
            }

            bookings = bookingsResult.Value;

            var preparationsResult = _preparations.Get();

            if (preparationsResult.HasErrors)
            {
                result = Result<Booking>.Failure(preparationsResult.Errors);
                preparations = null;
                return true;
            }

            preparations = preparationsResult.Value;

            result = null;

            return false;
        }
    }
}
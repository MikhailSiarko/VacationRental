using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Domain.Abstractions;
using VacationRental.Domain.Abstractions.Services;
using VacationRental.Domain.Models;

namespace VacationRental.Domain.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IStorage<int, Booking> _bookings;
        private readonly IStorage<int, Rental> _rentals;
        private readonly IStorage<int, Preparation> _preparations;
        private readonly ICalendarValidationService _calendarValidationService;

        public CalendarService(IStorage<int, Booking> bookings, IStorage<int, Rental> rentals,
            IStorage<int, Preparation> preparations,
            ICalendarValidationService calendarValidationService)
        {
            _bookings = bookings;
            _rentals = rentals;
            _preparations = preparations;
            _calendarValidationService = calendarValidationService;
        }

        public Result<Calendar> Get(int rentalId, DateTime start, int nights)
        {
            if (DataPreparation(rentalId, start, nights, out var calendar, out var bookings,
                    out var preparations, out var result)) return result;

            var occupations = new List<Occupation>();
            occupations.AddRange(bookings
                .Where(booking => booking.RentalId == rentalId));
            occupations.AddRange(preparations.Where(preparation => preparation.RentalId == rentalId));

            BuildDates(start, nights, occupations, calendar);

            return Result<Calendar>.Success(calendar);
        }

        private bool DataPreparation(int rentalId, DateTime start, int nights, out Calendar calendar,
            out IEnumerable<Booking> bookings,
            out IEnumerable<Preparation> preparations, out Result<Calendar> result)
        {
            var validationResult = _calendarValidationService.Validate(rentalId, start, nights);

            if (validationResult.Failed)
            {
                {
                    result = Result<Calendar>.Failure(validationResult.Errors, true);
                    calendar = null;
                    bookings = null;
                    preparations = null;
                    return true;
                }
            }

            var rentalResult = _rentals.Get(rentalId);

            if (rentalResult.HasErrors)
            {
                result = Result<Calendar>.Failure(rentalResult.Errors);
                calendar = null;
                bookings = null;
                preparations = null;
                return true;
            }

            calendar = new Calendar
            {
                RentalId = rentalId,
                Dates = new List<CalendarDate>()
            };

            var bookingsResult = _bookings.Get();

            if (bookingsResult.HasErrors)
            {
                result = Result<Calendar>.Failure(bookingsResult.Errors);
                bookings = null;
                preparations = null;
                return true;
            }

            bookings = bookingsResult.Value;

            var preparationResult = _preparations.Get();

            if (preparationResult.HasErrors)
            {
                result = Result<Calendar>.Failure(preparationResult.Errors);
                preparations = null;
                return true;
            }

            preparations = preparationResult.Value;
            result = null;

            return false;
        }

        private static void BuildDates(DateTime start, int nights,
            IList<Occupation> occupations, Calendar result)
        {
            for (var i = 0; i < nights; i++)
            {
                var date = new CalendarDate
                {
                    Date = start.Date.AddDays(i),
                    Bookings = new List<CalendarBooking>(),
                    PreparationTimes = new List<CalendarPreparation>()
                };

                foreach (var occupation in occupations)
                {
                    if (occupation.Start <= date.Date && occupation.Start.AddDays(occupation.Nights) > date.Date)
                    {
                        AddOccupation(occupation, date);
                    }
                }

                result.Dates.Add(date);
            }
        }

        private static void AddOccupation(Occupation occupation, CalendarDate date)
        {
            switch (occupation.Type)
            {
                case OccupationType.Booking:
                    date.Bookings.Add(new CalendarBooking
                    {
                        Id = occupation.Id,
                        Unit = occupation.Unit
                    });
                    break;
                case OccupationType.Preparation:
                    date.PreparationTimes.Add(new CalendarPreparation
                    {
                        Unit = occupation.Unit
                    });
                    break;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using VacationRental.Domain.Models;
using VacationRental.Domain.Services;
using Xunit;

namespace VacationsRental.Domain.Tests
{
    public class BookingTests
    {
        [Fact]
        public void GivenOneNightAvailableInUnit2_PostBookingForOneNight_ReturnsSuccessResult()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking
                {
                    Id = 1,
                    Start = new DateTime(2022, 12, 1),
                    Nights = 5,
                    RentalId = 1,
                    Unit = 2
                },
                new Booking
                {
                    Id = 2,
                    Start = new DateTime(2022, 12, 9),
                    Nights = 5,
                    RentalId = 1,
                    Unit = 2
                },
                new Booking
                {
                    Id = 3,
                    Start = new DateTime(2022, 12, 4),
                    Nights = 5,
                    RentalId = 1,
                    Unit = 1
                }
            };

            var preparations = new List<Preparation>
            {
                new Preparation
                {
                    Id = 1,
                    Start = new DateTime(2022, 12, 6),
                    Nights = 1,
                    RentalId = 1,
                    Unit = 2
                },
                new Preparation
                {
                    Id = 2,
                    Start = new DateTime(2022, 12, 14),
                    Nights = 1,
                    RentalId = 1,
                    Unit = 2
                },
                new Preparation
                {
                    Id = 3,
                    Start = new DateTime(2022, 12, 9),
                    Nights = 5,
                    RentalId = 1,
                    Unit = 1
                }
            };

            var occupations = new List<Occupation>();
            occupations.AddRange(bookings);
            occupations.AddRange(preparations);

            var rental = new Rental
            {
                Id = 1,
                Units = 2,
                PreparationTimeInDays = 1
            };

            var unitAvailabilityService = new UnitAvailabilityService();

            var booking = new Booking
            {
                Id = 4,
                Start = new DateTime(2022, 12, 7),
                Nights = 1,
                RentalId = 1,
                Unit = 2
            };

            // Act
            var result = unitAvailabilityService.GetAvailable(rental, occupations, booking.Start, booking.Nights);

            // Assert
            Assert.False(result.HasErrors);
            Assert.Equal(result.Value, booking.Unit);
        }

        [Fact]
        public void GivenOneNightAvailable_PostBookingForTwoNights_ReturnsFailedResult()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking
                {
                    Id = 1,
                    Start = new DateTime(2022, 12, 1),
                    Nights = 5,
                    RentalId = 1,
                    Unit = 1
                },
                new Booking
                {
                    Id = 2,
                    Start = new DateTime(2022, 12, 9),
                    Nights = 5,
                    RentalId = 1,
                    Unit = 1
                },
                new Booking
                {
                    Id = 3,
                    Start = new DateTime(2022, 12, 4),
                    Nights = 5,
                    RentalId = 1,
                    Unit = 2
                }
            };

            var preparations = new List<Preparation>
            {
                new Preparation
                {
                    Id = 1,
                    Start = new DateTime(2022, 12, 7),
                    Nights = 1,
                    RentalId = 1,
                    Unit = 2
                },
                new Preparation
                {
                    Id = 2,
                    Start = new DateTime(2022, 12, 15),
                    Nights = 1,
                    RentalId = 1,
                    Unit = 2
                },
                new Preparation
                {
                    Id = 3,
                    Start = new DateTime(2022, 12, 10),
                    Nights = 5,
                    RentalId = 1,
                    Unit = 1
                }
            };

            var occupations = new List<Occupation>();
            occupations.AddRange(bookings);
            occupations.AddRange(preparations);

            var unitAvailabilityService = new UnitAvailabilityService();

            var rental = new Rental
            {
                Id = 1,
                Units = 2,
                PreparationTimeInDays = 2
            };

            var booking = new Booking
            {
                Id = 4,
                Start = new DateTime(2022, 12, 7),
                Nights = 2,
                RentalId = 1
            };

            // Act
            var result = unitAvailabilityService.GetAvailable(rental, occupations, booking.Start, booking.Nights);

            // Assert
            Assert.True(result.HasErrors);
        }
    }
}
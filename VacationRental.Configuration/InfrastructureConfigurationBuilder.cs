using System.Collections.Generic;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using VacationRental.Domain.Abstractions;
using VacationRental.Domain.Abstractions.Services;
using VacationRental.Domain.Models;
using VacationRental.Domain.Services;
using VacationRental.Storage;
using VacationRental.Storage.Models;
using VacationRental.Validations;

namespace VacationRental.Configuration
{
    public static class InfrastructureConfigurationBuilder
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDictionary<int, BookingStorageModel>>(
                _ => new Dictionary<int, BookingStorageModel>());
            services.AddSingleton<IDictionary<int, RentalStorageModel>>(
                _ => new Dictionary<int, RentalStorageModel>());
            services.AddSingleton<IDictionary<int, PreparationStorageModel>>(
                _ => new Dictionary<int, PreparationStorageModel>());
            services.AddTransient<IStorage<int, Booking>, DefaultBookingStorage>();
            services.AddTransient<IStorage<int, Rental>, DefaultRentalStorage>();
            services.AddTransient<IStorage<int, Preparation>, DefaultPreparationStorage>();
            services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<ICalendarService, CalendarService>();
            services.AddTransient<IRentalService, RentalService>();
            services.AddTransient<IBookingValidationService, BookingValidationService>();
            services.AddTransient<ICalendarValidationService, CalendarValidationService>();
            services.AddTransient<IUnitAvailabilityService, UnitAvailabilityService>();
        }

        public static void ConfigureAutomapper(IServiceCollection serviceCollection, Assembly callingAssembly)
        {
            serviceCollection.AddAutoMapper(callingAssembly, typeof(DefaultBookingStorage).Assembly);
        }
    }
}
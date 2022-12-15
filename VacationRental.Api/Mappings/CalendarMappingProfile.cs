using AutoMapper;
using VacationRental.Api.Models;
using VacationRental.Domain.Models;

namespace VacationRental.Api.Mappings
{
    public class CalendarMappingProfile : Profile
    {
        public CalendarMappingProfile()
        {
            CreateMap<Calendar, CalendarViewModel>();
            CreateMap<CalendarDate, CalendarDateViewModel>();
            CreateMap<CalendarBooking, CalendarBookingViewModel>();
            CreateMap<CalendarPreparation, CalendarPreparationViewModel>();
        }
    }
}
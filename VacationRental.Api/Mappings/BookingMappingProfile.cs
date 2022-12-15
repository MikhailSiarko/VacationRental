using AutoMapper;
using VacationRental.Api.Models;
using VacationRental.Domain.Models;

namespace VacationRental.Api.Mappings
{
    public class BookingMappingProfile : Profile
    {
        public BookingMappingProfile()
        {
            CreateMap<Booking, ResourceIdViewModel>()
                .ForMember(x => x.Id, options => options.MapFrom(y => y.Id));
            CreateMap<Booking, BookingViewModel>();
        }
    }
}
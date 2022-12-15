using AutoMapper;
using VacationRental.Api.Models;
using VacationRental.Domain.Models;

namespace VacationRental.Api.Mappings
{
    public class RentalMappingProfile : Profile
    {
        public RentalMappingProfile()
        {
            CreateMap<Rental, RentalViewModel>();
            CreateMap<Rental, ResourceIdViewModel>()
                .ForMember(x => x.Id, options => options.MapFrom(y => y.Id));
        }
    }
}
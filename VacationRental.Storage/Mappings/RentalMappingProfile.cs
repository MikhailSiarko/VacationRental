using AutoMapper;
using VacationRental.Domain.Models;
using VacationRental.Storage.Models;

namespace VacationRental.Storage.Mappings
{
    public class RentalMappingProfile : Profile
    {
        public RentalMappingProfile()
        {
            CreateMap<RentalStorageModel, Rental>().ReverseMap();
        }
    }
}
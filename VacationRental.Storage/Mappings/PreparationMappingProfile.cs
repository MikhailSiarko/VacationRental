using AutoMapper;
using VacationRental.Domain.Models;
using VacationRental.Storage.Models;

namespace VacationRental.Storage.Mappings
{
    public class PreparationMappingProfile : Profile
    {
        public PreparationMappingProfile()
        {
            CreateMap<PreparationStorageModel, Preparation>().ReverseMap();
        }
    }
}
using AutoMapper;
using VacationRental.Domain.Models;
using VacationRental.Storage.Models;

namespace VacationRental.Storage.Mappings
{
    public class BookingMappingProfile : Profile
    {
        public BookingMappingProfile()
        {
            CreateMap<BookingStorageModel, Booking>().ReverseMap();
        }
    }
}
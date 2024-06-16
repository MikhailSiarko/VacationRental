using System.Collections.Generic;
using AutoMapper;
using VacationRental.Domain.Models;
using VacationRental.Storage.Abstractions;
using VacationRental.Storage.Models;

namespace VacationRental.Storage
{
    public class DefaultBookingStorage : DefaultStorage<int, Booking, BookingStorageModel>
    {
        public DefaultBookingStorage(IDictionary<int, BookingStorageModel> dictionary, IMapper mapper) : base(dictionary, mapper)
        {
        }

        protected override int GetKeyFromModel(Booking model)
        {
            return model.Id;
        }

        protected override int GetKeyFromStorageModel(BookingStorageModel model)
        {
            return model.Id;
        }

        protected override BookingStorageModel CreateStorageModel(Booking model)
        {
            return new BookingStorageModel
            {
                Id = Dictionary.Count + 1,
                RentalId = model.RentalId,
                Start = model.Start,
                Nights = model.Nights,
                Unit = model.Unit
            };
        }
    }
}
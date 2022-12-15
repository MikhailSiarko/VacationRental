using System.Collections.Generic;
using AutoMapper;
using VacationRental.Domain.Models;
using VacationRental.Storage.Abstractions;
using VacationRental.Storage.Models;

namespace VacationRental.Storage
{
    public class DefaultRentalStorage : DefaultStorage<int, Rental, RentalStorageModel>
    {
        public DefaultRentalStorage(IDictionary<int, RentalStorageModel> dictionary, IMapper mapper) : base(dictionary, mapper)
        {
        }

        protected override int GetKeyFromModel(Rental model)
        {
            return model.Id;
        }

        protected override int GetKeyFromStorageModel(RentalStorageModel model)
        {
            return model.Id;
        }

        protected override RentalStorageModel CreateStorageModel(Rental model)
        {
            return new RentalStorageModel
            {
                Id = Dictionary.Count + 1,
                Units = model.Units,
                PreparationTimeInDays = model.PreparationTimeInDays
            };
        }
    }
}
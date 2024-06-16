using System.Collections.Generic;
using AutoMapper;
using VacationRental.Domain.Models;
using VacationRental.Storage.Abstractions;
using VacationRental.Storage.Models;

namespace VacationRental.Storage
{
    public class DefaultPreparationStorage : DefaultStorage<int, Preparation, PreparationStorageModel>
    {
        public DefaultPreparationStorage(IDictionary<int, PreparationStorageModel> dictionary, IMapper mapper) : base(dictionary, mapper)
        {
        }

        protected override int GetKeyFromModel(Preparation model)
        {
            return model.Id;
        }

        protected override int GetKeyFromStorageModel(PreparationStorageModel model)
        {
            return model.Id;
        }

        protected override PreparationStorageModel CreateStorageModel(Preparation model)
        {
            return new PreparationStorageModel
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
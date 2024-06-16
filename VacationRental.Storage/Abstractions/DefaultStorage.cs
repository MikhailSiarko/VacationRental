using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using VacationRental.Domain;
using VacationRental.Domain.Abstractions;

namespace VacationRental.Storage.Abstractions
{
    public abstract class DefaultStorage<TKey, TModel, TStorageModel> : IStorage<TKey, TModel>
    {
        protected readonly IDictionary<TKey, TStorageModel> Dictionary;
        private readonly IMapper _mapper;

        public DefaultStorage(IDictionary<TKey, TStorageModel> dictionary, IMapper mapper)
        {
            Dictionary = dictionary;
            _mapper = mapper;
        }

        public Result<IEnumerable<TModel>> Get()
        {
            return Result<IEnumerable<TModel>>.Success(Dictionary.Values.Select(model =>
                _mapper.Map<TStorageModel, TModel>(model)));
        }

        public Result<TModel> Get(TKey key)
        {
            if (Dictionary.ContainsKey(key))
            {
                return Result<TModel>.Success(_mapper.Map<TStorageModel, TModel>(Dictionary[key]));
            }

            return Result<TModel>.Success(default);
        }

        public Result<TModel> Save(TModel entity)
        {
            try
            {
                var model = CreateStorageModel(entity);

                Dictionary.Add(GetKeyFromStorageModel(model), model);

                return Result<TModel>.Success(_mapper.Map<TStorageModel, TModel>(model));
            }
            catch (Exception e)
            {
                return Result<TModel>.Failure(e.Message);
            }
        }

        public Result<TModel> Update(TModel entity)
        {
            try
            {
                var key = GetKeyFromModel(entity);
                if (Dictionary.ContainsKey(key))
                {
                    Dictionary[key] = _mapper.Map<TModel, TStorageModel>(entity);
                    return Result<TModel>.Success(entity);
                }

                return Result<TModel>.Failure($"The entity with ID {key} doesn't exist");
            }
            catch (Exception e)
            {
                return Result<TModel>.Failure(e.Message);
            }
        }

        public Result<TModel> Delete(TKey key)
        {
            try
            {
                if (Dictionary.ContainsKey(key))
                {
                    var booking = Dictionary[key];
                    return Result<TModel>.Success(_mapper.Map<TStorageModel, TModel>(booking));
                }

                return Result<TModel>.Failure($"The entity with ID {key} doesn't exist");
            }
            catch (Exception e)
            {
                return Result<TModel>.Failure(e.Message);
            }
        }

        public Result<IEnumerable<TModel>> Delete(IEnumerable<TKey> keys)
        {
            var errors = new List<string>();
            var models = new List<TModel>();
            foreach (var key in keys)
            {
                var result = Delete(key);
                if (result.HasErrors)
                {
                    errors.AddRange(result.Errors);
                    continue;
                }
                models.Add(result.Value);
            }

            return errors.Any() ? Result<IEnumerable<TModel>>.Failure(errors) : Result<IEnumerable<TModel>>.Success(models);
        }

        protected abstract TKey GetKeyFromModel(TModel model);

        protected abstract TKey GetKeyFromStorageModel(TStorageModel model);

        protected abstract TStorageModel CreateStorageModel(TModel model);
    }
}
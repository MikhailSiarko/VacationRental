using System.Collections.Generic;

namespace VacationRental.Domain.Abstractions
{
    public interface IStorage<in TKey, TModel>
    {
        Result<IEnumerable<TModel>> Get();
        Result<TModel> Get(TKey key);
        Result<TModel> Save(TModel entity);
        Result<TModel> Update(TModel entity);
        Result<TModel> Delete(TKey key);
        Result<IEnumerable<TModel>> Delete(IEnumerable<TKey> keys);
    }
}
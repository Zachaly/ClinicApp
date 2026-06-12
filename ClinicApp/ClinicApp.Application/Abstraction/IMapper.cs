using ClinicApp.Domain.Entity;

namespace ClinicApp.Application.Abstraction;

public interface IModelMapper<in TEntity, out TModel> where TEntity : class, IEntity
{
    TModel MapEntityToModel(TEntity entity);
}

public interface IRequestMapper<out TEntity, in TAddRequest> where TEntity : class, IEntity
{
    TEntity MapRequestToEntity(TAddRequest request);
}

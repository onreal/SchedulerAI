namespace SchedulerApi.Infrastructure.Persistence.Accessors;

public class MappingContextAccessor
{
    private readonly Dictionary<Type, Dictionary<Guid, object>> _entities = new();

    public T? Get<T>(Guid id) where T : class
    {
        if (_entities.TryGetValue(typeof(T), out var map) && map.TryGetValue(id, out var entity))
            return entity as T;

        return null;
    }

    public void Register<T>(Guid id, T entity) where T : class
    {
        if (!_entities.TryGetValue(typeof(T), out var map))
        {
            map = new Dictionary<Guid, object>();
            _entities[typeof(T)] = map;
        }

        map[id] = entity;
    }
}
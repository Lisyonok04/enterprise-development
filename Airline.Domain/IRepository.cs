namespace Airline.Domain;

/// <summary>
/// Defines a generic repository interface that provides 
/// basic CRUD (Create, Read, Update, Delete) operations.
/// </summary>
/// <typeparam name="TEntity">The type of the entity being managed.</typeparam>
/// <typeparam name="TKey">The type of the entity's unique identifier.</typeparam>
public interface IRepository<TEntity, TKey>
    where TEntity : class
{
    /// <summary>
    /// Creates a new entity in the data store.
    /// </summary>
    /// <param name="entity">The entity instance to create.</param>
    /// <returns>The created entity, typically with an assigned identifier.</returns>
    public Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    /// Retrieves an entity from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>
    /// The entity if found; otherwise, <see langword="null"/>.
    /// </returns>
    public Task<TEntity?> GetAsync(TKey id);

    /// <summary>
    /// Retrieves all entities of the specified type from the data store.
    /// </summary>
    /// <returns>
    /// A list containing all entities of the specified type.
    /// </returns>
    public Task<IList<TEntity>> GetAllAsync();

    /// <summary>
    /// Updates an existing entity in the data store.
    /// </summary>
    /// <param name="entity">The entity instance containing updated data.</param>
    /// <returns>The updated entity.</returns>
    public Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    /// Deletes an entity from the data store by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>
    /// <see langword="true"/> if the entity was successfully deleted;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public Task<bool> DeleteAsync(TKey id);
}
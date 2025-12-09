namespace Airline.Domain;

/// <summary>
/// Defines a generic repository interface that provides 
/// basic CRUD (Create, Read, Update, Delete) operations.
/// </summary>
/// <typeparam name="TEntity">
/// The type of the entity being managed by the repository.
/// </typeparam>
/// <typeparam name="TKey">
/// The type of the entity's unique identifier (e.g., <see cref="string"/> for MongoDB).
/// </typeparam>
public interface IRepository<TEntity, TKey>
    where TEntity : class
{
    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity instance to add.</param>
    /// <returns>The created entity.</returns>
    public Task<TEntity> CreateAsync(TEntity entity);

    /// <summary>
    /// Retrieves an entity from the repository by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>
    /// The entity with the specified identifier, or <see langword="null"/> 
    /// if no entity with such an identifier exists.
    /// </returns>
    public Task<TEntity?> GetAsync(TKey id);

    /// <summary>
    /// Retrieves all entities stored in the repository.
    /// </summary>
    /// <returns>
    /// A list containing all entities in the repository.
    /// </returns>
    public Task<IList<TEntity>> GetAllAsync();

    /// <summary>
    /// Updates an existing entity in the repository.
    /// </summary>
    /// <param name="entity">The entity instance containing updated data.</param>
    /// <returns>The updated entity.</returns>
    public Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    /// Removes an entity from the repository by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns><see langword="true"/> if the entity was deleted; otherwise, <see langword="false"/>.</returns>
    public Task<bool> DeleteAsync(TKey id);
}
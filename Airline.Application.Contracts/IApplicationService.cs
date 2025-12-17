namespace Airline.Application.Contracts;

/// <summary>
/// Defines application service interface that provides 
/// standardized CRUD (Create, Read, Update, Delete) operations for domain entities.
/// Serves as a contract between the application layer and presentation layer.
/// </summary>
/// <typeparam name="TDto">
/// The Data Transfer Object (DTO) type used for read operations.
/// Represents the shape of data exposed to clients.
/// </typeparam>
/// <typeparam name="TCreateUpdateDto">
/// The Data Transfer Object (DTO) type used for create and update operations.
/// Contains only the fields required for mutation.
/// </typeparam>
/// <typeparam name="TKey">
/// The type of the unique identifier for the entity (e.g., <see cref="int"/>, <see cref="Guid"/>).
/// Must be a value type (<see langword="struct"/>).
/// </typeparam>
public interface IApplicationService<TDto, TCreateUpdateDto, TKey>
    where TDto : class
    where TCreateUpdateDto : class
    where TKey : struct
{
    /// <summary>
    /// Creates a new entity in the system.
    /// </summary>
    /// <param name="dto">The data transfer object containing creation data.</param>
    /// <returns>The created entity as a DTO, typically with an assigned identifier.</returns>
    public Task<TDto> CreateAsync(TCreateUpdateDto dto);

    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to retrieve.</param>
    /// <returns>The entity as a DTO if found; otherwise, <see langword="null"/>.</returns>
    public Task<TDto?> GetByIdAsync(TKey id);

    /// <summary>
    /// Retrieves all entities of the specified type.
    /// </summary>
    /// <returns>A list of all entities as DTOs.</returns>
    public Task<IList<TDto>> GetAllAsync();

    /// <summary>
    /// Updates an existing entity with new data.
    /// </summary>
    /// <param name="dto">The data transfer object containing updated data.</param>
    /// <param name="id">The unique identifier of the entity to update.</param>
    /// <returns>The updated entity as a DTO.</returns>
    public Task<TDto> UpdateAsync(TCreateUpdateDto dto, TKey id);

    /// <summary>
    /// Deletes an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>
    /// <see langword="true"/> if the entity was successfully deleted;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public Task<bool> DeleteAsync(TKey id);
}
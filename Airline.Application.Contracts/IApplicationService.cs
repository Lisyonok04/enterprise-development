namespace Airline.Application.Contracts;

public interface IApplicationService<TDto, TCreateUpdateDto, TKey>
    where TDto : class
    where TCreateUpdateDto : class
    where TKey : struct
{
    public Task<TDto> CreateAsync(TCreateUpdateDto dto);
    public Task<TDto?> GetByIdAsync(TKey id);
    public Task<IList<TDto>> GetAllAsync();
    public Task<TDto> UpdateAsync(TCreateUpdateDto dto, TKey id);
    public Task<bool> DeleteAsync(TKey id);

}
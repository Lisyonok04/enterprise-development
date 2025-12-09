namespace Airline.Application.Contracts;

public interface IApplicationService<TDto, TCreateUpdateDto, TKey>
    where TDto : class
    where TCreateUpdateDto : class
    where TKey : struct
{
    public Task<TDto> Create(TCreateUpdateDto dto);
    public Task<TDto?> Get(TKey dtoId);
    public Task<IList<TDto>> GetAll();
    public Task<TDto> Update(TCreateUpdateDto dto, TKey dtoId);
    public Task<bool> Delete(TKey dtoId);
}
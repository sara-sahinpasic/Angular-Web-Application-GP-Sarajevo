namespace Application.Services.Abstractions.Interfaces.Mapper;

public interface IObjectMapperService
{
    public void Map<From, To>(From fromObject, To toObject);
}

using Application.Services.Abstractions.Interfaces.Mapper;

namespace Application.Services.Implementations.Mapper;

public sealed class ObjectMapperService : IObjectMapperService
{
    public void Map<TFrom, TTo>(TFrom fromObject, TTo toObject)
    {
        ArgumentNullException.ThrowIfNull(fromObject);
        ArgumentNullException.ThrowIfNull(toObject);

        var toType = typeof(TTo);
        var fromType = typeof(TFrom);

        foreach (var prop in toType.GetProperties())
        {
            var fromObjectProp = fromType.GetProperty(prop.Name);

            if (fromObjectProp == null || fromObjectProp.PropertyType != prop.PropertyType)
            {
                continue;
            }

            var fromObjectPropValue = fromObjectProp.GetValue(fromObject);
            toType.GetProperty(prop.Name)!.SetValue(toObject, fromObjectPropValue);
        }
    }
}

using Application.Services.Abstractions.Interfaces.Mapper;

namespace Application.Services.Implementations.Mapper;

public sealed class ObjectMapperService : IObjectMapperService
{
    public void Map<From, To>(From fromObject, To toObject)
    {
        if (fromObject == null || toObject == null)
            throw new System.Exception("Object cannot be null.");

        var toType = typeof(To);
        var fromType = typeof(From);

        foreach (var prop in toType.GetProperties())
        {
            var fromObjectProp = fromType.GetProperty(prop.Name);

            if (fromObjectProp == null)
                continue;

            var fromObjectPropValue = fromObjectProp.GetValue(fromObject);
            toType.GetProperty(prop.Name)!.SetValue(toObject, fromObjectPropValue);
        }
    }
}

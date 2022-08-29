namespace Neo.Infrastructure.Framework.Domain;

public class BusinessException : Exception
{
    public BusinessException(Enum errorEnumValue)
    {
    }

    public BusinessException(Enum errorEnumValue, params string[] parameters)
    {
    }
}
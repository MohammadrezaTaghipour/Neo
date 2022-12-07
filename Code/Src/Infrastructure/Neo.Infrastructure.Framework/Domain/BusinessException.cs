namespace Neo.Infrastructure.Framework.Domain;

public class BusinessException : Exception
{
    public Enum ErrorCode { get; private set; }
    public string[] Parameters { get; protected set; }

    public BusinessException(Enum errorEnumValue)
    {
        ErrorCode = errorEnumValue;
    }

    public BusinessException(Enum errorEnumValue, params string[] parameters)
    {
        ErrorCode = errorEnumValue;
        Parameters = parameters;
    }
}
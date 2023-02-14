namespace Neo.Infrastructure.Framework.Domain;

public class BusinessException : Exception
{
    public string ErrorCode { get; private set; }
    public string[] Parameters { get; protected set; }

    public BusinessException(Enum errorEnumValue)
    {
        ErrorCode = errorEnumValue.ToString();
    }

    public BusinessException(string errorEnumValue)
    {
        ErrorCode = errorEnumValue;
    }

    public BusinessException(Enum errorEnumValue, params string[] parameters)
    {
        ErrorCode = errorEnumValue.ToString();
        Parameters = parameters;
    }
}
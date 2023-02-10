using Neo.Specs.Features.Shared;

namespace Suzianna.Rest.Screenplay.Questions;

public static class LastResponseException
{
    private static ErrorResponse _response;

    public static void Set(string errorCode, string errorMessage)
    {
        _response = new ErrorResponse
        {
            Code = errorCode,
            Message = errorMessage
        };
    }

    public static ErrorResponse Content()
    {
        return _response;
    }

    public static bool HasException()
    {
        return _response != null;
    }
}
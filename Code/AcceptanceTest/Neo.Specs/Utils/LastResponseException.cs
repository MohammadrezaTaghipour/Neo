using Neo.Specs.Features.Shared;
using System.Collections.Concurrent;

namespace Suzianna.Rest.Screenplay.Questions;

public static class LastResponseException
{
    private static ConcurrentQueue<ErrorResponse> _responses = new();

    public static void Set(string errorCode, string errorMessage)
    {
        _responses.Enqueue(new ErrorResponse
        {
            Code = errorCode,
            Message = errorMessage
        });
    }

    public static ErrorResponse Content()
    {
        _responses.TryDequeue(out var response);
        return response;
    }

    public static bool HasException()
    {
        return _responses.Count > 0;
    }
}
using Neo.Specs.Features.Shared;
using Suzianna.Core.Screenplay.Actors;

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

    public static ErrorResponse Content(Actor actor)
    {
        return _response ?? actor.AsksFor(LastResponse.Content<ErrorResponse>());
    }
}
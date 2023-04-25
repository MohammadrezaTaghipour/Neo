using Neo.Specs.Features.Shared;

namespace Suzianna.Rest.Screenplay.Questions;

public class LastRequestResponse
{
    public string RequestId { get; set; }
    private ErrorResponse _response;

    public void Set(string errorCode, string errorMessage)
    {
        _response = new ErrorResponse
        {
            Code = errorCode,
            Message = errorMessage
        };
    }

    public ErrorResponse Content()
    {
        return _response;
    }

    public bool HasException()
    {
        return _response != null;
    }
}
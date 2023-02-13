using Microsoft.Extensions.Configuration;

namespace Neo.Infrastructure.Framework.AspCore;

public interface IErrorResponseBuilder
{
    ErrorResponse Buid(string errorCode, string errorMessage);
}

public class ErrorResponseBuilder : IErrorResponseBuilder
{
    private readonly IConfiguration _configuration;

    public ErrorResponseBuilder(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ErrorResponse Buid(string errorCode, string errorMessage)
    {
        var basicOption = _configuration.GetSection("basic").Get<BasicOption>();
        var errorResponse = new ErrorResponse($"{basicOption.ServiceName}_{errorCode}",
            errorMessage);
        return errorResponse;
    }
}

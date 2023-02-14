using Neo.Specs.Features.Shared;
using Newtonsoft.Json;
using Suzianna.Rest;
using Suzianna.Rest.Screenplay.Questions;

namespace Neo.Specs.Utils;

public class CustomHttpRequestSender : IHttpRequestSender
{
    private static readonly HttpClient Client;
    private readonly LastResponseException _lastResponseException;

    static CustomHttpRequestSender()
    {
        Client = new HttpClient();
    }

    public CustomHttpRequestSender(LastResponseException lastResponseException)
    {
        _lastResponseException = lastResponseException;
    }

    public HttpResponseMessage Send(HttpRequestMessage message)
    {
        var response = Client.SendAsync(message).GetAwaiter().GetResult();
        if (!response.IsSuccessStatusCode)
        {
            var responseString = response.Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
            _lastResponseException.Set(errorResponse.Code, errorResponse.Message);
        }
        return response;
    }
}
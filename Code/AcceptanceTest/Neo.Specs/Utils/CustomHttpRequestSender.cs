using Neo.Specs.Features.Shared;
using Newtonsoft.Json;
using Suzianna.Rest;
using Suzianna.Rest.Screenplay.Questions;

namespace Neo.Specs.Utils;

public class CustomHttpRequestSender : IHttpRequestSender
{
    private static readonly HttpClient Client;
    private readonly LastRequestResponse _lastRequestResponse;

    static CustomHttpRequestSender()
    {
        Client = new HttpClient();
    }

    public CustomHttpRequestSender(LastRequestResponse lastRequestResponse)
    {
        _lastRequestResponse = lastRequestResponse;
    }

    public HttpResponseMessage Send(HttpRequestMessage message)
    {
        var response = Client.SendAsync(message).GetAwaiter().GetResult();
        _lastRequestResponse.RequestId = response.Headers.GetValues("X-ReqId").First();
        if (!response.IsSuccessStatusCode)
        {
            var responseString = response.Content.ReadAsStringAsync()
                .GetAwaiter()
                .GetResult();
            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
            _lastRequestResponse.Set(errorResponse.Code, errorResponse.Message);
        }
        return response;
    }
}
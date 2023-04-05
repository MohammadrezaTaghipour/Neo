using Neo.Web.UI.Services.StreamEventTypes.Models;

namespace Neo.Web.UI.Services.StreamEventTypes;

public class StreamEventTypeService : IStreamEventTypeService
{
    private readonly HttpClient _httpClient;
    string resourceUrl = "http://localhost:5049/api/streamEventTypes";

    public StreamEventTypeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Guid> Define(StreamEventTypeDefinitionModel model)
    {
        var response = await _httpClient.PostAsJsonAsync(resourceUrl, model);

        response.EnsureSuccessStatusCode();

        var id = await response.Content.ReadFromJsonAsync<Guid>();
        return id;
    }

    public Task<IReadOnlyCollection<StreamEventTypeModel>> GetALL(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<StreamEventTypeModel> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> Modify(StreamEventTypeModificationModel model)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> Remove(StreamEventTypeRemovalModel model)
    {
        throw new NotImplementedException();
    }

    void IDisposable.Dispose() => _httpClient.Dispose();
}
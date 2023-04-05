using Neo.Web.UI.Services.StreamEventTypes.Models;

namespace Neo.Web.UI.Services.StreamEventTypes;

public interface IStreamEventTypeService : IDisposable
{
    Task<Guid> Define(StreamEventTypeDefinitionModel model);
    Task<Guid> Modify(StreamEventTypeModificationModel model);
    Task<Guid> Remove(StreamEventTypeRemovalModel model);
    Task<StreamEventTypeModel> GetById(Guid id);
    Task<IReadOnlyCollection<StreamEventTypeModel>> GetALL(Guid id);
}

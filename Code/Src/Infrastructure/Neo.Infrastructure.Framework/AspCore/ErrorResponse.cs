
using System.Text.Json.Serialization;

namespace Neo.Infrastructure.Framework.AspCore
{
    public class ErrorResponse
    {
        [JsonConstructor]
        public ErrorResponse(string code, string message)
        {
            Code = code?.Replace("_", "-");
            Message = message;
        }

        public string Message { get; private set; }
        public string Code { get; private set; }
        public object AdditionalData { get; set; }
    }
}

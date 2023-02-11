

using System.Text.Json.Serialization;

namespace Neo.Infrastructure.Framework.AspCore
{
    public class ErrorResponse
    {
        private ErrorResponse()
        {

        }

        [JsonConstructor]
        public ErrorResponse(string code, string message)
        {
            Code = code.Replace("_", "-");
            Message = message;
        }

        public ErrorResponse(string code, string message,
            object additionalData)
        {
            Code = code;
            Message = message;
            AdditionalData = additionalData;
        }

        public string Message { get; private set; }
        public string Code { get; private set; }
        public object AdditionalData { get; set; }
    }
}

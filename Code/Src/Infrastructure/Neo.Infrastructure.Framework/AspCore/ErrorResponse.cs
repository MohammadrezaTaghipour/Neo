

namespace Neo.Infrastructure.Framework.AspCore
{
    public class ErrorResponse
    {
        public ErrorResponse(string message, string code)
        {
            Message = message;
            Code = code.Replace("_", "-");
        }

        public ErrorResponse(string message, string code,
            object additionalData)
        {
            Message = message;
            Code = code;
            AdditionalData = additionalData;
        }

        public string Message { get; }
        public string Code { get; set; }
        public object AdditionalData { get; set; }
    }
}

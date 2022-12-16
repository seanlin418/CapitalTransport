using System.Net;
using System.Reflection.Metadata;
using Transport.Application.Contract.Constants;

namespace Transport.Api.Exceptions
{
    public class ExceptionBase : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string? ErrorMessage { get; }
        public string ModuleMessage { get; } = ApplicationConstants.GeneralExceptionMessage;

        public ExceptionBase() : base() { }

        public ExceptionBase(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string moduleMessage = ApplicationConstants.GeneralExceptionMessage) : base()
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
            ModuleMessage = moduleMessage;
        }

        public ExceptionBase(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, Exception? innerException = null, string moduleMessage = ApplicationConstants.GeneralExceptionMessage) 
            : base(innerException?.Message, innerException)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
            ModuleMessage = moduleMessage;
        }

        public override string Message => !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : ModuleMessage;
    }
}

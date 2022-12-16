using System.Net;
using Transport.Application.Contract.Constants;

namespace Transport.Api.Exceptions
{
    public class BadRequestException : ExceptionBase
    {
        public BadRequestException() : base() { }

        public BadRequestException(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string moduleMessage = ApplicationConstants.GeneralExceptionMessage) 
            : base(errorMessage, statusCode, moduleMessage)
        {
        }

        public BadRequestException(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, Exception? innerException = null, string moduleMessage = ApplicationConstants.GeneralExceptionMessage)
            : base(errorMessage, statusCode, innerException, moduleMessage)
        {
        }
    }
}

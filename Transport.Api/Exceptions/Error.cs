using Newtonsoft.Json;

namespace Transport.Api.Exceptions
{
    public class Error
    {
        public int StatusCode { get; }
        public string ErrorMessage { get; }

        public Error(int statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

using System.Net;

namespace Infrastructure.Helpers;

public class RestException : Exception
{
    public RestException(HttpStatusCode code, object errors)
    {
        Code = code;
        Errors = errors;
    }

    public object Errors { get; set; }
    public HttpStatusCode Code { get; }
}
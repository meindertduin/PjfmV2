using System.Net;

namespace Pjfm.Application.Common
{
    public class ServiceRequestResult<T>
    {
        public bool IsSuccessful { get; set; }
        public T Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public static ServiceRequestResult<T> Success(T result, HttpStatusCode statusCode)
        {
            return new ServiceRequestResult<T>()
            {
                Result = result,
                StatusCode = statusCode,
                IsSuccessful = true,
            };
        }

        public static ServiceRequestResult<T> Fail(T result, HttpStatusCode statusCode)
        {
            return new ServiceRequestResult<T>()
            {
                Result = result,
                StatusCode = statusCode,
                IsSuccessful = false,
            };
        }
    }
}
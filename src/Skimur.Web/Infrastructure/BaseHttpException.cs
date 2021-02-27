using System;
using System.Net;

namespace Skimur.Web.Infrastructure
{
    public class BaseHttpException : Exception
    {
        public BaseHttpException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }

    public class NotFoundException : BaseHttpException
    {
        public NotFoundException() :
            base(HttpStatusCode.NotFound, string.Empty)
        {

        }
    }
}

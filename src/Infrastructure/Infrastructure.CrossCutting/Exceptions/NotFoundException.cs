using System.Net;

namespace DeviceApi.Infrastructure.CrossCutting.Exceptions
{
    public class NotFoundException : ApiErrorException
    {
        public NotFoundException() : base("Not found", ErrorCodes.NotFound, HttpStatusCode.NotFound)
        {
        }
    }
}

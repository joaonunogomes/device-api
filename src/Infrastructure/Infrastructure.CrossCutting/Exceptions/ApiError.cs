using Newtonsoft.Json;

namespace DeviceApi.Infrastructure.CrossCutting.Exceptions
{
    public class ApiError
    {
        public int ErrorCode { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

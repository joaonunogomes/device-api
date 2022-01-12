using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DeviceApi.Infrastructure.CrossCutting.Exceptions
{
    public class ApiError
    {
        public int ErrorCode { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(
                this, 
                new JsonSerializerSettings 
                { 
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }
    }
}

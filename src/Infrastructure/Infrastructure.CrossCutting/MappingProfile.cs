using Newtonsoft.Json;

namespace DeviceApi.Infrastructure.CrossCutting
{

    public static class MappingProfile
    {
        public static TDestiny Map<TSource, TDestiny>(TSource obj)
            where TSource : class
            where TDestiny : class
        {
            return JsonConvert.DeserializeObject<TDestiny>(JsonConvert.SerializeObject(obj));
        }
    }
}

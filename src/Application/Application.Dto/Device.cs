using System;

namespace DeviceApi.Application.Dto
{
    public class Device
    {
        public Guid Id { get; set; }

        public Guid BrandId { get; set; }

        public string Name { get; set; }

        public DateTime CreationDate { get; set; }
    }
}

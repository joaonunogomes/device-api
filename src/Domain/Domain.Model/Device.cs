using System;

namespace DeviceApi.Domain.Model
{
    public class Device
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Brand Brand { get; set; }

        public DateTime CreationDate { get; set; }
    }
}

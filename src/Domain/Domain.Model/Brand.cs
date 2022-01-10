using System;

namespace DeviceApi.Domain.Model
{
    public class Brand
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
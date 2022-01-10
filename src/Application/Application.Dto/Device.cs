using System;
using System.ComponentModel;

namespace DeviceApi.Application.Dto
{
    public class Device
    {
        [ReadOnly(true)]
        public Guid Id { get; set; }

        public Guid BrandId { get; set; }

        public string Name { get; set; }

        public DateTime CreationDate { get; set; }
    }
}

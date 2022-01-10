using System;
using System.ComponentModel;

namespace DeviceApi.Application.Dto
{
    public class Brand
    {
        [ReadOnly(true)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [ReadOnly(true)]
        public DateTime CreationDate { get; set; }
    }
}
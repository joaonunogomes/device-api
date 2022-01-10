using System;

namespace DeviceApi.Application.Dto
{
    public class DeviceFilters
    {
        public DeviceFilters()
        {
            this.BrandId = Guid.Empty;
        }

        /// <summary>
        /// Filters only for devices that belongs to the provided brand.<br/>
        /// If not provied will ignore this filter
        /// </summary>
        public Guid BrandId { get; set; }
    }
}

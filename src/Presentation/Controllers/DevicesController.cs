using DeviceApi.Application.Dto;
using DeviceApi.Application.Services.Devices;
using DeviceApi.Infrastructure.CrossCutting.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DeviceApi.Presentation.Api.Controllers
{
    [ApiController]
    [Route("/devices")]
    public class DevicesController : ControllerBase
    {
        private readonly IDeviceService deviceService;
        private readonly ILogger<DevicesController> logger;

        public DevicesController(IDeviceService deviceService, ILogger<DevicesController> logger)
        {
            this.deviceService = deviceService;
            this.logger = logger;
        }

        /// <summary>
        /// Creates a new device
        /// </summary>
        /// <remarks>
        /// __________________________________________________________
        /// <example>
        /// Sample request:
        ///     {
        ///         "Name": "OnePlus 6T",
        ///         "brandId": "85501265-0ed0-41a8-8295-0bec9788a4ba",
        ///         "creationDate": "2022-01-10T13:44:51.643Z"
        ///     }
        /// </example>
        /// </remarks>
        /// <param name="device">The device to be created.</param>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Device))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] Device device)
        {
            var createdDevice = await this.deviceService.CreateAsync(device);

            this.logger.Info($"Creating new device", device);

            return this.CreatedAtAction(nameof(GetAsync), createdDevice);
        }

        /// <summary>
        /// Get a device
        /// </summary>
        /// <param name="id">Universal identifier of the device.</param>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Device))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            return this.Ok(await this.deviceService.GetAsync(id));
        }

        /// <summary>
        /// Get all devices
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Device))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAsync()
        {
            return this.Ok(await this.deviceService.GetAllAsync());
        }

        /// <summary>
        /// Update a device
        /// </summary>
        /// <remarks>
        /// __________________________________________________________
        /// <example>
        /// Sample request:
        ///     {
        ///         "Name": "OnePlus 6T",
        ///         "brandId": "85501265-0ed0-41a8-8295-0bec9788a4ba",
        ///         "creationDate": "2022-01-10T13:44:51.643Z"
        ///     }
        /// </example>
        /// </remarks>
        /// <param name="id">Universal identifier of the device.</param>
        /// <param name="device">The device to be updated.</param>
        [HttpPut]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(Device))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutAsync([FromRoute] Guid id, Device device)
        {
            await this.deviceService.UpdateAsync(id, device);

            return this.NoContent();
        }
    }
}

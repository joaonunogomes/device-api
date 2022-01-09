using DeviceApi.Application.Dto;
using DeviceApi.Application.Services;
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
        /// <param name="device">The device to be created.</param>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Device))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] Device device)
        {
            try
            {
                var createdRover = await this.deviceService.CreateAsync(device);

                this.logger.Info($"Creating new device", device);

                return this.CreatedAtAction(nameof(GetAsync), createdRover);
            }
            catch(Exception ex)
            {
                var errorMessage = "Couldn't create device";
                this.logger.Error(errorMessage, ex);

                return BadRequest(errorMessage);
            }
        }

        /// <summary>
        /// Get a rover
        /// </summary>
        /// <param name="id">Universal identifier of the rover.</param>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Device))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            try
            {
                return this.Ok(await this.deviceService.GetAsync(id));
            }
            catch (Exception ex)
            {
                var errorMessage = "Couldn't get device";
                this.logger.Error(errorMessage, new { DeviceId = id } , ex);

                return BadRequest(errorMessage);
            }
        }
    }
}

using DeviceApi.Application.Dto;
using DeviceApi.Application.Services.Brands;
using DeviceApi.Infrastructure.CrossCutting.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DeviceApi.Presentation.Api.Controllers
{
    [ApiController]
    [Route("/brands")]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService brandService;
        private readonly ILogger<BrandsController> logger;

        public BrandsController(IBrandService brandService, ILogger<BrandsController> logger)
        {
            this.brandService = brandService;
            this.logger = logger;
        }

        /// <summary>
        /// Creates a new brand
        /// </summary>
        /// <param name="brand">The brand to be created.</param>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Brand))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] Brand brand)
        {
            try
            {
                var createdBrand = await this.brandService.CreateAsync(brand);

                this.logger.Info($"Creating new brand", brand);

                return this.CreatedAtAction(nameof(GetAsync), createdBrand);
            }
            catch (Exception ex)
            {
                var errorMessage = "Couldn't create brand";
                this.logger.Error(errorMessage, ex);

                return BadRequest(errorMessage);
            }
        }

        /// <summary>
        /// Get a brand
        /// </summary>
        /// <param name="id">Universal identifier of the brand.</param>
        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Brand))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            try
            {
                return this.Ok(await this.brandService.GetAsync(id));
            }
            catch (Exception ex)
            {
                var errorMessage = "Couldn't get brand";
                this.logger.Error(errorMessage, new { BrandId = id }, ex);

                return BadRequest(errorMessage);
            }
        }
    }
}

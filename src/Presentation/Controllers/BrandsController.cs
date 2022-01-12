using DeviceApi.Application.Dto;
using DeviceApi.Application.Services.Brands;
using DeviceApi.Infrastructure.CrossCutting.Exceptions;
using DeviceApi.Infrastructure.CrossCutting.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
        public async Task<IActionResult> PostAsync([FromBody] Brand brand)
        {
            var createdBrand = await this.brandService.CreateAsync(brand);

            this.logger.Info($"Creating new brand", brand);

            return this.CreatedAtAction("GetAll", createdBrand);
        }

        /// <summary>
        /// Get all brands
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Brand>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiError))]
        public async Task<IActionResult> GetAllAsync()
        {
            return this.Ok(await this.brandService.GetAllAsync());
        }
    }
}

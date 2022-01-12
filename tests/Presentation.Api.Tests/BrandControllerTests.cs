using CtorMock.Moq;
using DeviceApi.Application.Dto;
using DeviceApi.Application.Services.Brands;
using DeviceApi.Presentation.Api.Controllers;
using DeviceApi.Presentation.Api.Tests.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DeviceApi.Presentation.Api.Tests
{
    public class BrandControllerTests : MockBase<BrandsController>
    {
        private readonly Mock<IBrandService> brandServiceMock;
        private readonly Mock<ILogger<BrandsController>> loggerMock;

        private readonly Guid BRAND_ID = Guid.NewGuid();

        public BrandControllerTests()
        {
            this.brandServiceMock = Mocker.MockOf<IBrandService>();
            this.loggerMock = Mocker.MockOf<ILogger<BrandsController>>();
        }

        [Fact]
        public async Task PostAsync_DefaultBehaviour_ShouldReturnCreatedBrand()
        {
            // Arrange
            var brandMock = new Brand
            {
                Id = this.BRAND_ID
            };

            this.brandServiceMock
                .Setup(x => x.CreateAsync(It.IsAny<Brand>()))
                .ReturnsAsync(brandMock);

            // Act
            var act = await this.Subject.PostAsync(new Brand());

            // Assert
            act.Should().BeOfType(typeof(CreatedAtActionResult));
            var result = act.GetValueFromCreatedAtActionResult<Brand>();
            result.Should().NotBeNull();
            result.Should().BeSameAs(brandMock);
            this.brandServiceMock.Verify(x => x.CreateAsync(It.IsAny<Brand>()), Times.Once);
        }

        [Fact]
        public async Task PostAsync_WhenBrandServiceThrowsException_ShouldThrowException()
        {
            // Arrange
            var brandMock = new Brand();

            this.brandServiceMock
                .Setup(x => x.CreateAsync(It.IsAny<Brand>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.PostAsync(brandMock);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.brandServiceMock.Verify(x => x.CreateAsync(brandMock), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_DefaultBehaviour_ShouldReturnDeviceList()
        {
            // Arrange
            var brandListMock = new List<Brand>
            {
                new Brand
                {
                    Id = this.BRAND_ID
                }
            };

            this.brandServiceMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(brandListMock);

            // Act
            var act = await this.Subject.GetAllAsync();

            // Assert
            act.Should().BeOfType(typeof(OkObjectResult));
            var result = act.GetValueFromOkObjectResult<List<Brand>>();
            result.Should().NotBeNullOrEmpty();
            result.Should().BeSameAs(brandListMock);
            this.brandServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenDeviceServiceThrowsException_ShouldThrowExcception()
        {
            // Arrange
            this.brandServiceMock
                .Setup(x => x.GetAllAsync())
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.GetAllAsync();

            // Assert
            await act.Should().ThrowAsync<Exception>();

            this.brandServiceMock.Verify(x => x.GetAllAsync(), Times.Once);
        }
    }
}

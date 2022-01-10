using CtorMock.Moq;
using DeviceApi.Application.Dto;
using DeviceApi.Application.Services.Brands;
using DeviceApi.Presentation.Api.Controllers;
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
            var brandMock = new Brand();

            this.brandServiceMock
                .Setup(x => x.CreateAsync(It.IsAny<Brand>()))
                .ReturnsAsync(new Brand());

            // Act
            var act = await this.Subject.PostAsync(brandMock);

            // Assert
            act.Should().BeOfType(typeof(CreatedAtActionResult));
            this.brandServiceMock.Verify(x => x.CreateAsync(brandMock), Times.Once);
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
            this.brandServiceMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Brand>());

            // Act
            var act = await this.Subject.GetAllAsync();

            // Assert
            act.Should().BeOfType(typeof(OkObjectResult));
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

using CtorMock.Moq;
using DeviceApi.Application.Dto;
using DeviceApi.Application.Services;
using DeviceApi.Presentation.Api.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace DeviceApi.Presentation.Api.Tests
{
    public class DeviceControllerTests : MockBase<DevicesController>
    {
        private readonly Mock<IDeviceService> deviceServiceMock;
        private readonly Mock<ILogger<DevicesController>> loggerMock;

        private readonly Guid DEVICE_ID = Guid.NewGuid();

        public DeviceControllerTests()
        {
            this.deviceServiceMock = Mocker.MockOf<IDeviceService>();
            this.loggerMock = Mocker.MockOf<ILogger<DevicesController>>();
        }

        [Fact]
        public async Task PostAsync_DefaultBehaviour_ShouldReturnCreatedDevice()
        {
            // Arrange
            var deviceMock = new Device();

            this.deviceServiceMock
                .Setup(x => x.CreateAsync(It.IsAny<Device>()))
                .ReturnsAsync(new Device());

            // Act
            var act = await this.Subject.PostAsync(deviceMock);

            // Assert
            act.Should().BeOfType(typeof(CreatedAtActionResult));
            this.deviceServiceMock.Verify(x => x.CreateAsync(deviceMock), Times.Once);
        }

        [Fact]
        public async Task PostAsync_WhenDeviceServiceThrowsException_ShouldThrowException()
        {
            // Arrange
            var deviceMock = new Device();

            this.deviceServiceMock
                .Setup(x => x.CreateAsync(It.IsAny<Device>()))
                .ThrowsAsync(new Exception());

            // Act
            var act = await this.Subject.PostAsync(deviceMock);

            // Assert
            act.Should().BeOfType(typeof(BadRequestObjectResult));
            this.deviceServiceMock.Verify(x => x.CreateAsync(deviceMock), Times.Once);
        }

        [Fact]
        public async Task GetAsync_DefaultBehaviour_ShouldReturnDevice()
        {
            // Arrange
            this.deviceServiceMock
                .Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new Device { Id = this.DEVICE_ID });

            // Act
            var act = await this.Subject.GetAsync(this.DEVICE_ID);

            // Assert
            act.Should().BeOfType(typeof(OkObjectResult));
            this.deviceServiceMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Once);
        }

        [Fact]
        public async Task GetAsync_WhenDeviceServiceThrowsException_ShouldReturnBadRequest()
        {
            // Arrange
            this.deviceServiceMock
                .Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception());

            // Act
            var act = await this.Subject.GetAsync(this.DEVICE_ID);

            // Assert
            act.Should().BeOfType(typeof(BadRequestObjectResult));
            this.deviceServiceMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Once);
        }
    }
}

using CtorMock.Moq;
using DeviceApi.Application.Dto;
using DeviceApi.Application.Services.Devices;
using DeviceApi.Presentation.Api.Controllers;
using DeviceApi.Presentation.Api.Tests.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
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
            var deviceMock = new Device
            {
                Id = this.DEVICE_ID
            };

            this.deviceServiceMock
                .Setup(x => x.CreateAsync(It.IsAny<Device>()))
                .ReturnsAsync(deviceMock);

            // Act
            var act = await this.Subject.PostAsync(new Device());

            // Assert
            act.Should().BeOfType(typeof(CreatedAtActionResult));
            var result = act.GetValueFromCreatedAtActionResult<Device>();
            result.Should().NotBeNull();
            result.Should().BeSameAs(deviceMock);
            this.deviceServiceMock.Verify(x => x.CreateAsync(It.IsAny<Device>()), Times.Once);
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
            Func<Task> act = async () => await this.Subject.PostAsync(deviceMock);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.deviceServiceMock.Verify(x => x.CreateAsync(deviceMock), Times.Once);
        }

        [Fact]
        public async Task GetAsync_DefaultBehaviour_ShouldReturnDevice()
        {
            // Arrange
            var deviceMock = new Device 
            { 
                Id = this.DEVICE_ID 
            };

            this.deviceServiceMock
                .Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(deviceMock);

            // Act
            var act = await this.Subject.GetAsync(this.DEVICE_ID);

            // Assert
            act.Should().BeOfType(typeof(OkObjectResult));
            var result = act.GetValueFromOkObjectResult<Device>();
            result.Should().NotBeNull();
            result.Should().BeSameAs(deviceMock);
            this.deviceServiceMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Once);
        }

        [Fact]
        public async Task GetAsync_WhenDeviceServiceThrowsException_ShouldThrowException()
        {
            // Arrange
            this.deviceServiceMock
                .Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.GetAsync(this.DEVICE_ID);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.deviceServiceMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_DefaultBehaviour_ShouldReturnDeviceList()
        {
            // Arrange
            var deviceFiltersMock = new DeviceFilters();
            var deviceListMock = new List<Device>
            {
                new Device
                {
                    Id = this.DEVICE_ID
                }
            };

            this.deviceServiceMock
                .Setup(x => x.GetAllAsync(It.IsAny<DeviceFilters>()))
                .ReturnsAsync(deviceListMock);

            // Act
            var act = await this.Subject.GetAllAsync(deviceFiltersMock);

            // Assert
            act.Should().BeOfType(typeof(OkObjectResult));
            var result = act.GetValueFromOkObjectResult<List<Device>>();
            result.Should().NotBeNullOrEmpty();
            result.Should().BeSameAs(deviceListMock);
            this.deviceServiceMock.Verify(x => x.GetAllAsync(deviceFiltersMock), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenDeviceServiceThrowsException_ShouldThrowExcception()
        {
            // Arrange
            var deviceFiltersMock = new DeviceFilters();

            this.deviceServiceMock
                .Setup(x => x.GetAllAsync(It.IsAny<DeviceFilters>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.GetAllAsync(deviceFiltersMock);

            // Assert
            await act.Should().ThrowAsync<Exception>();

            this.deviceServiceMock.Verify(x => x.GetAllAsync(deviceFiltersMock), Times.Once);
        }

        [Fact]
        public async Task PutAsync_DefaultBehaviour_ShouldUpdateDeviceAndReturnNoContent()
        {
            // Arrange
            this.deviceServiceMock
                .Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Device>()))
                .Returns(Task.CompletedTask);

            // Act
            var act = await this.Subject.PutAsync(this.DEVICE_ID, new Device());

            // Assert
            act.Should().BeOfType(typeof(NoContentResult));
            this.deviceServiceMock.Verify(x => x.UpdateAsync(this.DEVICE_ID, It.IsAny<Device>()), Times.Once);
        }

        [Fact]
        public async Task PutAsync_WhenDeviceServiceThrowsException_ShouldThrowException()
        {
            // Arrange
            this.deviceServiceMock
                .Setup(x => x.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Device>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.PutAsync(this.DEVICE_ID, new Device());

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.deviceServiceMock.Verify(x => x.UpdateAsync(this.DEVICE_ID, It.IsAny<Device>()), Times.Once);
        }

        [Fact]
        public async Task PatchAsync_DefaultBehaviour_ShouldUpdateDeviceAndReturnNoContent()
        {
            // Arrange
            this.deviceServiceMock
                .Setup(x => x.PatchAsync(It.IsAny<Guid>(), It.IsAny<JsonPatchDocument<Device>>()))
                .Returns(Task.CompletedTask);

            // Act
            var act = await this.Subject.PatchAsync(this.DEVICE_ID, new JsonPatchDocument<Device>());

            // Assert
            act.Should().BeOfType(typeof(NoContentResult));
            this.deviceServiceMock.Verify(x => x.PatchAsync(this.DEVICE_ID, It.IsAny<JsonPatchDocument<Device>>()), Times.Once);
        }

        [Fact]
        public async Task PatchAsync_WhenDeviceServiceThrowsException_ShouldThrowException()
        {
            // Arrange
            this.deviceServiceMock
                .Setup(x => x.PatchAsync(It.IsAny<Guid>(), It.IsAny<JsonPatchDocument<Device>>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.PatchAsync(this.DEVICE_ID, new JsonPatchDocument<Device>());

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.deviceServiceMock.Verify(x => x.PatchAsync(this.DEVICE_ID, It.IsAny<JsonPatchDocument<Device>>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DefaultBehaviour_ShouldDeleteDeviceAndReturnNoContent()
        {
            // Arrange
            this.deviceServiceMock
                .Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .Returns(Task.CompletedTask);

            // Act
            var act = await this.Subject.DeleteAsync(this.DEVICE_ID);

            // Assert
            act.Should().BeOfType(typeof(NoContentResult));
            this.deviceServiceMock.Verify(x => x.DeleteAsync(this.DEVICE_ID), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenDeviceServiceThrowsException_ShouldThrowException()
        {
            // Arrange
            this.deviceServiceMock
                .Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.DeleteAsync(this.DEVICE_ID);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.deviceServiceMock.Verify(x => x.DeleteAsync(this.DEVICE_ID), Times.Once);
        }
    }
}

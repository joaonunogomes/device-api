using CtorMock.Moq;
using DeviceApi.Application.Dto;
using DeviceApi.Application.Services.Devices;
using DeviceApi.Data.Repository.Devices;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using DomainModel = DeviceApi.Domain.Model;

namespace DeviceApi.Application.Services.Tests
{
    public class DeviceServiceTests : MockBase<DeviceService>
    {
        private readonly Mock<IDeviceRepository> deviceRepositoryMock;
        private readonly Guid DEVICE_ID = Guid.NewGuid();

        public DeviceServiceTests()
        {
            this.deviceRepositoryMock = Mocker.MockOf<IDeviceRepository>();
        }

        [Fact]
        public async Task CreateAsync_DefaultBehaviour_ShouldReturnCreatedDevice()
        {
            // Arrange
            var device = new Device();

            this.deviceRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<DomainModel.Device>()))
                .Returns(Task.CompletedTask);

            // Act
            var act = await this.Subject.CreateAsync(device);

            // Assert
            act.Should().NotBeNull();
            act.Should().BeOfType(typeof(Device));
            act.Id.Should().NotBeEmpty();
            this.deviceRepositoryMock.Verify(x => x.AddAsync(It.IsAny<DomainModel.Device>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenRepositoryThrowsException_ShouldThrowException()
        {
            // Arrange
            var device = new Device();

            this.deviceRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<DomainModel.Device>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.CreateAsync(device);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.deviceRepositoryMock.Verify(x => x.AddAsync(It.IsAny<DomainModel.Device>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_DefaultBehaviour_ShouldReturnCreatedDevice()
        {
            // Arrange
            var device = new DomainModel.Device();

            this.deviceRepositoryMock
                .Setup(x => x.GetAsync(this.DEVICE_ID))
                .ReturnsAsync(device);

            // Act
            var act = await this.Subject.GetAsync(this.DEVICE_ID);

            // Assert
            act.Should().NotBeNull();
            act.Should().BeOfType(typeof(Device));
            this.deviceRepositoryMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Once);
        }

        [Fact]
        public async Task GetAsync_WhenRepositoryThrowsException_ShouldThrowException()
        {
            // Arrange
            this.deviceRepositoryMock
                .Setup(x => x.GetAsync(this.DEVICE_ID))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.GetAsync(this.DEVICE_ID);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.deviceRepositoryMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_DefaultBehaviour_ShouldUpdateDevice()
        {
            // Arrange
            var device = new Device();

            this.deviceRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(), It.IsAny<DomainModel.Device>()))
                .Returns(Task.CompletedTask);

            // Act
            await this.Subject.UpdateAsync(this.DEVICE_ID, device);

            // Assert
            this.deviceRepositoryMock.Verify(x => x.UpdateAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(),
                It.Is<DomainModel.Device>(x => x.Id == this.DEVICE_ID)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenDeviceIsNull_ShouldThrowException()
        {
            // Arrange

            // Act
            Func<Task> act = async () => await this.Subject.UpdateAsync(this.DEVICE_ID, null);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.deviceRepositoryMock.Verify(x => x.UpdateAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(),
                It.IsAny<DomainModel.Device>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenRepositoryThrowsException_ShouldThrowException()
        {
            // Arrange
            var device = new Device();

            this.deviceRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(), It.IsAny<DomainModel.Device>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.UpdateAsync(this.DEVICE_ID, device);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.deviceRepositoryMock.Verify(x => x.UpdateAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(),
                It.Is<DomainModel.Device>(x => x.Id == this.DEVICE_ID)), Times.Once);
        }
    }
}

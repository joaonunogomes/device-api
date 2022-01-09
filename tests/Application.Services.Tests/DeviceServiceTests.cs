using CtorMock.Moq;
using DeviceApi.Application.Dto;
using DeviceApi.Application.Services.Devices;
using DeviceApi.Data.Repository.Devices;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;


namespace DeviceApi.Application.Services.Tests
{
    public class DeviceServiceTests : MockBase<DeviceService>
    {
        private readonly Mock<IDeviceRepository> deviceRepositoryMock;

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
                .Setup(x => x.AddAsync(It.IsAny<Device>()))
                .Returns(Task.CompletedTask);

            // Act
            var act = await this.Subject.CreateAsync(device);

            // Assert
            act.Should().NotBeNull();
            act.Should().BeOfType(typeof(Device));
            act.Id.Should().NotBeEmpty();
            this.deviceRepositoryMock.Verify(x => x.AddAsync(device), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenRepositoryThrowsException_ShouldThrowException()
        {
            // Arrange
            var device = new Device();

            this.deviceRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Device>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.CreateAsync(device);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.deviceRepositoryMock.Verify(x => x.AddAsync(device), Times.Once);
        }
    }
}

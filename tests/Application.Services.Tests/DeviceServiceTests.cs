using CtorMock.Moq;
using DeviceApi.Application.Dto;
using DeviceApi.Application.Services.Devices;
using DeviceApi.Data.Repository.Devices;
using DeviceApi.Infrastructure.CrossCutting.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using System;
using System.Collections.Generic;
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
            this.deviceRepositoryMock.Verify(x => x.AddAsync(It.Is<DomainModel.Device>(x => x.Id != Guid.Empty && x.CreationDate != default(DateTime))), Times.Once);
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
        public async Task GetAsync_DefaultBehaviour_ShouldReturnDevice()
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
        public async Task GetAllAsync_DefaultBehaviour_ShouldReturnDeviceList()
        {
            // Arrange
            this.deviceRepositoryMock
                .Setup(x => x.GetManyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>()))
                .ReturnsAsync(new List<DomainModel.Device> { new DomainModel.Device() });

            // Act
            var act = await this.Subject.GetAllAsync();

            // Assert
            act.Should().NotBeNull();
            act.Should().NotBeEmpty();
            act.Should().BeOfType(typeof(List<Device>));
            this.deviceRepositoryMock.Verify(x => x.GetManyAsync(It.Is<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(x => true)), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenRepositoryReturnsNull_ShouldReturnEmptyList()
        {
            // Arrange
            this.deviceRepositoryMock
                .Setup(x => x.GetManyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>()))
                .ReturnsAsync(null as List<DomainModel.Device>);

            // Act
            var act = await this.Subject.GetAllAsync();

            // Assert
            act.Should().NotBeNull();
            act.Should().BeEmpty();
            act.Should().BeOfType(typeof(List<Device>));
            this.deviceRepositoryMock.Verify(x => x.GetManyAsync(It.Is<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(x => true)), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenRepositoryThrowsException_ShouldThrowException()
        {
            // Arrange
            this.deviceRepositoryMock
                .Setup(x => x.GetManyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.GetAllAsync();

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.deviceRepositoryMock.Verify(x => x.GetManyAsync(It.Is<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(x => true)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_DefaultBehaviour_ShouldUpdateDevice()
        {
            // Arrange
            var deviceToUpdate = new Device 
            { 
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow.AddDays(10),
                Name = "IPhone 20",
                BrandId = Guid.NewGuid()
            };

            var deviceMock = new DomainModel.Device
            {
                Id = this.DEVICE_ID,
                BrandId = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                Name = "IPhon e 2 0"
            };

            this.deviceRepositoryMock
                   .Setup(x => x.GetAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(deviceMock);

            this.deviceRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(), It.IsAny<DomainModel.Device>()))
                .Returns(Task.CompletedTask);

            // Act
            await this.Subject.UpdateAsync(this.DEVICE_ID, deviceToUpdate);

            // Assert
            this.deviceRepositoryMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Once);
            this.deviceRepositoryMock.Verify(x => x.UpdateAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(),
                It.Is<DomainModel.Device>(x => x.Id == this.DEVICE_ID)), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenDeviceToUpdateIsNull_ShouldThrowException()
        {
            // Act
            Func<Task> act = async () => await this.Subject.UpdateAsync(this.DEVICE_ID, null);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.deviceRepositoryMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Never);
            this.deviceRepositoryMock.Verify(x => x.UpdateAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(),
                It.IsAny<DomainModel.Device>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenDeviceDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var deviceToUpdate = new Device
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow.AddDays(10),
                Name = "IPhone 20",
                BrandId = Guid.NewGuid()
            };

            this.deviceRepositoryMock
                   .Setup(x => x.GetAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(null as DomainModel.Device);

            this.deviceRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(), It.IsAny<DomainModel.Device>()))
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> act = async () => await this.Subject.UpdateAsync(this.DEVICE_ID, deviceToUpdate);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
            this.deviceRepositoryMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Once);
            this.deviceRepositoryMock.Verify(x => x.UpdateAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(),
                It.IsAny<DomainModel.Device>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenRepositoryThrowsException_ShouldThrowException()
        {
            // Arrange
            var device = new Device();

            var deviceMock = new DomainModel.Device
            {
                Id = this.DEVICE_ID,
                BrandId = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                Name = "IPhon e 2 0"
            };

            this.deviceRepositoryMock
                   .Setup(x => x.GetAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(deviceMock);

            this.deviceRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(), It.IsAny<DomainModel.Device>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.UpdateAsync(this.DEVICE_ID, device);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.deviceRepositoryMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Once);
            this.deviceRepositoryMock.Verify(x => x.UpdateAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(),
                It.Is<DomainModel.Device>(x => x.Id == this.DEVICE_ID)), Times.Once);
        }

        [Fact]
        public async Task PatchAsync_DefaultBehaviour_ShouldUpdateDevicePropreties()
        {
            // Arrange
            var newName = "IPhone 20";
            var jsonPatchDocument = new JsonPatchDocument<Device>();
            jsonPatchDocument.Operations.Add(new Microsoft.AspNetCore.JsonPatch.Operations.Operation<Device>
            {
                op = "replace",
                path = "name",
                value = newName
            });

            var deviceMock = new DomainModel.Device
            {
                Id = this.DEVICE_ID,
                BrandId = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                Name = "IPhon e 2 0"
            };

            this.deviceRepositoryMock
                   .Setup(x => x.GetAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(deviceMock);

            this.deviceRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(), It.IsAny<DomainModel.Device>()))
                .Returns(Task.CompletedTask);

            // Act
            await this.Subject.PatchAsync(this.DEVICE_ID, jsonPatchDocument);

            // Assert
            this.deviceRepositoryMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Once);
            this.deviceRepositoryMock.Verify(x => x.UpdateAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(),
                It.Is<DomainModel.Device>(x => x.Id == this.DEVICE_ID && x.Name == newName)), Times.Once);
        }

        [Fact]
        public async Task PatchAsync_WhenJsonPatchDocumentIsNull_ShouldThrowApiErrorException()
        {
            // Arrange
            var jsonPatchDocument = null as JsonPatchDocument<Device>;
            
            // Act
            Func<Task> act = async () => await this.Subject.PatchAsync(this.DEVICE_ID, jsonPatchDocument);

            // Assert
            await act.Should().ThrowAsync<ApiErrorException>();
            this.deviceRepositoryMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Never);
            this.deviceRepositoryMock.Verify(x => x.UpdateAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(), It.IsAny<DomainModel.Device>()), Times.Never);
        }

        [Fact]
        public async Task PatchAsync_WhenDeviceDoesNotExists_ShouldThrowNotFoundException()
        {
            // Arrange
            var jsonPatchDocument = new JsonPatchDocument<Device>();

            this.deviceRepositoryMock
                   .Setup(x => x.GetAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(null as DomainModel.Device);

            // Act
            Func<Task> act = async () => await this.Subject.PatchAsync(this.DEVICE_ID, jsonPatchDocument);

            // Assert
            await act.Should().ThrowAsync<ApiErrorException>();
            this.deviceRepositoryMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Once);
            this.deviceRepositoryMock.Verify(x => x.UpdateAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(), It.IsAny<DomainModel.Device>()), Times.Never);
        }

        [Fact]
        public async Task PatchAsync_WhenRepositoryThrowsException_ShouldThrowException()
        {
            // Arrange
            var newName = "IPhone 20";
            var jsonPatchDocument = new JsonPatchDocument<Device>();
            jsonPatchDocument.Operations.Add(new Microsoft.AspNetCore.JsonPatch.Operations.Operation<Device>
            {
                op = "replace",
                path = "name",
                value = newName
            });

            var deviceMock = new DomainModel.Device
            {
                Id = this.DEVICE_ID,
                BrandId = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                Name = "IPhon e 2 0"
            };

            this.deviceRepositoryMock
                   .Setup(x => x.GetAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(deviceMock);

            this.deviceRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(), It.IsAny<DomainModel.Device>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.PatchAsync(this.DEVICE_ID, jsonPatchDocument);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.deviceRepositoryMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Once);
            this.deviceRepositoryMock.Verify(x => x.UpdateAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Device, bool>>>(),
                It.Is<DomainModel.Device>(x => x.Id == this.DEVICE_ID && x.Name == newName)), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DefaultBehaviour_ShouldDeleteDevice()
        {
            // Arrange
            this.deviceRepositoryMock
                   .Setup(x => x.GetAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(new DomainModel.Device());

            this.deviceRepositoryMock
               .Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
               .Returns(Task.CompletedTask);

            // Act
            await this.Subject.DeleteAsync(this.DEVICE_ID);

            // Assert
            this.deviceRepositoryMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Once);
            this.deviceRepositoryMock.Verify(x => x.DeleteAsync(this.DEVICE_ID), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenDeviceDoesNotexist_ShouldThrowNotFoundException()
        {
            // Arrange
            this.deviceRepositoryMock
                   .Setup(x => x.GetAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(null as DomainModel.Device);

            // Act
            Func<Task> act = async () => await this.Subject.DeleteAsync(this.DEVICE_ID);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
            this.deviceRepositoryMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Once);
            this.deviceRepositoryMock.Verify(x => x.DeleteAsync(this.DEVICE_ID), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_WhenRepositoryThrowException_ShouldThrowException()
        {
            // Arrange
            this.deviceRepositoryMock
                   .Setup(x => x.GetAsync(It.IsAny<Guid>()))
                   .ReturnsAsync(new DomainModel.Device());

            this.deviceRepositoryMock
               .Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
               .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.DeleteAsync(this.DEVICE_ID);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.deviceRepositoryMock.Verify(x => x.GetAsync(this.DEVICE_ID), Times.Once);
            this.deviceRepositoryMock.Verify(x => x.DeleteAsync(this.DEVICE_ID), Times.Once);
        }
    }
}

using CtorMock.Moq;
using DeviceApi.Application.Dto;
using DeviceApi.Application.Services.Brands;
using DeviceApi.Data.Repository.Brands;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using DomainModel = DeviceApi.Domain.Model;

namespace DeviceApi.Application.Services.Tests
{
    public class BrandServiceTests : MockBase<BrandService>
    {
        private readonly Mock<IBrandRepository> brandRepositoryMock;
        private readonly Guid BRAND_ID = Guid.NewGuid();


        public BrandServiceTests()
        {
            this.brandRepositoryMock = Mocker.MockOf<IBrandRepository>();
        }

        [Fact]
        public async Task CreateAsync_DefaultBehaviour_ShouldReturnCreatedBrand()
        {
            // Arrange
            var brand = new Brand();

            this.brandRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<DomainModel.Brand>()))
                .Returns(Task.CompletedTask);

            // Act
            var act = await this.Subject.CreateAsync(brand);

            // Assert
            act.Should().NotBeNull();
            act.Should().BeOfType(typeof(Brand));
            act.Id.Should().NotBeEmpty();
            this.brandRepositoryMock.Verify(x => x.AddAsync(It.IsAny<DomainModel.Brand>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenRepositoryThrowsException_ShouldThrowException()
        {
            // Arrange
            var brand = new Brand();

            this.brandRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<DomainModel.Brand>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.CreateAsync(brand);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.brandRepositoryMock.Verify(x => x.AddAsync(It.IsAny<DomainModel.Brand>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_DefaultBehaviour_ShouldReturnCreatedBrand()
        {
            // Arrange
            var brand = new DomainModel.Brand();

            this.brandRepositoryMock
                .Setup(x => x.GetAsync(this.BRAND_ID))
                .ReturnsAsync(brand);

            // Act
            var act = await this.Subject.GetAsync(this.BRAND_ID);

            // Assert
            act.Should().NotBeNull();
            act.Should().BeOfType(typeof(Brand));
            this.brandRepositoryMock.Verify(x => x.GetAsync(this.BRAND_ID), Times.Once);
        }

        [Fact]
        public async Task GetAsync_WhenRepositoryThrowsException_ShouldThrowException()
        {
            // Arrange
            this.brandRepositoryMock
                .Setup(x => x.GetAsync(this.BRAND_ID))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.GetAsync(this.BRAND_ID);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.brandRepositoryMock.Verify(x => x.GetAsync(this.BRAND_ID), Times.Once);
        }
    }
}

using CtorMock.Moq;
using DeviceApi.Application.Dto;
using DeviceApi.Application.Services.Brands;
using DeviceApi.Data.Repository.Brands;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
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
            this.brandRepositoryMock.Verify(x => x.AddAsync(It.Is<DomainModel.Brand>(x => x.Id != Guid.Empty && x.CreationDate != default(DateTime))), Times.Once);

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
        public async Task GetAllAsync_DefaultBehaviour_ShouldReturnBrandList()
        {
            // Arrange
            this.brandRepositoryMock
                .Setup(x => x.GetManyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Brand, bool>>>()))
                .ReturnsAsync(new List<DomainModel.Brand> { new DomainModel.Brand() });

            // Act
            var act = await this.Subject.GetAllAsync();

            // Assert
            act.Should().NotBeNull();
            act.Should().NotBeEmpty();
            act.Should().BeOfType(typeof(List<Brand>));
            this.brandRepositoryMock.Verify(x => x.GetManyAsync(It.Is<System.Linq.Expressions.Expression<Func<DomainModel.Brand, bool>>>(x => true)), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenRepositoryReturnsNull_ShouldReturnEmptyList()
        {
            // Arrange
            this.brandRepositoryMock
                .Setup(x => x.GetManyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Brand, bool>>>()))
                .ReturnsAsync(null as List<DomainModel.Brand>);

            // Act
            var act = await this.Subject.GetAllAsync();

            // Assert
            act.Should().NotBeNull();
            act.Should().BeEmpty();
            act.Should().BeOfType(typeof(List<Brand>));
            this.brandRepositoryMock.Verify(x => x.GetManyAsync(It.Is<System.Linq.Expressions.Expression<Func<DomainModel.Brand, bool>>>(x => true)), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenRepositoryThrowsException_ShouldThrowException()
        {
            // Arrange
            this.brandRepositoryMock
                .Setup(x => x.GetManyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<DomainModel.Brand, bool>>>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.GetAllAsync();

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.brandRepositoryMock.Verify(x => x.GetManyAsync(It.Is<System.Linq.Expressions.Expression<Func<DomainModel.Brand, bool>>>(x => true)), Times.Once);
        }
    }
}

using CtorMock.Moq;
using DeviceApi.Application.Dto;
using DeviceApi.Application.Services.Brands;
using DeviceApi.Data.Repository.Brands;
using FluentAssertions;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;


namespace DeviceApi.Application.Services.Tests
{
    public class BrandServiceTests : MockBase<BrandService>
    {
        private readonly Mock<IBrandRepository> brandRepositoryMock;

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
                .Setup(x => x.AddAsync(It.IsAny<Brand>()))
                .Returns(Task.CompletedTask);

            // Act
            var act = await this.Subject.CreateAsync(brand);

            // Assert
            act.Should().NotBeNull();
            act.Should().BeOfType(typeof(Brand));
            act.Id.Should().NotBeEmpty();
            this.brandRepositoryMock.Verify(x => x.AddAsync(brand), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenRepositoryThrowsException_ShouldThrowException()
        {
            // Arrange
            var brand = new Brand();

            this.brandRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Brand>()))
                .ThrowsAsync(new Exception());

            // Act
            Func<Task> act = async () => await this.Subject.CreateAsync(brand);

            // Assert
            await act.Should().ThrowAsync<Exception>();
            this.brandRepositoryMock.Verify(x => x.AddAsync(brand), Times.Once);
        }
    }
}

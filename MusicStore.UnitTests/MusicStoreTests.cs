using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using MusicStore.DataAccess;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories;
using MusicStore.Services.Implementations;

namespace MusicStore.UnitTests
{
    public class MusicStoreTests
    {
        [Fact]
        public void SumaTest()
        {
            // Arrange
            int a = 6;
            int b = 7;

            // Act

            var suma = a + b;
            var expected = 13;

            // Assert
            Assert.Equal(expected, suma);
        }

        private static async Task<MusicStoreDbContext> ArrangeDatabase()
        {
            var options = new DbContextOptionsBuilder<MusicStoreDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new MusicStoreDbContext(options);

            await dbContext.Database.EnsureCreatedAsync();
            return dbContext;
        }

        [Fact]
        public async Task ListGenreTest()
        {
            // Arrange
            var dbContext = await ArrangeDatabase();

            // Act
            var count = await dbContext.Set<Genre>().CountAsync();
            var expected = 8;

            // Assert
            Assert.Equal(expected, count);
        }

        [Fact]
        public async Task CheckIfGenreIsDeletedAfterMigrationTest()
        {
            // Arrange
            var dbContext = await ArrangeDatabase();

            // Act
            var genre = await dbContext.Set<Genre>().FirstOrDefaultAsync(g => g.Id == 1);
            if (genre != null)
            {
                dbContext.Set<Genre>().Remove(genre);
                await dbContext.SaveChangesAsync();
            }

            var count = await dbContext.Set<Genre>().CountAsync();
            var expected = 7;

            // Assert
            Assert.Equal(expected, count);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(5, true)]
        [InlineData(10, false)]
        public async Task GenreGetByIdTest(int id, bool expected)
        {
            // Arrange
            var dbContext = await ArrangeDatabase();

            var repository = new GenreRepository(dbContext);

            var mapperMock = new Mock<IMapper>();

            mapperMock.Setup(x => x.Map<GenreDtoResponse>(It.IsAny<Genre>()))
                .Returns(new GenreDtoResponse());

            var service = new GenreService(repository, 
                new Mock<ILogger<GenreService>>().Object,
                mapperMock.Object);

            // Act
            var actual = await service.GetAsync(id);

            // Assert
            Assert.Equal(expected, actual.Success);
        }

        [Theory]
        [InlineData(true, 0, "El concierto ya fue finalizado")]
        [InlineData(false, -10, "El concierto ya se realizo")]
        [InlineData(false, 1, null)]
        public async Task CheckIfSaleEvaluateConditionsTest(bool finalized, int daysPast, string messageExpected)
        {
            // Arrange
            var repository = new Mock<ISaleRepository>().Object;

            var logger = new Mock<ILogger<SaleService>>().Object;

            var mapperMock = new Mock<IMapper>().Object;

            var concertRepository = new Mock<IConcertRepository>();


            concertRepository.Setup(x => x.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(new Concert()
                {
                    Finalized = finalized,
                    DateEvent = DateTime.Now.AddDays(daysPast)
                });
            
            var customerRepository = new Mock<ICustomerRepository>();
            customerRepository.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new Customer());

            var service = new SaleService(repository, concertRepository.Object, logger, mapperMock, customerRepository.Object);

            // Act
            var result = await service.CreateSaleAsync(It.IsAny<string>(), new SaleDtoRequest
            {
                ConcertId = It.IsAny<int>(),
                TicketsQuantity = It.IsAny<int>()
            });

            // Arrange

            Assert.Equal(messageExpected, result.ErrorMessage);
        }

    }
}
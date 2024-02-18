using AutoMapper;
using DAL.Entities;
using Moq;
using ScientificExperiment.Tests.Database;
using ScientificExperiment.WebAPI.Models;
using ScientificExperiment.WebAPI.Services;

namespace ScientificExperiment.Tests.Services
{
    public class DataServiceGetTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public DataServiceGetTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public async Task GetResults_Returns_Correct_Results()
        {
            // Arrange
            var filter = new FilterModel
            {
                AverageIndicator_From = 0,
                AverageIndicator_To = 100,
                AverageTimeWork_From = 0,
                AverageTimeWork_To = 100
            };

            var files = new List<DAL.Entities.File>
             {
                new DAL.Entities.File { Id = 1, FileName = "file1.csv" },
                new DAL.Entities.File { Id = 2, FileName = "file2.csv" }
             };
            _fixture.Context.Files.AddRange(files);
            _fixture.Context.SaveChanges();

            var resultsData = new List<Result>
            {
            new Result { Id = 1, FileId = 1, AverageIndicator = 50, AverageTimeWork = 20, MinTimeWork = 10 },
            new Result { Id = 2, FileId = 2, AverageIndicator = 60, AverageTimeWork = 25, MinTimeWork = 12 }
            };
            _fixture.Context.Results.AddRange(resultsData);
            _fixture.Context.SaveChanges(); // Сохраняем изменения в базе данных


            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Result, ResultModel>();
            });
            IMapper mapper = mapperConfiguration.CreateMapper();


            var fileService = new Mock<FileService>(_fixture.Context);

            var dataServiceGet = new GetDataFromDbService(_fixture.Context, mapper, fileService.Object);

            // Act
            var result = await dataServiceGet.GetResults(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); 
        }

        [Fact]
        public async Task GetValues_Returns_Correct_Values()
        {
            // Arrange
            var files = new List<DAL.Entities.File>
             {
                new DAL.Entities.File { Id = 1, FileName = "file1.csv" },
                new DAL.Entities.File { Id = 2, FileName = "file2.csv" }
             };
            _fixture.Context.Files.AddRange(files);
            _fixture.Context.SaveChanges();

            var valuesData = new List<Value>
            {
            new Value { Id = 1, FileId = 1,  StartDateTime = DateTime.Now, WorkTime = 1500, Indicator = 167.68 },
            new Value { Id = 2, FileId = 2, StartDateTime = DateTime.Now.AddMinutes(-5), WorkTime = 1600, Indicator = 170.68 }
            };

            _fixture.Context.Values.AddRange(valuesData);
            _fixture.Context.SaveChanges();

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Value, ValueModel>();
            });
            IMapper mapper = mapperConfiguration.CreateMapper();


            var fileService = new Mock<FileService>(_fixture.Context);

            var service = new GetDataFromDbService(_fixture.Context, mapper, fileService.Object);

            // Act
            var result = await service.GetValues("file1.csv");
            var result2 = await service.GetValues("file2.csv");
            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result2);
            Assert.Single(result);
            Assert.Single(result2);
            Assert.Equal("file1.csv", result.First().FileName);
            Assert.Equal("file2.csv", result2.First().FileName);
        }
    }
}

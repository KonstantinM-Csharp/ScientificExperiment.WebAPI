using ScientificExperiment.WebAPI.Models;

namespace ScientificExperiment.Tests.Validation.Models
{
    public class FilterModelTests
    {
        [Fact]
        public void FilterModel_ShouldPassValidation_WithValidData()
        {
            // Arrange
            var model = new FilterModel
            {
                FileName = "example.csv",
                AverageIndicator_From = 0,
                AverageIndicator_To = 10,
                AverageTimeWork_From = 5,
                AverageTimeWork_To = 20
            };

            // Act
            var results = ValidationModel.ValidateModel(model);

            // Assert
            Assert.Empty(results); // Не должно быть ошибок валидации
        }

        [Theory]
        [InlineData(-1, 5, 5, 20, "AverageIndicator_From")]
        [InlineData(0, -1, 5, 20, "AverageIndicator_To")]
        [InlineData(0, 5, -1, 20, "AverageTimeWork_From")]
        [InlineData(0, 5, 5, -1, "AverageTimeWork_To")]
        public void FilterModel_ShouldFailValidation_WithInvalidData(double indicatorFrom, double indicatorTo, double timeWorkFrom, double timeWorkTo, string expectedFieldWithError)
        {
            // Arrange
            var model = new FilterModel
            {
                FileName = "example.csv",
                AverageIndicator_From = indicatorFrom,
                AverageIndicator_To = indicatorTo,
                AverageTimeWork_From = timeWorkFrom,
                AverageTimeWork_To = timeWorkTo
            };

            // Act
            var results = ValidationModel.ValidateModel(model);

            // Assert
            Assert.NotEmpty(results); // Должны быть ошибки валидации
            Assert.Contains(results, r => r.MemberNames.Contains(expectedFieldWithError)); // Ошибка должна быть связана с ожидаемым полем
        }
    }
}

using ScientificExperiment.WebAPI.Models;



namespace ScientificExperiment.Tests.Validation.Models
{
    public class ValueModelTest
    {
        [Fact]
        public void StartDateTimeValidation_ShouldFailForFutureDate()
        {
            // Arrange
            var model = new ValueModel();

            // Act
            model.StartDateTime = new DateTime(1999, 1, 1); // Устанавливаем дату до 2000 года
            var result1 = ValidationModel.ValidateModel(model);

            model.StartDateTime = DateTime.Now.AddSeconds(1); // Устанавливаем дату после текущей
            var result2 = ValidationModel.ValidateModel(model);

            model.StartDateTime = DateTime.Now.AddDays(-1); // Устанавливаем текущую дату
            var result3 = ValidationModel.ValidateModel(model);

            model.StartDateTime = new DateTime(2000, 1, 1); // Устанавливаем дату 2000 года
            var result4 = ValidationModel.ValidateModel(model);

            // Assert
            Assert.NotEmpty(result1); // Дата до 2000 года должна быть недопустимой
            Assert.NotEmpty(result2); // Дата после текущей даты должна быть недопустимой
            Assert.Empty(result3);    // Текущая дата должна быть допустимой
            Assert.Empty(result4);    // Дата 2000 года должна быть допустимой
        }
        [Fact]
        public void WorkTimeValidation_ShouldFailForNegativeTime()
        {
            // Arrange
            var model = new ValueModel
            {
                FileName = "test.csv",
                StartDateTime = DateTime.Now,
                WorkTime = -5, // Set a negative WorkTime to trigger validation error
                Indicator = 5.0
            };

            // Act
            var results = ValidationModel.ValidateModel(model);

            // Assert
            Assert.Equal("Время проведения эксперимента не может быть меньше 0", results[0].ErrorMessage);
        }

        [Fact]
        public void IndicatorValidation_ShouldFailForNegativeIndicator()
        {
            // Arrange
            var model = new ValueModel
            {
                FileName = "test.csv",
                StartDateTime = DateTime.Now,
                WorkTime = 10,
                Indicator = -5.0 // Set a negative Indicator to trigger validation error
            };

            // Act
            var results = ValidationModel.ValidateModel(model);

            // Assert
            Assert.Equal("Значение показателя не может быть меньше 0", results[0].ErrorMessage);
        }
    }

}
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
            model.StartDateTime = new DateTime(1999, 1, 1); // ������������� ���� �� 2000 ����
            var result1 = ValidationModel.ValidateModel(model);

            model.StartDateTime = DateTime.Now.AddSeconds(1); // ������������� ���� ����� �������
            var result2 = ValidationModel.ValidateModel(model);

            model.StartDateTime = DateTime.Now.AddDays(-1); // ������������� ������� ����
            var result3 = ValidationModel.ValidateModel(model);

            model.StartDateTime = new DateTime(2000, 1, 1); // ������������� ���� 2000 ����
            var result4 = ValidationModel.ValidateModel(model);

            // Assert
            Assert.NotEmpty(result1); // ���� �� 2000 ���� ������ ���� ������������
            Assert.NotEmpty(result2); // ���� ����� ������� ���� ������ ���� ������������
            Assert.Empty(result3);    // ������� ���� ������ ���� ����������
            Assert.Empty(result4);    // ���� 2000 ���� ������ ���� ����������
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
            Assert.Equal("����� ���������� ������������ �� ����� ���� ������ 0", results[0].ErrorMessage);
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
            Assert.Equal("�������� ���������� �� ����� ���� ������ 0", results[0].ErrorMessage);
        }
    }

}
using System.ComponentModel.DataAnnotations;

namespace ScientificExperiment.WebAPI.Models
{
    /// <summary>
    /// Содержит в себе поля для фильтрации Results
    /// </summary>
    public class FilterModel
    {
        public string? FileName { get; set; } = null;
        [Range(-0.0000001, Double.MaxValue, ErrorMessage = "Время работы должно быть положительным")]
        public double AverageIndicator_From { get; set; } = -0.0000001;
        [Range(-0.0000001, Double.MaxValue, ErrorMessage = "Время работы должно быть положительным")]
        public double AverageIndicator_To { get; set; } = -0.0000001;
        [Range(-0.0000001, Double.MaxValue, ErrorMessage = "Показатель должен быть положительным")]
        public double AverageTimeWork_From { get; set; } = -0.0000001;
        [Range(-0.0000001, Double.MaxValue, ErrorMessage = "Показатель должен быть положительным")]
        public double AverageTimeWork_To { get; set; } = -0.0000001;
    }
}

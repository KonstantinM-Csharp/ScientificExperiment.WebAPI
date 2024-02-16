
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApi_CSV.Services.Validation;

namespace WebApi_CSV.Models
{
    public class ValueModel
    {
        public string FileName { get; set; }
        [StartDateTime(ErrorMessage = "Дата проведения эксперимента должна быть между 01.01.2000 и текущей датой")]
        public DateTime StartDateTime { get; set; }
        [Range(-0.0000001, int.MaxValue, ErrorMessage = "Время проведения эксперимента не может быть меньше 0")]
        public int WorkTime { get; set; }
        [Range(-0.0000001, double.MaxValue, ErrorMessage = "Значение показателя не может быть меньше 0")]
        public double Indicator { get; set; }
        [JsonIgnore]
        public List<string>? Errors { get; set; }
    }
}

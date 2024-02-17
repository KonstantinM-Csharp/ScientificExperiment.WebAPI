using System.ComponentModel.DataAnnotations;

namespace ScientificExperiment.WebAPI.Services.Validators
{
    public class StartDateTimeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateTime = (DateTime)value;
            return dateTime >= new DateTime(2000, 1, 1) && dateTime <= DateTime.Now;
        }
    }
}

using System.ComponentModel.DataAnnotations;


namespace ScientificExperiment.Tests.Validation
{
    internal class ValidationModel
    {
        public static List<ValidationResult> ValidateModel<T>(T model)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
            return results;
        }
    }
}

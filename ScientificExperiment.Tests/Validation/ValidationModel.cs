using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_CSV.Models;

namespace WebApi.Tests.Validation
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

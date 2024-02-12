using FluentValidation;
using WebApi_CSV.Models;

namespace WebApi_CSV.Services.Validation
{
    public class ValueModelValidator: AbstractValidator<ValueModel>
    {
        public ValueModelValidator()
        {
            RuleFor(x => x.StartDateTime)
                .Must(BeValidDateTime)
                .WithMessage("Дата проведения эксперимента должна быть между 01.01.2000 и текущей датой");

            RuleFor(x => x.WorkTime)
                .GreaterThan(0)
                .WithMessage("Время проведения эксперимента не может быть меньше или равно нулю");

            RuleFor(x => x.Indicator)
                .GreaterThan(0)
                .WithMessage("Значение показателя не может быть меньше или равно нулю");
        }
        public bool BeValidDateTime(DateTime dateTime)
        {
            return dateTime >= new DateTime(2000, 1, 1) && dateTime <= DateTime.Now;
        }
    }
}

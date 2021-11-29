using FluentValidation;
using Notifier.Server.Dto.RequestModels.User;

namespace Notifier.Server.Validations.User
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterModel>
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Please specify a {PropertyName}")
                .Length(3, 30).WithMessage("{PropertyName} length must between 3 and 30");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Please specify a {PropertyName}")
                .Length(6, 30).WithMessage("{PropertyName} length must between 6 and 30");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Please specify a {PropertyName}")
                .Length(6, 30).WithMessage("{PropertyName} length must between 6 and 20")
                .Equal(c => c.Password).WithMessage("The entered passwords are inconsistent");
        }
    }
}

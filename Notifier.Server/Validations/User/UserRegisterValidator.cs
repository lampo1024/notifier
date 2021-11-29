using FluentValidation;
using Notifier.Server.Dto.RequestModels.User;

namespace Notifier.Server.Validations.User
{
    public class UserRegisterValidator : AbstractValidator<UserRegisterModel>
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Please specify a user name")
                .Length(3, 30).WithMessage("user name must between 3 and 30");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Please specify a user name")
                .Length(6, 30).WithMessage("password must between 6 and 30");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Please specify a user name")
                .Length(6, 30).WithMessage("length must between 6 and 20")
                .Equal(c => c.Password).WithMessage("password must be equal");
        }
    }
}

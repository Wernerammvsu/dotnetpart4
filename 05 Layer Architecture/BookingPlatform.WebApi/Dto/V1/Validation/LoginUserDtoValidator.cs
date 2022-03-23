using FluentValidation;

namespace BookingPlatform.WebApi.Dto.V1.Validation;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
	public LoginUserDtoValidator()
	{
		RuleFor(b => b.Username).NotEmpty();
		RuleFor(b => b.Password).NotEmpty();
	}
}

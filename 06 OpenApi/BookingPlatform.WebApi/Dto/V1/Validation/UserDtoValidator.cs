using FluentValidation;

namespace BookingPlatform.WebApi.Dto.V1.Validation;

public class UserDtoValidator : AbstractValidator<UserDto>
{
	public UserDtoValidator()
	{
		RuleFor(b => b.Username).MinimumLength(4);
		RuleFor(b => b.Password).MinimumLength(6);
	}
}

using DtoValidation.Configuration;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace DtoValidation.Dto.V2.Validation
{
	public class BookingDtoValidator : AbstractValidator<BookingDto>
	{
		public BookingDtoValidator(IOptions<DatabaseConfiguration> options)
		{
			RuleFor(b => b.Username).NotEmpty();
			RuleFor(b => b.RoomName).NotEmpty();
			RuleFor(b => b.FromUtc).NotNull();
			RuleFor(b => b.ToUtc).NotNull();
			RuleFor(b => b.Comment).MinimumLength(5).MaximumLength(20);
			RuleFor(b => b.ToUtc).Must((dto, toUtc) => (toUtc!.Value - dto.FromUtc!.Value).TotalMinutes > 30).WithMessage(options.Value.ConnectionString);
		}
	}
}

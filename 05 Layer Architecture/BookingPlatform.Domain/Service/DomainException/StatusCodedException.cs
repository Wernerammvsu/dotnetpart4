namespace BookingPlatform.Domain.Service.DomainException
{
	public class StatusCodedException : Exception
	{
		public readonly int StatusCode;

		public StatusCodedException(int statusCode, string message) : base(message)
		{
			StatusCode = statusCode;
		}
	}
}
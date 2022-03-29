namespace BookingPlatform.Domain.Service.DomainException
{
	// Say about Microsoft.AspNetCore.Http.BadHttpRequestException
	public class RoomNotFoundException : StatusCodedException
	{
		private const string MessageTemplate = "Room with id '{0}' is not found";

		public RoomNotFoundException(int roomId)
			: base(statusCode: 301, string.Format(MessageTemplate, roomId))
		{
		}
	}
}

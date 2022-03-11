using Booking.Domain;
using System;

namespace Booking.WebApi;

public class TimeProvider : ITimeProvider
{
	public DateTime UtcNow => DateTime.UtcNow;
}

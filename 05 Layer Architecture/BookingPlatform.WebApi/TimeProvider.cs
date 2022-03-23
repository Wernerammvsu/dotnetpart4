using BookingPlatform.Domain;
using System;

namespace BookingPlatform.WebApi;

public class TimeProvider : ITimeProvider
{
	public DateTime UtcNow => DateTime.UtcNow;
}

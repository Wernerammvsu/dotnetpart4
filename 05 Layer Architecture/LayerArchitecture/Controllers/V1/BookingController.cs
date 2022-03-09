﻿using Booking.DataAccess;
using Booking.Domain.Service;
using Booking.WebApi.Dto.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Booking.WebApi.Controllers.V1;

[ApiController]
[Route("v1/booking")]
public class BookingController : Controller
{
	private readonly BookingContext _bookingContext;
	private readonly IBookingService _bookingService;

	public BookingController(BookingContext bookingContext,
		IBookingService bookingService)
	{
		_bookingContext = bookingContext;
		_bookingService = bookingService;
	}

	[Route("{id}")]
	public IActionResult Get(int id)
	{
		return Ok(id);
	}

	public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
	{
		var bookings = await _bookingService.FindBookingsWithDefaultRoomAsync();
		return Ok(bookings.Select(b => BookingDto.FromBooking(b)));
	}

	[HttpPost]
	public async Task<ActionResult<BookingDto>> CreateBooking(BookingDto bookingDto)
	{
		// Validate Dto
		if (!ModelState.IsValid)
			return BadRequest(ModelState);
		if (bookingDto.FromUtc < DateTime.UtcNow)
			return BadRequest("Cannot have from date earlier than now");
		if (bookingDto.ToUtc - bookingDto.FromUtc <= TimeSpan.FromMinutes(30))
			return BadRequest("Booking period should be at lease 30 minutes long");

		// Find User
		DataAccess.Models.UserDAL? user = await _bookingContext
			.Users
			.FirstOrDefaultAsync(u => u.UserName == bookingDto.Username);
		if (user is null)
			return BadRequest($"User with name '{bookingDto.Username}' cannot be found");

		var newBooking = bookingDto.ToBooking(user.Id);
		newBooking = await _bookingService.CreateBookingAsync(newBooking);

		return Ok(BookingDto.FromBooking(newBooking));
	}
}

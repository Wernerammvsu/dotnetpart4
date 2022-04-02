﻿using BookingPlatform.DataAccess.Models;
using BookingPlatform.Domain.Entity;

namespace BookingPlatform.DataAccess.Mapper
{
    public interface IBookingMapper
    {
        Booking Map(BookingDAL bookingDAL);
        BookingDAL Map(Booking booking);
    }
}
using AutoMapper;
using DtoValidation.DataAccess.Entity;
using DtoValidation.Dto;
using System;

namespace DtoValidation.Dto.Mapping
{
	public class DefaultAutoMapperProfile : Profile
	{
		public DefaultAutoMapperProfile()
		{
			CreateMap<Booking, V2.BookingDto>()
				.ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User!.UserName))
				.ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room!.RoomName));
			/*	ForMember(dest => dest.FromUtc, opt => opt.MapFrom(src => src.FromUtc!.Value
					if (src.FromUtc!.HasValue)
				throw new ArgumentNullException("FromUtc");));*/
		}

		//public Booking ToBooking(int userId)
		//{ 
		//	if (!FromUtc.HasValue)
		//		throw new ArgumentNullException("FromUtc");
		//	if (!ToUtc.HasValue)
		//		throw new ArgumentNullException("ToUtc");
		//	return new Booking(comment: Comment,
		//		fromUtc: DateTime.SpecifyKind(FromUtc.Value, DateTimeKind.Utc),
		//		toUtc: DateTime.SpecifyKind(ToUtc.Value, DateTimeKind.Utc),
		//		userId: userId);
		//}
	}
}

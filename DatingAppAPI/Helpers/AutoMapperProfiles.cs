using AutoMapper;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Extensions;

namespace DatingAppAPI.Helpers
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<AppUser, MemberDto>()
				.ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
				.ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
			//If MemberDto has a property that is the same as a property in AppUser, it will be mapped
			// Also, in the case of GetAge, if you have a property, say named "Age" and another property that is Get-Property ("GetAge")
			// then Automapper will map that
			CreateMap<Photo, PhotoDto>();
			CreateMap<MemberUpdateDto, AppUser>();
			CreateMap<RegisterDto, AppUser>();
			CreateMap<Message, MessageDto>()
				.ForMember(d => d.SenderPhotoUrl,
					o => o.MapFrom(
						s => s.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
								.ForMember(d => d.SenderPhotoUrl,
					o => o.MapFrom(
						s => s.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));

			CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
			CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);

		}
	}
}

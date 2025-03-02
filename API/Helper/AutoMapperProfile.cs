using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, MemberDto>()
                     .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                         src.Photos.FirstOrDefault(x => x.IsMain)!.Url))
                     .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));

            //CreateMap<AppUser, MemberDto>()
            //    .ForMember(d => d.Age, o=>o.MapFrom(s=>s.DateOfBirth.CalculateAge()))
            //    .ForMember(d => d.PhotoUrl, o =>
            //    o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<string, DateOnly>().ConvertUsing(x => DateOnly.Parse(x));
            CreateMap<Message, MessageDto>()
                .ForMember(p => p.SenderUserPhotoUrl, o => o.MapFrom(s => s.Sender.Photos
                .FirstOrDefault(x => x.IsMain)!.Url))
                 .ForMember(p => p.RecipientUserPhotoUrl, o => o.MapFrom(s => s.Recipient.Photos
                .FirstOrDefault(x => x.IsMain)!.Url));
            // select url from phtot where photo.isMain == true

            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue
            ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
            //compare local time to utc time


        }
    }
}

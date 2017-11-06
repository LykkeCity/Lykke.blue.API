using AutoMapper;
using Lykke.blue.Api.Requests;
using Lykke.blue.Api.Responses;
using Lykke.Service.Registration.Models;

namespace Lykke.blue.Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AuthRequestModel, AuthModel>()
                .ForMember(dest => dest.UserAgent, opt => opt.Ignore())
                .ForMember(dest => dest.Ip, opt => opt.Ignore());

            CreateMap<AuthResponse, AuthResponseModel>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token));
        }
    }
}

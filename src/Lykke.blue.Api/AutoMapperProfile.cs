using AutoMapper;
using ApiRequests = Lykke.blue.Api.Requests;
using ApiResponses = Lykke.blue.Api.Responses;
using ClientModel = Lykke.Service.Pledges.Client.AutorestClient.Models;
using Lykke.blue.Api.Responses;
using Lykke.Service.Registration.Models;

namespace Lykke.blue.Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApiRequests.AuthRequestModel, AuthModel>()
                .ForMember(dest => dest.UserAgent, opt => opt.Ignore())
                .ForMember(dest => dest.Ip, opt => opt.Ignore());

            CreateMap<AuthResponse, AuthResponseModel>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.Token));

            CreateMap<ApiRequests.CreatePledgeRequest, ClientModel.CreatePledgeRequest>()
                .ForMember(dest => dest.ClientId, opt => opt.Ignore());

            CreateMap<ClientModel.CreatePledgeResponse, ApiResponses.CreatePledgeResponse>();
            CreateMap<ClientModel.GetPledgeResponse, ApiResponses.GetPledgeResponse>();
            CreateMap<ApiRequests.UpdatePledgeRequest, ClientModel.UpdatePledgeRequest>();
            CreateMap<ClientModel.UpdatePledgeResponse, ApiResponses.UpdatePledgeResponse>();
        }
    }
}

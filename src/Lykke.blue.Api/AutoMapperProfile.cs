using AutoMapper;
using Lykke.blue.Api.AzureRepositories.LykkeSettings;
using Lykke.blue.Api.Core.Settings.LykkeSettings;
using Lykke.blue.Api.Responses;
using Lykke.blue.Api.Responses.ReferralLinks.Offchain;
using Lykke.blue.Service.ReferralLinks.Client.AutorestClient.Models;
using Lykke.Service.Registration.Models;
using ApiRequests = Lykke.blue.Api.Requests;
using ClientModel = Lykke.Service.Pledges.Client.AutorestClient.Models;

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

            CreateMap<ClientModel.CreatePledgeResponse, CreatePledgeResponse>();
            CreateMap<ClientModel.GetPledgeResponse, GetPledgeResponse>();
            CreateMap<ApiRequests.UpdatePledgeRequest, ClientModel.UpdatePledgeRequest>();
            CreateMap<ClientModel.UpdatePledgeResponse, UpdatePledgeResponse>();
            CreateMap<LykkeGlobalSettingsEntity, LykkeGlobalSettings>();
            CreateMap<OffchainTradeRespModel, RefLinksTransferOffchainResponse>();
            CreateMap<OffchainSuccessTradeRespModel, RefLinksTransferOffchainResponse>();
        }
    }
}

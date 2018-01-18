using System.Runtime.Serialization;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    public abstract class RefLinksBaseRequestModel<T>
    {
        [IgnoreDataMember]
        public abstract string LogMessage { get;}
        public abstract T ConvertToServiceModel();
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Lykke.blue.Api.Models.RefLinksModels
{
    public abstract class RefLinksBaseRequestModel<T>
    {
        [IgnoreDataMember]
        abstract public string LogMessage { get;}
        abstract public T ConvertToServiceModel();


        //public static OffchainFinalizeModel ConvertToServiceModel(this FinalizeTransferModel src)
        //{
        //    return new OffchainFinalizeModel
        //    {
        //        TransferId = src.TransferId,
        //        ClientRevokePubKey = src.ClientRevokePubKey,
        //        ClientRevokeEncryptedPrivateKey = src.ClientRevokeEncryptedPrivateKey,
        //        SignedTransferTransaction = src.SignedTransferTransaction,
        //        ClientId = src.ClientId,
        //        RefLinkId = src.RefLinkId
        //    };
        //}
    }
}

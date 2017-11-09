using Lykke.blue.Service.InspireStream.Client.AutorestClient.Models;
using System;

namespace Lykke.blue.Api.Models.TwitterModels
{
    public class TweetsRequestModel
    {
        public bool IsExtendedSearch { get; set; }
        public string AccountEmail { get; set; }
        public string SearchQuery { get; set; }
        public int MaxResult { get; set; }
        public DateTime UntilDate { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public TweetsSearchModel CreateReques(TweetsRequestModel src)
        {
            return new TweetsSearchModel()
            {
                AccountEmail = src.AccountEmail,
                IsExtendedSearch = src.IsExtendedSearch,
                MaxResult = src.MaxResult,
                PageNumber = src.PageNumber,
                PageSize = src.PageSize,
                SearchQuery = src.SearchQuery,
                UntilDate = src.UntilDate
            };
        }
    }
}

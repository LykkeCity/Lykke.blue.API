namespace Lykke.blue.Api.Models.ClientsModels
{
    public class UsersCountResponseModel
    {
        public int Count { get; set; }

        public static UsersCountResponseModel Create(int count)
        {
            return new UsersCountResponseModel()
            {
                Count = count
            };
        }
    }
}

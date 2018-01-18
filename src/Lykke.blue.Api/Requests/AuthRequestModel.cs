namespace Lykke.blue.Api.Requests
{
    public class AuthRequestModel
    {
        // ReSharper disable once UnusedMember.Global
        // used by AutoMapper
        public string Email { get; set; }
        public string Password { get; set; }
        public string ClientInfo { get; set; }
        public string PartnerId { get; set; }
    }
}

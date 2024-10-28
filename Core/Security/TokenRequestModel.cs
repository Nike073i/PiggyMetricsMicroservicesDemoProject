namespace Core.Security
{
    public class TokenRequestModel
    {
        public string AuthorityUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}

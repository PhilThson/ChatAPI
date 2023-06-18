namespace ChatAPI.Domain.Settings
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public string ExpirationTimeMin { get; set; }
        public string RefreshTokenExpirationTimeDays { get; set; }
    }
}
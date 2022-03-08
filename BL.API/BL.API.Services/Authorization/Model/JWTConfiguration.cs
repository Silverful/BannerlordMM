namespace BL.API.Services.Authorization.Model
{
    public class JWTConfiguration
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string SecretKey { get; set; }
        public int TokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}

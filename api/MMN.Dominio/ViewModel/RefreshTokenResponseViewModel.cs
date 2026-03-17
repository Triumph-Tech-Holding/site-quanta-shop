using MMN.Util.Enum;

namespace MMN.Dominio.ViewModel
{
    public class RefreshTokenResponseViewModel
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Error { get; set; }
        public EnumErrorTypeRefreshToken ErrorType { get; set; }
        public bool Success { get; set; }
    }
}

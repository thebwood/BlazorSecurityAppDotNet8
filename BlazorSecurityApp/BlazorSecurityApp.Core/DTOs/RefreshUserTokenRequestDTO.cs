namespace BlazorSecurityApp.Core.DTOs
{
    public class RefreshUserTokenRequestDTO
    {
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}

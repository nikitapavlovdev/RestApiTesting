namespace EfMobWebApiApp.Models
{
    public class RegisterEndpointsResponse
    {
        public bool Success { get; set; }
        public string Status {  get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}

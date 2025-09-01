namespace EfMobWebApiApp.Models
{
    public class UploadResponse
    {
        public bool Success { get; set; }
        public string Status { get; set; } = string .Empty;
        public string Message { get; set; } = string.Empty;
    }
}

namespace url_shortener.Models;
public class ShortenUrl
{
    public Guid id { get; set; }
    public string code { get; set; } = string.Empty;
    public string url { get; set; } = string.Empty;
    public string shortUrl { get; set; } = string.Empty;
    public DateTime createdOn { get; set; }
}
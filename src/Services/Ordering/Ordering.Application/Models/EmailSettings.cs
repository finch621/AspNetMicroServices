namespace Ordering.Application.Models;

public class EmailSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string? FromAddress { get; set; }
    public string? FromName { get; set; }
}

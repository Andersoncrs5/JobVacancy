namespace Api.Utils.Res;

public class ResponseTokens
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? ExpiredAt { get; set; }
    public DateTime? ExpiredAtRefreshToken { get; set; }
}
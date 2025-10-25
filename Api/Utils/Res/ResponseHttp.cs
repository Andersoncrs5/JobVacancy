namespace Api.Utils.Res;

public class ResponseHttp<T>
{
    public T? Data { get; set; }
    public string? Message { get; set; }
    public string? TraceId { get; set; }
    public int Code { get; set; }
    public int? Version { get; set; }
    public bool Status { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}
namespace JobVacancy.API.Configs.S3;

public class MinIOSettings
{
    public string ServiceURL { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
    public string Region { get; set; } = "us-east-1";
}
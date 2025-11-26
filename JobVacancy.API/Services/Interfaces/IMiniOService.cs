using JobVacancy.API.models.entities;
using Microsoft.AspNetCore.Mvc;
using Minio.DataModel;
using Minio.DataModel.Encryption;
using Minio.DataModel.Response;

namespace JobVacancy.API.Services.Interfaces;

public interface IMiniOService
{
    Task SetBucketPolicyToPublicRead(string bucketName);
    Task CreateNewBucketIfNotExists(string bucketName);
    Task<MemoryStream> GetPartialObject(
        string bucket,
        string objectName,
        long startBytes,
        long endBytes);
    string GetContentType(string fileName);
    Task<FileStreamResult> DownloadFile(string bucket, string fileName, string? versionId = null);
    Task<bool> ExistsFile(string bucket, string fileName);
    Task<bool> ExistsBucket(string bucket);
    Task RemoveIncompleteUpload(string bucket, string objectName, string uploadId);
    Task RemoveFile(string bucket, string fileName, string? versionId = null);
    Task<string> SetPresignedGet(string bucket, string fileName, int expiry, Dictionary<string, string>? reqParams);
    Task<ObjectStat> GetStatObjectAsync(string bucket, string fileName, string? versionId = null);
    Task RemoveObjects(string bucket, List<string> objects);
    Task<string> PresignedPutObject(string bucket, string fileName, int exp, string? versionId = null);
    Task<PutObjectResponse> UploadOptimized(string bucketName, IFormFile file, string? versionId = null);
    Task CopyFile(
        string fromBucketName,
        string fromObjectName,
        string destBucketName,
        string destObjectName,
        IServerSideEncryption? sseSrc = null,
        IServerSideEncryption? sseDest = null,
        string? versionId = null
    );
}

using JobVacancy.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel;
using Minio.DataModel.Args;
using Minio.DataModel.Encryption;
using Minio.DataModel.Response;
using Minio.Exceptions;

namespace JobVacancy.API.Services.Providers;

// SetBucketPolicy

public class MiniOService(IMinioClient client,ILogger<MiniOService> logger) : IMiniOService
{
    
    public async Task<MemoryStream> GetPartialObject(
        string bucket, 
        string objectName, 
        long startBytes, 
        long endBytes)
    {
        var partialStream = new MemoryStream();

        var args = new GetObjectArgs()
            .WithBucket(bucket)
            .WithObject(objectName)
            .WithOffsetAndLength(startBytes, endBytes - startBytes + 1)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(partialStream);
            });

        try
        {
            await client.GetObjectAsync(args).ConfigureAwait(false);
            
            partialStream.Position = 0;
            return partialStream;
        }
        catch (Minio.Exceptions.ObjectNotFoundException)
        {
            throw new FileNotFoundException($"Object '{objectName}' not found in bucket '{bucket}'.");
        }
        catch (Minio.Exceptions.BucketNotFoundException)
        {
            throw new BucketNotFoundException($"Bucket '{bucket}' not found.");
        }
        catch (MinioException ex)
        {
            logger.LogError(ex, $"Error getting partial object range {startBytes}-{endBytes} for '{objectName}': {ex.Message}");
            throw;
        }
    }
    
    public async Task RemoveIncompleteUpload(string bucket, string objectName, string uploadId)
    {
        try
        {
            var args = new RemoveIncompleteUploadArgs()
                .WithBucket(bucket)
                .WithObject(objectName);
            
            await client.RemoveIncompleteUploadAsync(args).ConfigureAwait(false);

            logger.LogInformation($"Incomplete upload for object '{objectName}' in bucket '{bucket}' removed successfully.");
        }
        catch (Minio.Exceptions.ObjectNotFoundException)
        {
            throw new FileNotFoundException($"Object '{objectName}' not found in bucket '{bucket}'.");
        }
        catch (Minio.Exceptions.BucketNotFoundException)
        {
            throw new BucketNotFoundException($"Bucket '{bucket}' not found.");
        }
        catch (MinioException ex)
        {
            logger.LogError(ex, $"Error removing incomplete upload for object '{objectName}': {ex.Message}");
            throw;
        }
    }
    
    public async Task<string> PresignedPutObject(string bucket, string fileName, int exp, string? versionId = null)
    {
        PresignedPutObjectArgs args = new PresignedPutObjectArgs()
            .WithBucket(bucket)
            .WithObject(fileName)
            .WithExpiry(exp);

        try
        {
            return await client.PresignedPutObjectAsync(args).ConfigureAwait(false);
        }
        catch (Minio.Exceptions.BucketNotFoundException)
        {
            throw new BucketNotFoundException($"Bucket '{bucket}' not found.");
        }
    }
    
    public async Task RemoveObjects(string bucket, List<string> objects)
    {
        if (objects.Count == 0)
            return;

        RemoveObjectsArgs args = new RemoveObjectsArgs()
            .WithBucket(bucket)
            .WithObjects(objects);

        try
        {
            await client.RemoveObjectsAsync(args);
        }
        catch (Minio.Exceptions.BucketNotFoundException)
        {
            throw new BucketNotFoundException($"Bucket '{bucket}' not found.");
        }
    }

    public async Task<ObjectStat> GetStatObjectAsync(string bucket, string fileName, string? versionId = null)
    {
        try
        {
            var args = new StatObjectArgs()
                .WithBucket(bucket)
                .WithObject(fileName);

            if (!string.IsNullOrEmpty(versionId))
                args.WithVersionId(versionId);

            return await client.StatObjectAsync(args);
        }
        catch (Minio.Exceptions.ObjectNotFoundException)
        {
            throw new FileNotFoundException($"Object '{fileName}' not found in bucket '{bucket}'.");
        }
        catch (Minio.Exceptions.BucketNotFoundException)
        {
            throw new BucketNotFoundException($"Bucket '{bucket}' not found.");
        }
    }
    
    public async Task<PutObjectResponse> UploadOptimized(string bucketName, IFormFile file, string? versionId = null)
    {
        await CreateNewBucketIfNotExists(bucketName); 
        
        if (file.Length == 0)
            throw new ArgumentNullException(nameof(file));

        var fileId = Guid.NewGuid().ToString();
        var extension = Path.GetExtension(file.FileName);
        var objectName = $"{fileId}{extension}";

        try
        {
            await using var uploadStream = file.OpenReadStream(); 

            PutObjectArgs putArgs = new PutObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithStreamData(uploadStream)
                .WithObjectSize(uploadStream.Length)
                .WithContentType(file.ContentType);

            if (!string.IsNullOrWhiteSpace(versionId))
                putArgs.WithVersionId(versionId);
        
            var response = await client.PutObjectAsync(putArgs);
        
            // logger.LogInformation($"Successfully uploaded {objectName}"); // Opcional
            return response;
        }
        catch (MinioException ex)
        {
            logger.LogError(ex, "MinIO error during upload: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "General error during upload: {Message}", ex.Message);
            throw;
        }
    }
    
    public async Task<FileStreamResult> DownloadFile(string bucket, string fileName, string? versionId = null)
    {
        if (!await ExistsBucket(bucket))
            throw new Exception("Bucket not found");

        if (!await ExistsFile(bucket, fileName))
            throw new Exception("File not found");

        var fileStream = new MemoryStream();

        var args = new GetObjectArgs()
            .WithBucket(bucket)
            .WithObject(fileName)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(fileStream);
            });

        if (!string.IsNullOrWhiteSpace(versionId))
            args.WithVersionId(versionId);
        
        await client.GetObjectAsync(args);

        fileStream.Position = 0;

        var contentType = GetContentType(fileName);

        return new FileStreamResult(fileStream, contentType)
        {
            FileDownloadName = fileName
        };
    }

    public async Task RemoveFile(string bucket, string fileName, string? versionId = null)
    {
        if (!await ExistsBucket(bucket))
            throw new Exception("Bucket not found");
        if (!await ExistsFile(bucket, fileName))
            throw new Exception("File not found");

        RemoveObjectArgs args = new RemoveObjectArgs()
            .WithBucket(bucket)
            .WithObject(fileName);
        
        if (!string.IsNullOrEmpty(versionId))
            args.WithVersionId(versionId);

        await client.RemoveObjectAsync(args);
    }

    public async Task<string> SetPresignedGet(string bucket, string fileName, int expiry, Dictionary<string, string>? reqParams)
    {
        if (!await ExistsBucket(bucket))
            throw new Exception("Bucket not found");
        
        if (!await ExistsFile(bucket, fileName))
            throw new Exception("File not found");

        PresignedGetObjectArgs args = new PresignedGetObjectArgs()
            .WithBucket(bucket)
            .WithObject(fileName)
            .WithExpiry(expiry)
            .WithHeaders(reqParams);

        string presignedGetObjectAsync = await client.PresignedGetObjectAsync(args).ConfigureAwait(false);
        return presignedGetObjectAsync;
    }
    
    public async Task CopyFile(
        string fromBucketName,
        string fromObjectName,
        string destBucketName,
        string destObjectName,
        IServerSideEncryption? sseSrc = null,
        IServerSideEncryption? sseDest = null,
        string? versionId = null
    )
    {
        if (!await ExistsBucket(fromBucketName))
            throw new Exception($"Source bucket '{fromBucketName}' not found.");

        if (!await ExistsBucket(destBucketName))
            throw new Exception($"Destination bucket '{destBucketName}' not found.");

        if (!await ExistsFile(fromBucketName, fromObjectName))
            throw new Exception($"Source file '{fromObjectName}' not found.");

        var cpSrcArgs = new CopySourceObjectArgs()
            .WithBucket(fromBucketName)
            .WithObject(fromObjectName)
            .WithServerSideEncryption(sseSrc);

        if (!string.IsNullOrEmpty(versionId))
            cpSrcArgs.WithVersionId(versionId);

        var args = new CopyObjectArgs()
            .WithBucket(destBucketName)
            .WithObject(destObjectName)
            .WithCopyObjectSource(cpSrcArgs)
            .WithServerSideEncryption(sseDest);

        await client.CopyObjectAsync(args).ConfigureAwait(false);
    }
    
    public async Task CreateNewBucketIfNotExists(string bucketName)
    {
        BucketExistsArgs existsArgs = new BucketExistsArgs().WithBucket(bucketName);
        bool found = await client.BucketExistsAsync(existsArgs).ConfigureAwait(false);
        
        if (!found)
        {
            var mbArgs = new MakeBucketArgs()
                .WithBucket(bucketName);
            await client.MakeBucketAsync(mbArgs).ConfigureAwait(false);
        }
    }
    
    public async Task SetBucketPolicyToPublicRead(string bucketName)
    {
        string policyJson = $@"{{
            ""Version"": ""2012-10-17"",
            ""Statement"": [
                {{
                    ""Sid"": ""AddPublicReadAcl"",
                    ""Effect"": ""Allow"",
                    ""Principal"": ""*"",
                    ""Action"": [""s3:GetObject""],
                    ""Resource"": [""arn:aws:s3:::{bucketName}/*""]
                }}
            ]
        }}";

        
        var policyArgs = new SetPolicyArgs()
            .WithBucket(bucketName)
            .WithPolicy(policyJson);

        try
        {
            await client.SetPolicyAsync(policyArgs).ConfigureAwait(false);
        }
        catch (Minio.Exceptions.BucketNotFoundException)
        {
            throw new BucketNotFoundException($"Bucket '{bucketName}' not found.");
        }
        catch (MinioException ex)
        {
            throw new MinioException(ex.Message, ex);
        }
    }
    
    public string GetContentType(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLower();

        return ext switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".pdf" => "application/pdf",
            ".txt" => "text/plain",
            ".json" => "application/json",
            ".zip" => "application/zip",
            _ => "application/octet-stream",
        };
    }

    public async Task<bool> ExistsFile(string bucket, string fileName)
    {
        try
        {
            var statArgs = new StatObjectArgs()
                .WithBucket(bucket)
                .WithObject(fileName);

            await client.StatObjectAsync(statArgs);
            return true;
        }
        catch (MinioException)
        {
            return false;
        }
    }
    
    public async Task<bool> ExistsBucket(string bucket)
    {
        BucketExistsArgs existsArgs = new BucketExistsArgs().WithBucket(bucket);
        return await client.BucketExistsAsync(existsArgs).ConfigureAwait(false);
    }
}
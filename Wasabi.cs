using System.Runtime.Versioning;
using Amazon.IdentityManagement;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

public class Wasabi
{
    private readonly string ACCESS_KEY;
    private readonly string SECRET_KEY;
    private readonly AmazonS3Client _s3;


    public Wasabi(string accessKey, string secretKey)
    {
        ACCESS_KEY = accessKey;
        SECRET_KEY = secretKey;

        var s3Config = new AmazonS3Config() { ServiceURL = "https://s3.wasabisys.com", ForcePathStyle = true };
        _s3 = new AmazonS3Client(new BasicAWSCredentials(ACCESS_KEY, SECRET_KEY), s3Config);
    }

    public async Task CreateBucketAsync(string bucketName, string region = "us-east-1", bool enableObjectLock = false)
    {
        try
        {
            var request = new PutBucketRequest
            {
                BucketName = bucketName,
                BucketRegionName = region,
                UseClientRegion = false,
                ObjectLockEnabledForBucket = enableObjectLock
            };

            await _s3.PutBucketAsync(request);

            if (enableObjectLock)
            {
                PutBucketVersioningRequest versioningRequest = new PutBucketVersioningRequest
                {
                    BucketName = bucketName,
                    VersioningConfig = new S3BucketVersioningConfig
                    {
                        Status = "Enabled"
                    }
                };
                await _s3.PutBucketVersioningAsync(versioningRequest);
            }

            Console.WriteLine($"Bucket {bucketName} created successfully.");
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task DeleteBucketAsync(string bucketName)
    {
        try
        {
            await _s3.DeleteBucketAsync(bucketName);
            Console.WriteLine($"Bucket {bucketName} deleted successfully.");
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task UploadFileAsync(string bucketName, string filePath, string fileName)
    {
        try
        {
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = fileName,
                FilePath = filePath,
                UseChunkEncoding = false,
            };
            await _s3.PutObjectAsync(request);
            Console.WriteLine($"File uploaded to bucket {bucketName} successfully.");
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task DeleteFileAsync(string bucketName, string key)
    {
        try
        {
            await _s3.DeleteObjectAsync(bucketName, key);
            Console.WriteLine($"File deleted from bucket {bucketName} successfully.");
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task DownloadFileAsync(string bucketName, string key)
    {
        
        try
        {
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), key);
            
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };

            using (var response = await _s3.GetObjectAsync(request))
            {
                await response.WriteResponseStreamToFileAsync(downloadPath, false, default);
            }

            Console.WriteLine($"File downloaded from bucket {bucketName} successfully. Path: {downloadPath}");

        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task ListBucketsAsync()
    {
        try
        {
            var response = await _s3.ListBucketsAsync();
            Console.WriteLine("Buckets:");

            foreach (var bucket in response.Buckets)
            {
                Console.WriteLine($"- {bucket.BucketName}");
            }
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
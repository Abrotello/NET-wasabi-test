using Amazon.IdentityManagement;
using Amazon.S3;

public class Wasabi(string accessKey, string secretKey)
{
    private readonly string ACCESS_KEY = accessKey;
    private readonly string SECRET_KEY = secretKey;


    private Boolean Connect()
    {
        if (string.IsNullOrEmpty(SECRET_KEY) || string.IsNullOrEmpty(ACCESS_KEY))
        {
            Console.WriteLine("Warning: Secret Key or Access Key is empty!");
            return false;
        }

        Console.WriteLine("Connecting to Wasabi with provided keys...");
        
        var iamConfig = new AmazonIdentityManagementServiceConfig { ServiceURL = "https://iam.wasabisys.com" };
        var iamClient = new AmazonIdentityManagementServiceClient(ACCESS_KEY, SECRET_KEY, iamConfig);
        
        try
        {
            var response = iamClient.ListUsersAsync().Result;
            Console.WriteLine($"Successfully connected to Wasabi. Number of users: {response.Users.Count}");
            return true;
        }
        catch (Exception ex)
        { 
            Console.WriteLine($"Error connecting to Wasabi: {ex.Message}");
            return false;
        }
    }

    public void CreateBucket(string bucketName, string region)
    {
        if (!Connect()) return;

        var s3Config = new AmazonS3Config() { ServiceURL = "https://s3.wasabisys.com" };
        var s3 = new AmazonS3Client(ACCESS_KEY, SECRET_KEY, s3Config);

        try
        {
            var putBucketRequest = new Amazon.S3.Model.PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            };

            var response = s3.PutBucketAsync(putBucketRequest).Result;
            Console.WriteLine($"Bucket '{bucketName}' created successfully in region '{region}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating bucket: {ex.Message}");
        }
    }

    public void ListBuckets()
    {
        if (!Connect()) return;

        var s3Config = new AmazonS3Config() { ServiceURL = "https://s3.wasabisys.com" };
        var s3 = new AmazonS3Client(ACCESS_KEY, SECRET_KEY, s3Config);

        try
        {
            var response = s3.ListBucketsAsync().Result;
            Console.WriteLine("Buckets:");
            foreach (var bucket in response.Buckets)
            {
                Console.WriteLine($"- {bucket.BucketName}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error listing buckets: {ex.Message}");
        }
    }

    public void DeleteBucket(string bucketName)
    {
        
    }
}
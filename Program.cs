using System;
using Amazon.IdentityManagement;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;

namespace AWSWasabi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // var iam_config = new AmazonIdentityManagementServiceConfig { ServiceURL = "https://iam.wasabisys.com" };
            var s3_config = new AmazonS3Config() { ServiceURL = "https://s3.us-west-1.wasabisys.com" };

            var chain = new CredentialProfileStoreChain();
            if (chain.TryGetProfile("wasabi", out var profile))
            {
                Console.WriteLine("Wasabi profile found.");
                var access_key = profile.Options.AccessKey;
                var secret_key = profile.Options.SecretKey;

                var s3 = new AmazonS3Client(access_key, secret_key, s3_config);
                var buckets = s3.ListBucketsAsync().Result;
                Console.WriteLine("Buckets:");
                foreach (var bucket in buckets.Buckets)
                {
                    Console.WriteLine(bucket.BucketName);
                }
            }
            else
            {
                Console.WriteLine("Wasabi profile not found.");
            }

        }
    }
}
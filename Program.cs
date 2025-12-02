using Amazon.Runtime.CredentialManagement;

namespace AWSWasabi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Main method");
            var chain = new CredentialProfileStoreChain();
            if (!chain.TryGetAWSCredentials("wasabi", out var profile))
            {
                Console.WriteLine("Could not find Wasabi credentials in the AWS credential store.");
                return;
            }
            var credentials = profile.GetCredentials();
            Wasabi wasabi = new(credentials.AccessKey, credentials.SecretKey);
            //wasabi.createBucket("bucket-de-mariana", "us-east-1");
            wasabi.ListBuckets();
        }
    }
}
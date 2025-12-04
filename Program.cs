using Amazon.Runtime.CredentialManagement;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AWSWasabi
{
    public static class Program
    {
        const string regex = "^[a-z]{3,}[a-z0-9[]*a-z0-9]";
        public static async Task Main(string[] args)
        {
            
            Console.WriteLine("Main method");
            Console.WriteLine("Running Wasabi S3 operations...");

            do 
            {
                Console.WriteLine("\nOptions:");
                Console.WriteLine("\n1. List Buckets");
                Console.WriteLine("2. Create Bucket");
                Console.WriteLine("3. Delete Bucket");
                Console.WriteLine("4. Upload File to Bucket");
                Console.WriteLine("5. Download File from Bucket");
                Console.WriteLine("6. Delete File from Bucket");
                Console.WriteLine("\nPress 'q' to quit");
                Console.Write("\nSelect an option (1-6): ");
                var option = Console.ReadLine();
                await ReadOption(option);
                Console.WriteLine("\n");
            } while (true || Console.ReadLine() != "q");
        }

        private async static Task ReadOption(string option)
        {
            var chain = new CredentialProfileStoreChain();
            if (!chain.TryGetAWSCredentials("wasabi", out var profile))
            {
                Console.WriteLine("Could not find Wasabi credentials in the AWS credential store.");
                return;
            }
            var credentials = profile.GetCredentials();
            Wasabi wasabi = new(credentials.AccessKey, credentials.SecretKey);
            switch (option)
            {
                case "1":
                    await wasabi.ListBucketsAsync();
                    break;
                case "2":
                    Console.Write("Enter bucket name: ");
                    string bucketName = Console.ReadLine();
                    await wasabi.CreateBucketAsync(bucketName);
                    break;
                case "3":
                    // Delete Bucket
                    break;
                case "4":
                    // Upload File to Bucket
                    break;
                case "5":
                    // Download File from Bucket
                    break;
                case "6":
                    // Delete File from Bucket
                    break;
                case "q":
                    break;
                default:
                    Console.WriteLine("Invalid option. Please select a valid option (1-6).");
                    break;
            }
        }
    }
}
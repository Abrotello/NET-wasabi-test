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

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Main method");
            Console.WriteLine("Running Wasabi S3 operations...");
            Console.ResetColor();

            string? option;
            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nOptions:");
                Console.WriteLine("\n1. List Buckets");
                Console.WriteLine("2. Create Bucket");
                Console.WriteLine("3. Delete Bucket");
                Console.WriteLine("4. Upload File to Bucket");
                Console.WriteLine("5. Download File from Bucket");
                Console.WriteLine("6. Delete File from Bucket");
                Console.WriteLine("7. List Files in Bucket");
                Console.WriteLine("\nPress 'q' to quit");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\nSelect an option (1-7): ");
                Console.ResetColor();
                option = Console.ReadLine() ?? string.Empty;
                await ReadOption(option);
                Console.WriteLine("\n");
            } while (option != "q");
        }

        private async static Task ReadOption(string option)
        {
            var chain = new CredentialProfileStoreChain();
            if (!chain.TryGetAWSCredentials("wasabi", out var profile))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Could not find Wasabi credentials in the AWS credential store.");
                Console.ResetColor();
                return;
            }
            var credentials = profile.GetCredentials();
            Wasabi wasabi = new(credentials.AccessKey, credentials.SecretKey);
            switch (option)
            {
                case "1":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    await wasabi.ListBucketsAsync();
                    Console.ResetColor();
                    break;
                case "2":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Enter bucket name: ");
                    Console.ResetColor();
                    string bucketName = Console.ReadLine() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(bucketName))
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        await wasabi.CreateBucketAsync(bucketName);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Bucket name cannot be empty.");
                        Console.ResetColor();
                    }
                    break;
                case "3":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Enter bucket name to delete: ");
                    Console.ResetColor();
                    string bucketNameToDelete = Console.ReadLine() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(bucketNameToDelete))
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        await wasabi.DeleteBucketAsync(bucketNameToDelete);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Bucket name cannot be empty.");
                        Console.ResetColor();
                    }
                    break;
                case "4":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Enter bucket name: ");
                    Console.ResetColor();
                    string bucketNameUpload = Console.ReadLine() ?? string.Empty;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Enter file path: ");
                    Console.ResetColor();
                    string filePath = Console.ReadLine() ?? string.Empty;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Enter file name in bucket: ");
                    Console.ResetColor();
                    string fileName = Console.ReadLine() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(bucketNameUpload) && !string.IsNullOrWhiteSpace(filePath) && !string.IsNullOrWhiteSpace(fileName))
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        await wasabi.UploadFileAsync(bucketNameUpload, filePath, fileName);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("All fields are required.");
                        Console.ResetColor();
                    }
                    break;
                case "5":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Enter bucket name: ");
                    Console.ResetColor();
                    string bucketNameDownload = Console.ReadLine() ?? string.Empty;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Enter file name to download: ");
                    Console.ResetColor();
                    string fileNameDownload = Console.ReadLine() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(bucketNameDownload) && !string.IsNullOrWhiteSpace(fileNameDownload))
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        await wasabi.DownloadFileAsync(bucketNameDownload, fileNameDownload);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("All fields are required.");
                        Console.ResetColor();
                    }
                    break;
                case "6":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Enter bucket name: ");
                    Console.ResetColor();
                    string bucketNameDeleteFile = Console.ReadLine() ?? string.Empty;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Enter file name to delete: ");
                    Console.ResetColor();
                    string fileNameDelete = Console.ReadLine() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(bucketNameDeleteFile) && !string.IsNullOrWhiteSpace(fileNameDelete))
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        await wasabi.DeleteFileAsync(bucketNameDeleteFile, fileNameDelete);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("All fields are required.");
                        Console.ResetColor();
                    }
                    break;
                case "7":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("Enter bucket name: ");
                    Console.ResetColor();
                    string bucketNameList = Console.ReadLine() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(bucketNameList))
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        await wasabi.ListFilesAsync(bucketNameList);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Bucket name cannot be empty.");
                        Console.ResetColor();
                    }
                    break;
                case "q":
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option. Please select a valid option (1-7).");
                    Console.ResetColor();
                    break;
            }
        }
    }
}
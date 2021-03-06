using System;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace azure_storage_account_sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "__connection__string";
            var containerName = "test-container";
            var container = new BlobContainerClient(connectionString, containerName);
            var accountName = "accountName";
            var accountKey = "accountKey";

            foreach (BlobItem blobItem in container.GetBlobs())
            {
                BlobClient blob = container.GetBlobClient(blobItem.Name);
                BlobSasBuilder sas = CreateSaSToken(blob);
                sas.SetPermissions(BlobSasPermissions.Read);
                StorageSharedKeyCredential storageSharedKeyCredential = new StorageSharedKeyCredential(accountName, accountKey);

                string sasToken = sas.ToSasQueryParameters(storageSharedKeyCredential).ToString();
                Console.WriteLine($"Sas Token:{sasToken}");
            }
            Console.WriteLine("Hello World!");
        }

        static BlobSasBuilder AttachSasPolicy(BlobClient blob) => new BlobSasBuilder
        {
            Identifier = "stored access policy identifier",
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
        };

        static BlobSasBuilder CreateSaSToken(BlobClient blob) => new BlobSasBuilder
        {
            BlobContainerName = blob.BlobContainerName,
            BlobName = blob.Name,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(1)
        };
    }
}

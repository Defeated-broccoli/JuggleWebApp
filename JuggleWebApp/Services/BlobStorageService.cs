using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using JuggleWebApp.Interfaces;
using System.Text.RegularExpressions;

namespace JuggleWebApp.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public BlobStorageService(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("AzureConnectionString");

            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> AddFile(IFormFile file, string containerName)
        {
            using var stream = file.OpenReadStream();
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            if(!await containerClient.ExistsAsync())
            {
                await containerClient.CreateAsync(PublicAccessType.Blob);
            }
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = containerClient.GetBlobClient(fileName);

            
            await blobClient.UploadAsync(stream);
            return blobClient.Uri.ToString();
        }

        public async Task<bool> DeleteFile(string file, string containerName)
        {
            var containerClient =  _blobServiceClient.GetBlobContainerClient(containerName);

            string pattern = @"[^/]+$";
            string blobName = Regex.Match(file, pattern).Value;

            var result = containerClient.DeleteBlobIfExists(blobName);

            return result;
        }
    }
}

using Azure.Storage.Blobs;
using Azure.Storage.Files;

namespace JuggleWebApp.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> AddFile(IFormFile file, string containerName);
        Task<bool> DeleteFile(string file, string containerName);

    }
}

using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MusicStore.Entities;
using MusicStore.Services.Interfaces;

namespace MusicStore.Services.Implementations;

public class AzureBlobStorageUploader : IFileUploader
{
    private readonly IOptions<AppSettings> _options;
    private readonly ILogger<AzureBlobStorageUploader> _logger;

    public AzureBlobStorageUploader(IOptions<AppSettings> options, ILogger<AzureBlobStorageUploader> logger)
    {
        _options = options;
        _logger = logger;
    }
    
    public async Task<string> UploadFileAsync(string? base64String, string? fileName)
    {
        if (string.IsNullOrEmpty(base64String) || string.IsNullOrEmpty(fileName))
        {
            return string.Empty;
        }

        try
        {
            var client = new BlobServiceClient(_options.Value.StorageConfiguration.Path);

            var container = client.GetBlobContainerClient("musicproject");

            var blob = container.GetBlobClient(fileName);

            await using var stream = new MemoryStream(Convert.FromBase64String(base64String));
            await blob.UploadAsync(stream, true);

            return $"{_options.Value.StorageConfiguration.PublicUrl}{fileName}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error subiendo el archivo {fileName}", fileName);

            return string.Empty;
        }
    }
}
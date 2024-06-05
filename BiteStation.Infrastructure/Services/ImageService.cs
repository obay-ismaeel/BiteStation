using BiteStation.Domain.Abstractions;
using BiteStation.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace BiteStation.Infrastructure.Services;
public class ImageService : IImageService
{
    public async Task<string> StoreAsync(IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            return null;
        }

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;

        var filePath = Path.Combine(FileSettings.ImagesPath, uniqueFileName);

        var fullPath = Path.Combine(FileSettings.WebRootPath, filePath);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        return filePath;
    }

    public void Delete(string? filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return;

        var fullPath = Path.Combine(FileSettings.WebRootPath, filePath);
        File.Delete(fullPath);
    }
}
using BiteStation.Domain.Attributes;
using BiteStation.Domain.Constants;

namespace BiteStation.Presentation.Dtos;

public class IncomingRestaurantDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    [MaxFileSize(FileSettings.MaxFileSizeInBytes)]
    [AllowedExtensions(FileSettings.AllowedExtensions)]
    public IFormFile ImageFile { get; set; }
}

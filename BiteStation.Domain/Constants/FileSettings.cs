namespace BiteStation.Domain.Constants;

public static class FileSettings
{
    public const string WebRootPath = "C:\\Users\\obayh\\OneDrive\\Desktop\\Projects\\ASP practice projects\\BiteStation\\BiteStation.Presentation\\wwwroot\\";
    public const string ImagesPath = "assets\\images";
    public const string AllowedExtensions = ".jpg,.png,.jpeg";
    public const int MaxFileSizeInMB = 1;
    public const int MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;
}

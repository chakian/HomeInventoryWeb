namespace HomeInv.Common.Constants;

public struct GlobalConstants
{
    public const long MaxAllowedFileSize = 20 * 1024 * 1024;
    public static readonly string[] AllowedImageExtensions = new string[] { ".png", ".jpg", ".jpeg", ".gif", ".tiff", ".bmp", ".svg" };

    public const string DefaultItemDefinitionImage = "/assets/s-logo-blue-white_128.png";

    public enum ImageSize
    {
        Minimum = 125,
        Medium = 250,
        MedMax = 500,
        Maximum = 1000,
    }
}

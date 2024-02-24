using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using static HomeInv.Common.Constants.GlobalConstants;

namespace HomeInv.Common.Utils;

public sealed class ImageUtility
{
    private const string extension = "jpg";
    private readonly string _basePath;
    private readonly string _baseFileName;
    private readonly string _imageUrlTemplate;

    public ImageUtility(int homeId, int itemDefinitionId)
    {
        _basePath = Path.Combine(Environment.CurrentDirectory, "wwwroot", "uploads", homeId.ToString());
        _baseFileName = $"item_{itemDefinitionId}";
        _imageUrlTemplate = $"/uploads/{homeId}/item_{itemDefinitionId}_" + "{0}." + extension;
    }

    public async Task HandleFileUpload(IBrowserFile uploadedFile)
    {
        DeleteExistingVariations();
        
        // Save the original image as JPEG with a maximum of 1000 pixels
        await SaveResizedCopy(uploadedFile, 1000);

        // Save resized copies
        foreach (ImageSize imageSize in Enum.GetValues(typeof(ImageSize)))
        {
            int imageSizeValue = (int)imageSize;
            await SaveResizedCopy(uploadedFile, imageSizeValue);
        }
    }

    private async Task SaveResizedCopy(IBrowserFile uploadedFile, int size)
    {
        await using var originalStream = uploadedFile.OpenReadStream(MaxAllowedFileSize);
        using var originalImage = await Image.LoadAsync(originalStream);
        // Resize the image
        // var (newWidth, newHeight) = CalculateSize(originalImage.Width, originalImage.Height, size);
        originalImage.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(Math.Min(size, originalImage.Width), Math.Min(size, originalImage.Height)),
            Mode = ResizeMode.Max
        }));

        // Generate the name for the resized image
        var resizedName = $"{_baseFileName}_{size}.{extension}";

        // Save the resized image
        await SaveImage(originalImage, resizedName);
    }

    private async Task SaveImage(Image image, string imageName)
    {
        var imagePath = Path.Combine(_basePath, imageName);
        await using var outputStream = new FileStream(imagePath, FileMode.Create);
        await image.SaveAsync(outputStream, new JpegEncoder());
    }

    private void DeleteExistingVariations()
    {
        var files = Directory.GetFiles(_basePath, $"{_baseFileName}_*").ToList();

        //TODO: The block below ensures the old image name versions to be deleted once the item definitions are saved with new images.
        files.AddRange(Directory.GetFiles(_basePath, $"{_baseFileName}.*"));
        //var oldFiles = Directory.GetFiles(_basePath, $"{_baseFileName}.*");
        //if (oldFiles.Any())
        //{
        //    foreach (var oldFile in oldFiles)
        //    {
        //        files.Add(oldFile);
        //    }
        //}
        
        if (files.Count == 0) return;
        foreach (var file in files)
        {
            File.Delete(file);
        }
    }

    private static Tuple<int, int> CalculateSize(int originalWidth, int originalHeight, int newSize)
    {
        if (originalWidth < newSize && originalHeight < newSize) return new Tuple<int, int>(originalWidth, originalHeight);
        
        int newWidth, newHeight;
        if (originalWidth > originalHeight)
        {
            newWidth = newSize;
            newHeight = (int)((float)newWidth / originalWidth * originalHeight);
        }
        else if (originalHeight > originalWidth)
        {
            newHeight = newSize;
            newWidth = (int)((float)newHeight / originalHeight * originalWidth);
        }
        else
        {
            newWidth = newHeight = newSize;
        }

        return new Tuple<int, int>(newWidth, newHeight);
    }

    public string GetImageDisplayLink(ImageSize imageSize)
    {
        if (!Directory.Exists(_basePath))
        {
            return DefaultItemDefinitionImage;
        }

        var files = Directory.GetFiles(_basePath, $"{_baseFileName}_*");
        if (files.Length <= 0) return DefaultItemDefinitionImage;

        var bestSize = (int)imageSize;

        if (!files.Any(f => f.Equals($"{_baseFileName}_{(int)imageSize}.{extension}")))
            bestSize = (int)ImageSize.Minimum;

        return string.Format(_imageUrlTemplate, bestSize);
    }
}
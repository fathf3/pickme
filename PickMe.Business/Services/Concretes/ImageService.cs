using Microsoft.AspNetCore.Http;
using PickMe.Business.Services.Abstractions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Logging;

public class ImageService : IImageService
{
    private readonly string _uploadFolderPath;
    

    public ImageService()
    {
        _uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "surveys");
        if (!Directory.Exists(_uploadFolderPath))
        {
            Directory.CreateDirectory(_uploadFolderPath);
        }
    }

    public async Task<string> UploadSurveyImageAsync(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
            return null;

        var uniqueFileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
        var filePath = Path.Combine(_uploadFolderPath, uniqueFileName);
       

        using (var stream = imageFile.OpenReadStream())
        {
            using (var image = await SixLabors.ImageSharp.Image.LoadAsync(stream))
            {
                // Maksimum 800x800 boyutuna küçültme
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(800, 800)
                }));

                // Sıkıştırılmış olarak kaydet (kaliteyi düşür)
                await image.SaveAsync(filePath, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder
                {
                    Quality = 75 // Kaliteyi %75'e düşürerek daha az yer kaplamasını sağla
                });
            }
        }

        return $"/images/surveys/{uniqueFileName}";
    }
}

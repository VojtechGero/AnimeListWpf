using AnimeListWpf.Models;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Media.Imaging;

namespace AnimeListWpf.Services;

public class ImageService
{
    private HttpClient _httpClient;
    private MD5 _md5;
    public ImageService()
    {
        _httpClient = new HttpClient();
        _md5 = MD5.Create();
    }
    public async Task<BitmapImage> DownloadImageToBitmap(string imageUrl)
    {
        var bytes = await _httpClient.GetByteArrayAsync(imageUrl);
        using var ms = new MemoryStream(bytes);
        var bitmap = new BitmapImage();

        bitmap.BeginInit();
        bitmap.CacheOption = BitmapCacheOption.OnLoad; // Important: loads immediately
        bitmap.StreamSource = ms;
        bitmap.EndInit();
        bitmap.Freeze();
        return bitmap;
    }
    //check all files
    //checkFile
    //download and save a file
    private string ComputeShortHash(string imageUrl)
    {
        byte[] hashBytes = _md5.ComputeHash(Encoding.UTF8.GetBytes(imageUrl));
        // 8 hex znaků = dostatečné
        return Convert.ToHexString(hashBytes).Substring(0, 8);
    }
    private string GetFileNameFromUrl(AContent content)
    {
        string hash = ComputeShortHash(content.ImageUrl);
        char type = content.IsAnime ? 'a' : 'm';
        string fileName = $"{type}{content.Id}-{hash}.jpg";
        return fileName;
    }
    public async Task DownloadImageToFile(AContent content)
    {
        byte[] bytes = await _httpClient.GetByteArrayAsync(content.ImageUrl);
        string fileName = GetFileNameFromUrl(content);
        string folder = "Images";
        Directory.CreateDirectory(folder);
        string fullPath = Path.Combine(folder, fileName);
        await File.WriteAllBytesAsync(fullPath, bytes);
    }
}

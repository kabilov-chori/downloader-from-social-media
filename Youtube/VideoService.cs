using YoutubeExplode;

namespace Youtube;

public class VideoService(YoutubeClient youtubeClient)
{
    public async Task<string> DownloadVideoAsync(string videoUrl, string outputPath, string fileName)
    {
        var streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(videoUrl);
        var videoStreamInfo = streamManifest
            .GetVideoOnlyStreams()
            .MaxBy(s => s.VideoQuality.MaxHeight);

        if (videoStreamInfo == null)
        {
            throw new Exception("Не удалось найти подходящий поток для видео.");
        }

        var videoFile = Path.Combine(outputPath, $"{fileName}_video.mp4");
        Console.WriteLine("Загрузка видео...");
        await youtubeClient.Videos.Streams.DownloadAsync(videoStreamInfo, videoFile);
        Console.WriteLine("Видео загружено.");

        return videoFile;
    }
}
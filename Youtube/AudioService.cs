using YoutubeExplode;

namespace Youtube;

public class AudioService(YoutubeClient youtubeClient)
{

    public async Task<string> DownloadAudioAsync(string videoUrl, string outputPath, string fileName)
    {
        var streamManifest = await youtubeClient.Videos.Streams.GetManifestAsync(videoUrl);
        var audioStreamInfo = streamManifest
            .GetAudioOnlyStreams()
            .MaxBy(s => s.Bitrate);

        if (audioStreamInfo == null)
        {
            throw new Exception("Не удалось найти подходящий поток для аудио.");
        }

        var audioFile = Path.Combine(outputPath, $"{fileName}.mp3");
        Console.WriteLine("Загрузка аудио...");
        await youtubeClient.Videos.Streams.DownloadAsync(audioStreamInfo, audioFile);
        Console.WriteLine("Аудио загружено.");

        return audioFile;
    }
}
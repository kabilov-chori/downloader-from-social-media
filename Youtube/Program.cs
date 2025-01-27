using Youtube;
using YoutubeExplode;

var youtube = new YoutubeClient();
var outputPath = "save_path"; // Путь для сохранения файлов

while (true)
{
    Console.WriteLine("Введите URL видео (или введите 'exit' для выхода):");
    string? videoUrl = Console.ReadLine();

    // Если пользователь вводит 'exit', выходим из цикла
    if (videoUrl?.ToLower() == "exit")
    {
        Console.WriteLine("Завершаем программу...");
        break;
    }

    if (string.IsNullOrEmpty(videoUrl))
    {
        Console.WriteLine("URL не может быть пустым. Попробуйте снова.");
        continue;
    }
    
    try
    {
        var video = await youtube.Videos.GetAsync(videoUrl);
        var videoId = video.Id;
        var videoTitle = video.Title;
        
        // Преобразуем название видео в безопасный формат для файловой системы
        var invalidChars = Path.GetInvalidFileNameChars();
        var safeTitle = string.Concat(videoTitle.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
        
        // Формируем имя файла как <название>_<айди>.mp4
        var fileName = $"{safeTitle}_{videoId}.mp4";
        var outputFile = Path.Combine(outputPath, fileName);

        var videoService = new VideoService(youtube);
        var audioService = new AudioService(youtube);

        // Загружаем видео и аудио
        var videoFile = await videoService.DownloadVideoAsync(videoUrl, outputPath, fileName);
        var audioFile = await audioService.DownloadAudioAsync(videoUrl, outputPath, fileName);

        // Объединяем видео и аудио с помощью Ffmpeg
        await FfmpegService.MuxAudioVideoAsync(videoFile, audioFile, outputFile);

        Console.WriteLine($"Готовый файл сохранен: {outputFile}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Произошла ошибка: {ex.Message}");
    }
}
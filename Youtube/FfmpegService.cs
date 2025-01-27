using System.Diagnostics;

namespace Youtube;

public static class FfmpegService
{
    public static async Task MuxAudioVideoAsync(string videoPath, string audioPath, string outputPath)
    {
        var ffmpegPath =
            @"path_of_ffmpeg.exe"; // путь локального приложения ffmpeg.exe
        var arguments = $"-i \"{videoPath}\" -i \"{audioPath}\" -c:v copy -c:a aac \"{outputPath}\"";

        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = ffmpegPath,
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        // Стартуем процесс и асинхронно ожидаем его завершение
        process.Start();

        // Читаем стандартный вывод и ошибки, чтобы отследить процесс
        var output = process.StandardOutput.ReadToEndAsync();
        var error = process.StandardError.ReadToEndAsync();

        // Ждем завершения процесса
        await Task.WhenAll(output, error);

        process.WaitForExit();

        if (process.ExitCode != 0)
        {
            throw new Exception($"Ошибка при объединении видео и аудио с помощью ffmpeg: {await error}");
        }

        Console.WriteLine("Процесс объединения завершен успешно.");
    }
}
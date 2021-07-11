using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Component.FileUpload.Helpers
{
    public class FileHelper
    {
        public static async Task SaveFile(string fullPath, IFormFile content)
        {
            using var stream = new FileStream(fullPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            await content.CopyToAsync(stream);
        }

        public static (string relativePath, string fullPath) CreateFilePath(string baseFolder, string folder, string fileName)
        {
            var uid = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(fileName);

            var savedFileName = $"{uid}{extension}";

            var todayPath = GetTodayPath();

            var timeFrame = TimeFrameDetection();

            var relativePath = Path.Combine(folder, todayPath, timeFrame, savedFileName);

            var fileDirectory = Path.Combine(baseFolder, folder, todayPath, timeFrame);

            Directory.CreateDirectory(fileDirectory);

            var fullPath = Path.Combine(fileDirectory, savedFileName);

            relativePath = $"/{relativePath}";

            return (relativePath, fullPath);
        }

        private static string GetTodayPath()
        {
            var today = DateTime.Today;

            var todayPath = Path.Combine(today.Year.ToString("0000"), today.Month.ToString("00"), today.Day.ToString("00"));

            return todayPath;
        }

        private static string TimeFrameDetection()
        {
            var h = DateTime.Now.TimeOfDay.Hours;

            return h <= 6 && h >= 0 ? "A" : h > 6 && h <= 12 ? "B" : h > 12 && h < 18 ? "C" : h >= 18 && h < 23 ? "D" : "A";
        }
    }
}

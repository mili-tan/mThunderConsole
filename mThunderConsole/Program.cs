using System;
using System.IO;
using ThunderSdk;

namespace mThunderConsole
{
    static class Program
    {
        static void Main(string[] args)
        {
            var manager = new DownloadManager(1, Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"));

            switch (args.Length)
            {
                case 1:
                    manager.CreateNewTask(args[0], args[0].Split('/')[args[0].Split('/').Length - 1]);
                    break;
                case 2:
                    manager.CreateNewTask(args[0], args[1]);
                    break;
                case 3:
                    manager.CreateNewTask(args[0], args[1], cookies: args[3]);
                    break;
                default:
                    Console.WriteLine("ThunderConsole/0.1");
                    Console.WriteLine("ThunderConsole [DownloadURL] (FileName) (Cookies)");
                    break;
            }

            manager.TaskDownload += (s, e) =>
            {
                if (!(s is DownFileInfo info)) return;

                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"{info.TaskInfo.Percent * 100:0.0000}% TotalSize: {info.TaskInfo.TotalSize} " +
                              $"/ {info.TaskInfo.TotalDownload} | Speed: {info.TaskInfo.Speed}" +
                              $"(P2P: {info.TaskInfo.SpeedP2P} P2S: {info.TaskInfo.SpeedP2S}) | OnlySource: {info.IsOnlyOriginal}");

            };

            manager.TaskCompleted += (s, e) =>
            {
                if (!(s is DownFileInfo info)) return;
                Console.WriteLine();
                Console.WriteLine(info.FileName + " Download Completed | " + info.WasteTime.Seconds + "s");
                Environment.Exit(0);
            };

            manager.TaskError += (s, e) =>
            {
                if (!(s is DownFileInfo info)) return;

                Console.WriteLine(info.FileName + " Download Error");
            };

            manager.StartAllTask();

            Console.ReadKey();
        }
    }
}

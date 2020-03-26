using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunicatorCms.Core.AppFileSystem;
using CommunicatorCms.Core.Settings;

namespace CommunicatorCms.Core
{
    public static class WebSampleInstaller
    {
        private static string WebSampleDirectoryAppPath { get; set; } = "/WebSample";

        public static void Install() 
        {
            var absPath = AppPath.ConvertAppPathToAbsolutePath(WebSampleDirectoryAppPath);

            var sampleSubDirs = Directory.GetDirectories(absPath);

            foreach (var sampleSubDir in sampleSubDirs)
            {
                var destinationDir = sampleSubDir.Replace(WebSampleDirectoryAppPath, GeneralSettings.WebRootPath);
                DirectoryCopy(sampleSubDir, destinationDir, true);
            }
        }
        public static bool IsEmpty() 
        {
            var absPath = AppPath.ConvertAppPathToAbsolutePath(WebSampleDirectoryAppPath);

            var sampleSubDirs = Directory.GetDirectories(absPath);

            foreach (var sampleSubDir in sampleSubDirs)
            {
                var destinationDir = sampleSubDir.Replace(WebSampleDirectoryAppPath, GeneralSettings.WebRootPath);

                if (Directory.Exists(destinationDir)) 
                {
                    return false;
                }
            }

            return true;
        }
        public static void InstallIfEmpty() 
        {
            if (IsEmpty()) 
            {
                Install();
            }
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories?redirectedfrom=MSDN

            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }
    }
}

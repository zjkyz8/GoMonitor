using System.IO;
using System.Linq;
using System.Threading;

namespace GoMonitor
{
    public class LocalFileManager
    {
        private bool threadShouldStop;

        public void Start()

        {
            new Thread(ManageLocalFile).Start();
        }

        private void ManageLocalFile()
        {
            while (!threadShouldStop)
            {
                var fileNeedToKeep = GetNewestFileName();
                var currentDirectory = Directory.GetCurrentDirectory();
                var files = Directory.GetFiles(currentDirectory, "*.xml", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    if (!file.Equals(fileNeedToKeep))
                        File.Delete(file);
                }
                Thread.Sleep(10000);
            }
        }

        public byte[] GetNewestFileContent()
        {
            var newestFile = GetNewestFileName();
            if(string.IsNullOrEmpty(newestFile))
                return new byte[0];
            using (var fileStream = new FileStream(newestFile, FileMode.Open, FileAccess.Read))
            {
                var fileContent = new byte[fileStream.Length];
                fileStream.Read(fileContent, 0, fileContent.Length);
                return fileContent;
            }
        }

        public string GetNewestFileName()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var files = Directory.GetFiles(currentDirectory, "*.xml", SearchOption.TopDirectoryOnly);
            var fileName = files.OrderByDescending(x => x).FirstOrDefault();
            if (string.IsNullOrEmpty(fileName))
            {
                return string.Empty;
            }
            return fileName;
        }

        public void Stop()
        {
            threadShouldStop = true;
        }
    }
}
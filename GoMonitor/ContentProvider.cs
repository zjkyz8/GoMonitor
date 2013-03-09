using System;
using System.IO;
using System.Net;

namespace GoMonitor
{
    public class ContentProvider
    {
        public byte[] GetContent(string url)
        {
            using (var Client = new WebClient())
            {
                return Client.DownloadData(url);
            }
        }

        public string WriteContent(byte[] content)
        {
            var fileName = GetFileName();
            using(var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(content, 0, content.Length);
            }
            return fileName;
        }

        private static string GetFileName()
        {
            return string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), "xml");
        }
    }
}
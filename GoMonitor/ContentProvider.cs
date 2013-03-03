using System;
using System.Net;

namespace GoMonitor
{
    public class ContentProvider
    {
        public string GetContent(string url)
        {
            var fileName = GetFileName();
            using (var Client = new WebClient())
            {
                Client.DownloadFile(url, fileName);
            }
            return fileName;
        }

        private static string GetFileName()
        {
            return string.Format("{0}.{1}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), "xml");
        }
    }
}
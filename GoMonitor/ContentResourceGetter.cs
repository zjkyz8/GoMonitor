using System.Threading;

namespace GoMonitor
{
    public class ContentResourceGetter
    {
        private readonly ContentProvider contentProvider;
        private bool threadShouldStop;
        private readonly LocalFileManager localFileManager;
        private const string URL = "http://10.18.7.153:8153/go/cctray.xml";

        public ContentResourceGetter()
        {
            localFileManager = new LocalFileManager();
            contentProvider = new ContentProvider();
        }

        public void Start()
        {
            new Thread(GetContent).Start();
        }

        private void GetContent()
        {
            while (!threadShouldStop)
            {
                var content = contentProvider.GetContent(URL);
                var remoteMD5 = Utility.CalculateContentMD5(content);
                var localMD5 = Utility.CalculateContentMD5(localFileManager.GetNewestFileContent());
                if (!remoteMD5.Equals(localMD5))
                {
                    contentProvider.WriteContent(content);
                }
                Thread.Sleep(10000);
            }
        }

        public void Stop()
        {
            threadShouldStop = false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace GoMonitor
{
    public class ContentService
    {
        private readonly ContentProvider provider;
        private readonly string fileName;
        private readonly TabControl tabControl;

        public ContentService()
        {
            provider = new ContentProvider();
            fileName = provider.GetContent("http://10.18.7.153:8153/go/cctray.xml");
        }

        public IList<KeyValuePair<string, XmlNode>> GetFormatedContent()
        {
            var doc = new XmlDocument();
            doc.Load(fileName);
            return doc.SelectNodes("/Projects/Project").Cast<XmlNode>().Select(x=>new KeyValuePair<string, XmlNode>(x.Attributes["name"].Value, x)).ToList();
        }

        public IEnumerable<JobEntity> TranslateJobs(IEnumerable<KeyValuePair<string, XmlNode>> jobList)
        {
            IList <JobEntity> jobs= new List<JobEntity>();
            foreach (var item in jobList)
            {
                var keys = item.Key.Split(new string[] {" :: "}, StringSplitOptions.RemoveEmptyEntries);

                var job = new JobEntity();
                if (keys.Length >= 1)
                {
                    job.PipeLineName = keys[0];
                }
                if(keys.Length>=2)
                {
                    job.StageName = keys[1];
                }
                if(keys.Length>=3)
                {
                    job.JobName = keys[2];
                }
                jobs.Add(job);
            }
            return jobs;
        }
    }

    public class JobEntity
    {
        public string PipeLineName { get; set; }

        public string StageName { get; set; }

        public string JobName { get; set; }
    }
}
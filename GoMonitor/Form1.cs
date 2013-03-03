using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace GoMonitor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            contentChangeMonitor.RunWorkerAsync();
        }

        private void contentChangeMonitor_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var jobList = e.UserState as IList<JobEntity>;
            FillTabContent(jobList);
        }

        private void contentChangeMonitor_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var contentService = new ContentService();
            var formatedContent = contentService.GetFormatedContent();
            var jobs = contentService.TranslateJobs(formatedContent);
            worker.ReportProgress(0, jobs);
            Thread.Sleep(5000);
        }
   
        private void FillTabContent(IEnumerable<JobEntity> jobList)
        {
            tabControl1.TabPages.Clear();
            foreach (var jobEntity in jobList)
            {
                if(!tabControl1.TabPages.ContainsKey(jobEntity.PipeLineName))
                {
                    tabControl1.TabPages.Add(jobEntity.PipeLineName, jobEntity.PipeLineName);
                    tabControl1.TabPages[jobEntity.PipeLineName].Controls.Add(new TableLayoutPanel());
                    if(!tabControl1.TabPages[jobEntity.PipeLineName].Controls.ContainsKey(jobEntity.StageName))
                    {
                        tabControl1.TabPages[jobEntity.PipeLineName].Controls[0].Controls.Add(new Label{Text = jobEntity.StageName});
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

            var rowCount = new Dictionary<string, int>();
            foreach (var jobEntity in jobList)
            {
                TableLayoutPanel tableLayoutPanel;

                if(!tabControl1.TabPages.ContainsKey(jobEntity.PipeLineName))
                {
                    tabControl1.TabPages.Add(jobEntity.PipeLineName, jobEntity.PipeLineName);
                    var layoutPanel = new TableLayoutPanel {Dock = DockStyle.Fill, BorderStyle = BorderStyle.Fixed3D};
                    tabControl1.TabPages[jobEntity.PipeLineName].Controls.Add(layoutPanel);                    
                }

                if (!tabControl1.TabPages[jobEntity.PipeLineName].Controls[0].Controls.ContainsKey(jobEntity.StageName))
                {
                    tableLayoutPanel = (tabControl1.TabPages[jobEntity.PipeLineName].Controls[0] as TableLayoutPanel);
                    if(tableLayoutPanel.Controls.Find(jobEntity.StageName, false).Length==0)
                    {
                        tableLayoutPanel.Controls.Add(new Label { Text = jobEntity.StageName, Name = jobEntity.StageName, Dock = DockStyle.Fill, Font = new Font(FontFamily.GenericSerif, 40) }, tableLayoutPanel.ColumnCount, tableLayoutPanel.RowCount);
                        tableLayoutPanel.ColumnCount++;
                        tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent));
                        var width = 100 / tableLayoutPanel.ColumnCount;
                        foreach (ColumnStyle style in tableLayoutPanel.ColumnStyles)
                        { 
                            style.Width = width;
                        }
                        rowCount.Add(jobEntity.StageFullName, 1);
                    }
                }

                tableLayoutPanel = (tabControl1.TabPages[jobEntity.PipeLineName].Controls[0] as TableLayoutPanel);
                var label = tableLayoutPanel.Controls.Find(jobEntity.StageName, false)[0];
                var columnPosition = tableLayoutPanel.GetColumn(label);

                var barColor = jobEntity.LastBuildStatus ? Color.Green : Color.Red;
                var barStyle = jobEntity.IsBuilding ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;

                var progressBar = new TextProgressBar
                                      {
                                          Name = jobEntity.JobName,
                                          Text = jobEntity.JobName,
                                          Dock = DockStyle.Fill,
                                          Style = barStyle,
                                          BackColor = barColor,
                                      };
                tableLayoutPanel.Controls.Add(progressBar, columnPosition, rowCount[jobEntity.StageFullName]);
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent));
                rowCount[jobEntity.StageFullName]++;

                var height = 100 / rowCount[jobEntity.StageFullName];
                foreach (RowStyle style in tableLayoutPanel.RowStyles)
                {
                    style.Height = height;
                }

            }
        }
    }



    [ToolboxItem(true)]
    class TextProgressBar : ProgressBar
    {

        [System.Runtime.InteropServices.DllImport("user32.dll ")]
        static extern IntPtr GetWindowDC(IntPtr hWnd);
        
        [System.Runtime.InteropServices.DllImport("user32.dll ")]
        static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        private Color _TextColor = Color.Black;

        private Font _TextFont = new Font("SimSun ", 12);

        public Color TextColor
        {
            get { return _TextColor; }
            set
            {
                _TextColor = value;
                Invalidate();
            }
        }

        public Font TextFont
        {
            get { return _TextFont; }
            set
            {
                _TextFont = value;
                Invalidate();
            }
        }

        protected override void WndProc(ref   Message m)
        {
            base.WndProc(ref   m);
            if (m.Msg == 0xf || m.Msg == 0x133)
            {  
                IntPtr hDC = GetWindowDC(m.HWnd);
                if (hDC.ToInt32() == 0)
                {
                    return;
                }
                var g = Graphics.FromHdc(hDC);
                var brush = new SolidBrush(_TextColor);
                SizeF size = g.MeasureString(Text, _TextFont);
                var x = (Width - size.Width) / 2;
                var y = (Height - size.Height) / 2;
                g.DrawString(Text, _TextFont, brush, x, y);
                m.Result = IntPtr.Zero;
                ReleaseDC(m.HWnd, hDC);
                }
            }
        }
}

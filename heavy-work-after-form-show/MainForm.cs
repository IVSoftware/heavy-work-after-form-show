using System.ComponentModel;
using System.Diagnostics;

namespace heavy_work_after_form_show
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Debug.Assert(Visible, "Expecting visiblee form");

            BackgroundWorker worker = new BackgroundWorker { WorkerReportsProgress = true };
            worker.DoWork += (sender, e) => mockHeavyWork(worker);
            worker.ProgressChanged += (sender, e) =>
            {
                if (!IsDisposed) // In case app ends before the task does.
                {
                    toolStripProgressBar.Value = e.ProgressPercentage;
                    if (e.ProgressPercentage >= 100)
                    {
                        statusStrip.Visible = false;
                    }
                }
            };
            worker.RunWorkerAsync();
        }

        void mockHeavyWork(BackgroundWorker worker)
        {
            for (int i = 0; i <= 100; i++)
            {
                Task.Delay(TimeSpan.FromSeconds(0.1)).Wait();
                worker.ReportProgress(i);
            }
        }
    }
}
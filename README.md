Since you are trying to **add some heavy work after the form is shown** one way to do that is with a [BackgroundWorker](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.backgroundworker?view=net-7.0) that you can start when the MainForm indicates that it has loaded (your for will be visible at this point).

Here's an example that mocks a 10-second heavy workload with a progress bar, while keeping the UI responsive at all times.

[![heavy work progress bar][1]][1]

___

```
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
```


  [1]: https://i.stack.imgur.com/EOZhv.png
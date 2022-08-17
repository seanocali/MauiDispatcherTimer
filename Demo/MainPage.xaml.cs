using MauiDispatcherTimer;

namespace Demo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += Timer_Tick;
            InitializeComponent();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tickLabel.Text = $"Tick at {DateTime.Now.ToString("O")}";
        }

        DispatcherTimer timer;

        private void StartButton_Clicked(object sender, EventArgs e)
        {
            headerLabel.Text = "Running, 1s Interval";
            timer.Start();
        }

        private void StopButton_Clicked(object sender, EventArgs e)
        {
            timer.Stop();
            headerLabel.Text = "Stopped";
            tickLabel.Text = "";
        }
    }
}
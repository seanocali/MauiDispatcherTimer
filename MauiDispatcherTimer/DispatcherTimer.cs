using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MauiDispatcherTimer
{
    /// <summary>
    /// Abstraction of MAUI's PeriodicTimer to mimic the DispatcherTimer 
    /// found in System.Windows.Threading and Windows.UI.Xaml.
    /// </summary>
    public class DispatcherTimer : IDispatcherTimer
    {
        PeriodicTimer _timer;
        CancellationTokenSource _cts;


        // Returns:
        //     The amount of time between ticks. The default is a **TimeSpan**
        TimeSpan _interval = new TimeSpan();
        public TimeSpan Interval
        {
            get => _interval;
            set
            {
                if (value != _interval)
                {
                    _interval = value;
                    if (_isEnabled)
                    {
                        Start();
                    }
                }
            }
        }

        //
        // Summary:
        //     Gets a value that indicates whether the timer is running.
        //
        // Returns:
        //     **true** if the timer is enabled and running; otherwise, **false**.
        bool _isEnabled;
        public bool IsEnabled => _isEnabled;
        public bool IsRunning => _isEnabled;


        bool _isRepeating = true;
        public bool IsRepeating
        {
            get => _isRepeating;
            set => _isRepeating = value;
        }

        //
        // Summary:
        //     Occurs when the timer interval has elapsed.
        public event EventHandler Tick;


        //
        // Summary:
        //     Starts the DispatcherTimer.
        public void Start()
        {
            Stop();
            var task = RunTimer();
        }

        //
        // Summary:
        //     Stops the DispatcherTimer.
        public void Stop()
        {
            if (_cts != null)
            {
                try
                {
                    _cts.Cancel();
                }
                catch (ObjectDisposedException ex) { Console.WriteLine(ex.Message); }
            }
        }


        async Task RunTimer()
        {
            _isEnabled = true;

            try
            {
                using (_cts = new CancellationTokenSource())
                using (var timer = new PeriodicTimer(_interval))
                {
                    var cancellationToken = _cts.Token;
                    while (await timer.WaitForNextTickAsync(cancellationToken))
                    {
                        var task = OnTick(_isRepeating, cancellationToken);
                    }
                }
            }
            catch (TaskCanceledException ex) { Console.WriteLine(ex.Message); }
            finally
            {
                _cts = null;
                _isEnabled = false;
            }
        }

        async Task OnTick(bool repeat, CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                Tick?.Invoke(this, null);
            }

            if (!repeat)
            {
                Stop();
            }
        }
    }
}
namespace Pomodoro.Library;

/// <summary>
/// Pomodoro Timer
/// </summary>
public class PomodoroTimer : ObservableBase
{
    private const double timer_interval_ms = 100;
    private const string display_format = "mm\\:ss";
    private readonly List<PomodoroItem> _items =
    [
        new(PomodoroType.TaskTimer,
        Color.FromArgb(255, 240, 58, 23),
        Color.FromArgb(255, 239, 105, 80)),
        new(PomodoroType.ShortBreak,
        Color.FromArgb(255, 131, 190, 236),
        Color.FromArgb(255, 179, 219, 212)),
        new(PomodoroType.LongBreak,
        Color.FromArgb(255, 186, 216, 10),
        Color.FromArgb(255, 228, 245, 119))
    ];

    private bool _isStarted;
    private string? _display;
    private PomodoroItem? _item;
    private PomodoroAlert? _alert;
    private DateTime _start;
    private DateTime _finish;
    private TimeSpan _current;
    private Timer? _timer;

    /// <summary>
    /// Added Event
    /// </summary>
    public event EventHandler<PomodoroAlertEventArgs>? Added;

    /// <summary>
    /// Removed Event
    /// </summary>
    public event EventHandler<PomodoroAlertEventArgs>? Removed;

    /// <summary>
    /// Triggered Event
    /// </summary>
    public event EventHandler<PomodoroAlertEventArgs>? Triggered;

    /// <summary>
    /// Updated Event
    /// </summary>
    public event EventHandler<PomodoroUpdateEventArgs>? Updated;

    /// <summary>
    /// Is Started?
    /// </summary>
    public bool IsStarted => _isStarted;

    /// <summary>
    /// Items
    /// </summary>
    public List<PomodoroItem> Items => _items;

    /// <summary>
    /// Display
    /// </summary>
    public string? Display
    {
        get => _display;
        set => SetProperty(ref _display, value);
    }

    /// <summary>
    /// Pomodoro Item
    /// </summary>
    public PomodoroItem? Item
    {
        get => _item;
        set => SetProperty(ref _item, value);
    }

    /// <summary>
    /// Get Display
    /// </summary>
    /// <param name="value">TimeSpan</param>
    /// <returns>Output</returns>
    private static string GetDisplay(TimeSpan value) =>
           value.ToString(display_format);

    /// <summary>
    /// Set
    /// </summary>
    /// <param name="item">Pomodoro Item</param>
    private void Set(PomodoroItem? item)
    {
        Item = item;
        _current = item!.TimeSpan;
        Display = GetDisplay(_current);
    }

    /// <summary>
    /// Tick
    /// </summary>
    private void Tick()
    {
        var display = GetDisplay(_current + (_start - DateTime.UtcNow));
        if (_isStarted && display != GetDisplay(TimeSpan.Zero))
            Updated?.Invoke(this, new PomodoroUpdateEventArgs(display));
        else
            Triggered?.Invoke(this, new PomodoroAlertEventArgs(_alert));
    }

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        if (_alert == null)
        {
            _start = DateTime.UtcNow;
            _finish = _start.Add(Item!.TimeSpan);
            _alert = new PomodoroAlert(_start, _finish, Item);
            Added?.Invoke(this, new PomodoroAlertEventArgs(_alert));
        }
        Set(Item);
        if (_timer == null)
        {
            _timer = new Timer()
            {
                Interval = timer_interval_ms
            };
            _timer.Elapsed += (object? sender, ElapsedEventArgs e) => Tick();
        }
        _timer.Start();
        _isStarted = true;
    }

    /// <summary>
    /// Stop
    /// </summary>
    private void Stop()
    {
        Removed?.Invoke(this, new PomodoroAlertEventArgs(_alert));
        Reset();
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="notification">Notification</param>
    public PomodoroTimer(PomodoroNotification? notification)
    {
        if (notification != null)
        {
            var item = Items.First(f => f.Id == notification.Id);
            _start = notification.Completed - item.TimeSpan;
            _finish = notification.Completed;
            _alert = new PomodoroAlert(_start, _finish, item);
            Set(item);
            Start();
        }
        else
            Set(Items.First());
    }

    /// <summary>
    /// Reset
    /// </summary>
    public void Reset()
    {
        _alert = null;
        _timer?.Stop();
        _isStarted = false;
        Set(Item);
    }

    /// <summary>
    /// Toggle Timer
    /// </summary>
    public void Toggle()
    {
        if (_isStarted)
            Stop();
        else
            Start();
    }

    /// <summary>
    /// Select
    /// </summary>
    /// <param name="item">Pomodoro Item</param>
    public void Select(PomodoroItem? item) => 
        Set(item);
}

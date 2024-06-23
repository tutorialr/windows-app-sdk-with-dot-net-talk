namespace Pomodoro.Library;

/// <summary>
/// Application Provider
/// </summary>
/// <param name="notification">Notification</param>
public class ApplicationProvider(PomodoroNotification? notification)
{
    private const string title = "Pomodoro";
    private const string toggle_label = "Toggle";
    private const string toggle_resource = "TimerClock";

    private readonly PomodoroTimer _timer = new(notification);
    private Dialog? _dialog;

    /// <summary>
    /// Get Resource Uri
    /// </summary>
    /// <param name="resource">Resource</param>
    /// <returns>Resource Uri</returns>
    private static Uri GetResourceUri(string resource, bool small = true) =>
        new($"ms-appx:///Assets/{resource}{(small ? "64" : string.Empty)}.png");

    /// <summary>
    /// Add
    /// </summary>
    /// <param name="commandBar">Command Bar</param>
    /// <param name="label">Label</param>
    /// <param name="resource">Resource</param>
    /// <param name="command">Command</param>
    /// <param name="parameter">Parameter</param>
    private static void Add(CommandBar commandBar, string label, string? resource, 
        ICommand command, object? parameter = null) =>
        commandBar.PrimaryCommands.Add(new AppBarButton()
        {
            Label = label,
            Command = command,
            CommandParameter = parameter,
            Icon = new BitmapIcon()
            {
                ShowAsMonochrome = false,
                UriSource = GetResourceUri(resource ?? nameof(PomodoroType.None))
            },
        });

    /// <summary>
    /// Show
    /// </summary>
    /// <param name="item">Pomodoro Item</param>
    /// <param name="messages">Messages</param>
    private async void Show(PomodoroItem? item, params string[] messages)
    {
        var content = new StackPanel()
        {
            Orientation = Orientation.Vertical
        };
        var resource = item?.Resource ?? nameof(PomodoroType.None);
        var image = new Image()
        {
            Width = 128,
            Height = 128,
            Margin = new Thickness(5),
            Source = new BitmapImage(GetResourceUri(resource, false))
        };
        content.Children.Add(image);
        foreach (var message in messages)
        {
            content.Children.Add(new TextBlock()
            {
                Text = message,
                HorizontalTextAlignment = TextAlignment.Center
            });
        }
        await _dialog!.ConfirmAsync(content);
    }

    /// <summary>
    /// Choose
    /// </summary>
    /// <param name="item">Pomodoro Item</param>
    private void Choose(PomodoroItem? item)
    {
        if (_timer.IsStarted)
            Show(_timer.Item, "To switch Timers you need to Toggle", $"{_timer.Item}");
        else
            _timer.Select(item);
    }

    /// <summary>
    /// Pomodoro Timer
    /// </summary>
    public PomodoroTimer Timer => 
        _timer;

    /// <summary>
    /// Triggered
    /// </summary>
    /// <param name="alert">Pomodoro Alert</param>
    public void Triggered(PomodoroAlert? alert)
    {
        Show(alert?.Item, "Timer Completed", $"{alert?.Item}", $"{alert}");
        _timer.Reset();
    }

    /// <summary>
    /// Loaded
    /// </summary>
    /// <param name="display">Grid</param>
    /// <param name="commandBar">Command Bar</param>>
    public void Loaded(Grid display, CommandBar commandBar)
    {
        _dialog = new Dialog(display.XamlRoot, title);
        Add(commandBar, toggle_label, toggle_resource,
               new ActionCommandHandler((param) => _timer.Toggle()));
        foreach (var item in _timer.Items)
            Add(commandBar, $"{item}", item.Resource,
                new ActionCommandHandler((param) => Choose(param as PomodoroItem)), item);
        display.DataContext = _timer;
    }

    /// <summary>
    /// Get Resource
    /// </summary>
    /// <param name="resource">Resource</param>
    /// <returns>Resource</returns>
    public static string GetResource(PomodoroItem? item) =>
        GetResourceUri(item?.Resource ?? nameof(PomodoroType.None)).ToString();
}

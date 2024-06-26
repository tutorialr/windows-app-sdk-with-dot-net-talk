// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pomodoro.Desktop;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    private readonly DispatcherQueue _dispatcherQueue;
    private readonly ApplicationProvider _applicationProvider;

    /// <summary>
    /// Widget
    /// </summary>
    /// <param name="display">Display</param>
    private static void Widget(string display)
    {
        var widgetIds = WidgetManager.GetDefault().GetWidgetIds();
        var widgetId = widgetIds?.FirstOrDefault();
        if (widgetId != null)
            WidgetProvider.UpdateWidget(widgetId, display);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public MainWindow()
    {
        _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        var notifier = ToastNotificationManager.CreateToastNotifier();
        var scheduled = notifier.GetScheduledToastNotifications().FirstOrDefault();
        var notification = scheduled != null ? 
            new PomodoroNotification(scheduled.Id, scheduled.DeliveryTime.UtcDateTime) : null;
        _applicationProvider = new ApplicationProvider(notification);
        _applicationProvider.Timer.Added += (object? sender, PomodoroAlertEventArgs e) =>
        {
            var xml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
            xml.GetElementsByTagName("image")[0].Attributes.GetNamedItem("src").InnerText = 
                ApplicationProvider.GetResource(e.Alert?.Item);
            xml.GetElementsByTagName("text")[0].InnerText = $"{e.Alert?.Item}";
            xml.GetElementsByTagName("text")[1].InnerText = $"{e.Alert}";
            var toast = new ScheduledToastNotification(xml, e.Alert!.Finish)
            {
                Id = e.Alert.Item.Id
            };
            notifier.AddToSchedule(toast);
        };
        _applicationProvider.Timer.Removed += (object? sender, PomodoroAlertEventArgs e) =>
        {
            foreach (var toast in notifier.GetScheduledToastNotifications())
                notifier.RemoveFromSchedule(toast);
            Widget(string.Empty);
        };
        _applicationProvider.Timer.Triggered += (object? sender, PomodoroAlertEventArgs e) =>
            _dispatcherQueue.TryEnqueue(() => _applicationProvider.Triggered(e.Alert));
        _applicationProvider.Timer.Updated += (object? sender, PomodoroUpdateEventArgs e) =>
            _dispatcherQueue.TryEnqueue(() => Update(e));
        InitializeComponent();
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="e">Pomodoro Update Event Args</param>
    private void Update(PomodoroUpdateEventArgs e)
    {
        Widget(e.Display ?? string.Empty);
        _applicationProvider.Timer.Display = e.Display;
    }

    /// <summary>
    /// Loaded
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Routed Event Args</param>
    private void Loaded(object sender, RoutedEventArgs e) =>
        _applicationProvider.Loaded(Display, Command);
}

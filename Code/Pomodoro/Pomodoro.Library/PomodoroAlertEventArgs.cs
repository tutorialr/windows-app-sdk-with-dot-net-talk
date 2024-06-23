namespace Pomodoro.Library;

/// <summary>
/// Alert Event Args
/// </summary>
/// <param name="alert">Alert</param>
public class PomodoroAlertEventArgs(PomodoroAlert? alert) : EventArgs
{
    /// <summary>
    /// Alert
    /// </summary>
    public PomodoroAlert? Alert { get; } = alert;
}

namespace Pomodoro.Library;

/// <summary>
/// Alert Event Args
/// </summary>
/// <param name="display">Display</param>
public class PomodoroUpdateEventArgs(string? display) : EventArgs
{
    /// <summary>
    /// Display
    /// </summary>
    public string? Display { get; } = display;
}

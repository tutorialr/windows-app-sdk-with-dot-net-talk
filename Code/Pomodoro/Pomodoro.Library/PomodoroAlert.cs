namespace Pomodoro.Library;

/// <summary>
/// Pomodoro Alert
/// </summary>
/// <param name="start">Start Time</param>
/// <param name="finish">Finish Time</param>
/// <param name="item">Pomodoro Item</param>
public class PomodoroAlert(DateTime start, DateTime finish, PomodoroItem item)
{
    /// <summary>
    /// Start
    /// </summary>
    public DateTime Start { get; set; } = start;

    /// <summary>
    /// Finish
    /// </summary>
    public DateTime Finish { get; set; } = finish;

    /// <summary>
    /// Pomodoro Item
    /// </summary>
    public PomodoroItem Item { get; set; } = item;

    /// <summary>
    /// To String
    /// </summary>
    /// <returns>String</returns>
    public override string ToString() =>
        $"Started {Start:HH:mm} Finished {Finish:HH:mm}";
}

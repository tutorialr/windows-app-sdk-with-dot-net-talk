namespace Pomodoro.Library;

/// <summary>
/// Pomodoro Notification
/// </summary>
/// <param name="id">Id</param>
/// <param name="completed">Completed</param>
public class PomodoroNotification(string id, DateTime completed)
{
    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; } = id;

    /// <summary>
    /// Completed
    /// </summary>
    public DateTime Completed { get; } = completed;
}

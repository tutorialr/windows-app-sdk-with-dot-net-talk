namespace Pomodoro.Library;

/// <summary>
/// Pomodoro Type
/// </summary>
public enum PomodoroType
{
    [Description("TimerClock")]
    None = 0,
    [Description("Tomato")]
    TaskTimer = 25,
    [Description("HotBeverage")]
    ShortBreak = 5,
    [Description("GreenApple")]
    LongBreak = 20
}

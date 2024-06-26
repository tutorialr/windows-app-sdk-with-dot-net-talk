namespace Pomodoro.Library;

/// <summary>
/// Pomodoro Item
/// </summary>
public partial class PomodoroItem : ObservableBase
{
    private const string space = " ";
    [GeneratedRegex(@"\p{Lu}\p{Ll}*")]
    private static partial Regex regex();

    private PomodoroType _type;
    private Color _upper;
    private Color _lower;

    /// <summary>
    /// Pomodoro Type
    /// </summary>
    public PomodoroType Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }

    /// <summary>
    /// Upper Colour
    /// </summary>
    public Color Upper
    {
        get => _upper;
        set => SetProperty(ref _upper, value);
    }

    /// <summary>
    /// Lower Colour
    /// </summary>
    public Color Lower
    {
        get => _lower;
        set => SetProperty(ref _lower, value);
    }

    /// <summary>
    /// Id
    /// </summary>
    public string Id =>
       Enum.GetName(typeof(PomodoroType), Type) ?? nameof(PomodoroType.None);

    /// <summary>
    /// TimeSpan
    /// </summary>
    public TimeSpan TimeSpan =>
        TimeSpan.FromMinutes((double)Type);

    /// <summary>
    /// Resource
    /// </summary>
    public string? Resource =>
        (Type.GetType()
        .GetField(Type.ToString())
        ?.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .FirstOrDefault() as DescriptionAttribute)
                ?.Description;

    /// <summary>
    /// To String
    /// </summary>
    /// <returns>String</returns>
    public override string ToString() =>
        string.Join(space, regex().Matches(Id)
            .Cast<Match>().Select(s => s.Value));

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="type">Pomodoro Type</param>
    /// <param name="upper">Upper Colour</param>
    /// <param name="lower">Lower Colour</param>
    public PomodoroItem(PomodoroType type, Color upper, Color lower) =>
        (Type, Upper, Lower) = (type, upper, lower);
}

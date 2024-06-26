namespace Pomodoro.Desktop.Widget;

/// <summary>
/// Pomodoro Widget
/// </summary>
/// <param name="widgetId">Widget Id</param>
/// <param name="startingState">Starting State</param>
internal class PomodoroWidget : WidgetBase
{
    private const string default_display = "00:00";
    private static string _widgetTemplate = string.Empty;

    /// <summary>
    /// Definition Id
    /// </summary>
    public static string DefinitionId { get; } = nameof(PomodoroWidget);

    /// <summary>
    /// Pomodoro Widget
    /// </summary>
    /// <param name="widgetId">Widget Id</param>
    /// <param name="startingState">Starting State</param>
    public PomodoroWidget(string widgetId, string startingState) : base(widgetId, startingState)
    {
        if (string.IsNullOrEmpty(State))
            state = default_display;
        else
            Display = startingState;
    }

    /// <summary>
    /// Activate
    /// </summary>
    /// <param name="widgetContext">Widget Context</param>
    public override void Activate(WidgetContext widgetContext) => 
        isActivated = true;

    /// <summary>
    /// Deactivate
    /// </summary>
    public override void Deactivate() => 
        isActivated = false;

    /// <summary>
    /// Get Template for Widget
    /// </summary>
    /// <returns>Widget Template</returns>
    public override string GetTemplateForWidget()
    {
        if (string.IsNullOrEmpty(_widgetTemplate))
            _widgetTemplate = ReadTemplateFromPackage("ms-appx:///Templates/PomodoroWidgetTemplate.json");
        return _widgetTemplate;
    }

    /// <summary>
    /// Get Data for Widget
    /// </summary>
    /// <returns>Widget Data</returns>
    public override string GetDataForWidget()
    {
        var state = new JsonObject
        {
            ["display"] = string.IsNullOrEmpty(State) ? default_display : State
        };
        return state.ToJsonString();
    }

    /// <summary>
    /// Display
    /// </summary>
    private string Display { get; set; } = default_display;
}

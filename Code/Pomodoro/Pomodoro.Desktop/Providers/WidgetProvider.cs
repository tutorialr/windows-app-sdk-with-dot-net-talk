namespace Pomodoro.Desktop.Providers;

/// <summary>
/// Widget Provider
/// </summary>
/// <remarks>
/// Copyright (C) Microsoft Corporation. Licensed under the MIT License
/// </remarks>
[ComVisible(true)]
[ComDefaultInterface(typeof(IWidgetProvider))]
[Guid("34dd0cbf-2867-4c3f-9c45-8b8c0a36231e")]
public sealed class WidgetProvider : IWidgetProvider
{
    private static bool _haveRecoveredWidgets = false;
    private static readonly Dictionary<string, WidgetBase> _widgetInstances = [];
    public static readonly Dictionary<string, WidgetCreateDelegate> _widgetImplementations = [];

    /// <summary>
    /// Recover Running Widgets
    /// </summary>
    private static void RecoverRunningWidgets()
    {
        if(!_haveRecoveredWidgets)
        {
            try
            {
                var widgetManager = WidgetManager.GetDefault();
                if (widgetManager?.GetWidgetInfos() != null)
                {
                    foreach (var widgetInfo in widgetManager.GetWidgetInfos())
                    {
                        var widgetContext = widgetInfo.WidgetContext;
                        if (!_widgetInstances.ContainsKey(widgetContext.Id))
                        {
                            if (_widgetImplementations.TryGetValue(widgetContext.DefinitionId, out WidgetCreateDelegate? value))
                                _widgetInstances[widgetContext.Id] = value(widgetContext.Id, widgetInfo.CustomState);
                            else
                                widgetManager.DeleteWidget(widgetContext.Id);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Ignore Exception
            }
            finally
            {
                _haveRecoveredWidgets = true;
            }
        }
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public WidgetProvider() => 
        RecoverRunningWidgets();

    /// <summary>
    /// Activate Widget
    /// </summary>
    /// <param name="widgetContext">Widget Context</param>
    public void Activate(WidgetContext widgetContext)
    {
        if (!_widgetInstances.ContainsKey(widgetContext.Id))
            throw new Exception($"Activate called for unknown widget");
    }

    /// <summary>
    /// Create Widget
    /// </summary>
    /// <param name="widgetContext">Widget Context</param>
    public void CreateWidget(WidgetContext widgetContext)
    {
        if (!_widgetImplementations.TryGetValue(widgetContext.DefinitionId, out WidgetCreateDelegate? widgetCreateDelegate))
            throw new Exception($"Invalid widget definition requested: {widgetContext.DefinitionId}");

        var widgetInstance = widgetCreateDelegate(widgetContext.Id, string.Empty);
        _widgetInstances[widgetContext.Id] = widgetInstance;

        var widgetOptions = new WidgetUpdateRequestOptions(widgetContext.Id)
        {
            Template = widgetInstance.GetTemplateForWidget(),
            Data = widgetInstance.GetDataForWidget(),
            CustomState = widgetInstance.State
        };
        WidgetManager.GetDefault().UpdateWidget(widgetOptions);
    }

    /// <summary>
    /// Get Widget Data
    /// </summary>
    /// <param name="state">State</param>
    /// <returns>Widget Data</returns>
    public static string GetWidgetData(string state) => 
        state;

    /// <summary>
    /// Deactivate Widget
    /// </summary>
    /// <param name="widgetId">Widget Id</param>
    public void Deactivate(string widgetId) => 
        _widgetInstances[widgetId].Deactivate();

    /// <summary>
    /// Delete Widget
    /// </summary>
    /// <param name="widgetId">Widget Id</param>
    /// <param name="customState">Custom State</param>
    public void DeleteWidget(string widgetId, string customState) =>
        _widgetInstances.Remove(widgetId);

    /// <summary>
    /// Widget On Action Invoked
    /// </summary>
    /// <param name="actionInvokedArgs">Action Invoked Args</param>
    public void OnActionInvoked(WidgetActionInvokedArgs actionInvokedArgs) => 
        _widgetInstances[actionInvokedArgs.WidgetContext.Id].OnActionInvoked(actionInvokedArgs);

    /// <summary>
    /// On Widget Context Changed
    /// </summary>
    /// <param name="contextChangedArgs">Context Changed Args</param>
    public void OnWidgetContextChanged(WidgetContextChangedArgs contextChangedArgs) => 
        _widgetInstances[contextChangedArgs.WidgetContext.Id].OnWidgetContextChanged(contextChangedArgs);

    /// <summary>
    /// Update Widget
    /// </summary>
    /// <param name="widgetId">Widget Id</param>
    /// <param name="customState">Custom State</param>
    /// <returns>True if Updated, False if Not</returns>
    public static bool UpdateWidget(string widgetId, string customState)
    {
        RecoverRunningWidgets();
        if (!_widgetInstances.TryGetValue(widgetId, out WidgetBase? value))
            return false;
        var widgetInstance = value;
        widgetInstance.SetState(customState);
        var widgetOptions = new WidgetUpdateRequestOptions(widgetId)
        {
            Template = widgetInstance.GetTemplateForWidget(),
            Data = widgetInstance.GetDataForWidget(),
            CustomState = widgetInstance.State
        };
        WidgetManager.GetDefault().UpdateWidget(widgetOptions);
        return true;
    }

    /// <summary>
    /// Add Widget
    /// </summary>
    /// <param name="widgetId">Widget Id</param>
    /// <param name="widgetCreateDelegate">Widget Create Delegate</param>
    public static void AddWidget(string widgetId, WidgetCreateDelegate widgetCreateDelegate) =>
        _widgetImplementations.Add(widgetId, widgetCreateDelegate);

    /// <summary>
    /// Clear Widgets
    /// </summary>
    public static void ClearWidgets() => 
        _widgetImplementations.Clear();
}

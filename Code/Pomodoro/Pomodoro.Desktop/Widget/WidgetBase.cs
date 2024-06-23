namespace Pomodoro.Desktop.Widget;

/// <summary>
/// Widget Create Delegate
/// </summary>
/// <remarks>
/// Copyright (C) Microsoft Corporation. Licensed under the MIT License.
/// </remarks>
/// <param name="widgetId">Widget Id</param>
/// <param name="initialState">Initial State</param>
/// <returns>Widget Base</returns>
public delegate WidgetBase WidgetCreateDelegate(string widgetId, string initialState);

/// <summary>
/// Widget Base
/// </summary>
/// <param name="widgetId">Widget Id</param>
/// <param name="startingState">Starting State</param>
public abstract class WidgetBase(string widgetId, string initialState)
{
    protected string id = widgetId;
    protected string state = initialState;
    protected bool isActivated = false;

    /// <summary>
    /// Id
    /// </summary>
    public string Id { get => id; }

    /// <summary>
    /// State
    /// </summary>
    public string State { get => state; }

    /// <summary>
    /// Is Activated?
    /// </summary>
    public bool IsActivated { get => isActivated; }

    /// <summary>
    /// Set State
    /// </summary>
    /// <param name="state">State</param>
    public void SetState(string state) =>
        this.state = state;

    /// <summary>
    /// Read Template from Package
    /// </summary>
    /// <param name="packageUri">Package Uri</param>
    /// <returns>Widget Template</returns>
    protected static string ReadTemplateFromPackage(string packageUri)
    {
        var uri = new Uri(packageUri);
        var storageFileTask = StorageFile.GetFileFromApplicationUriAsync(uri).AsTask();
        storageFileTask.Wait();
        var readTextTask = FileIO.ReadTextAsync(storageFileTask.Result).AsTask();
        readTextTask.Wait();
        return readTextTask.Result;
    }

    /// <summary>
    /// Activate Widget
    /// </summary>
    /// <param name="widgetContext">Widget </param>
    public virtual void Activate(WidgetContext widgetContext) { }

    /// <summary>
    /// Deactivate Widget
    /// </summary>
    public virtual void Deactivate() { }

    /// <summary>
    /// Widget On Action Invoked
    /// </summary>
    /// <param name="actionInvokedArgs">Widget Action Invoked Args</param>
    public virtual void OnActionInvoked(WidgetActionInvokedArgs actionInvokedArgs) { }

    /// <summary>
    /// On Widget Context Changed
    /// </summary>
    /// <param name="contextChangedArgs"></param>
    public virtual void OnWidgetContextChanged(WidgetContextChangedArgs contextChangedArgs) { }

    /// <summary>
    /// Get Template for Widget
    /// </summary>
    /// <returns>Widget Template</returns>
    public abstract string GetTemplateForWidget();

    /// <summary>
    /// Get Data for Widget
    /// </summary>
    /// <returns>Widget Data</returns>
    public abstract string GetDataForWidget();
}

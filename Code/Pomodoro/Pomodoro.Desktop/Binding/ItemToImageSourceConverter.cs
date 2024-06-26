namespace Pomodoro.Desktop.Binding;

/// <summary>
/// Pomodoro Item to Image Source Converter
/// </summary>
public class ItemToImageSourceConverter : IValueConverter
{
    /// <summary>
    /// Convert
    /// </summary>
    /// <param name="value">Source</param>
    /// <param name="targetType">Target Type</param>
    /// <param name="parameter">Parameter</param>
    /// <param name="language">Language</param>
    /// <returns>Target</returns>
    public object Convert(object value, Type targetType, object parameter, string language) =>
        value is PomodoroItem item ? new BitmapImage(new Uri($"ms-appx:///Assets/{item.Resource}.png")) : new();

    /// <summary>
    /// Convert Back
    /// </summary>
    /// <param name="value">Source</param>
    /// <param name="targetType">Target Type</param>
    /// <param name="parameter">Parameter</param>
    /// <param name="language">Language</param>
    /// <returns>Target</returns>
    /// <exception cref="NotImplementedException"></exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language) =>
        throw new NotImplementedException();
}

﻿namespace Pomodoro.Desktop.Binding;

/// <summary>
/// Drawing To Windows Color Converter
/// </summary>
public class DrawingToWindowsColorConverter : IValueConverter
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
        value is System.Drawing.Color item ? Windows.UI.Color.FromArgb(item.A, item.R, item.G, item.B) : Colors.Transparent;

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
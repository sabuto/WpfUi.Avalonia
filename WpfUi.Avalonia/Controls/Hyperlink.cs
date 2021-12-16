using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Interactivity;

namespace WpfUi.Avalonia.Controls;

/// <summary>
///     A Button that opens a url in a web browser
/// </summary>
public class Hyperlink : global::Avalonia.Controls.Button
{
    /// <summary>
    ///     Property for <see cref="NavigateUri" />.
    /// </summary>
    public static readonly StyledProperty<string> NavigateUriProperty =
        AvaloniaProperty.Register<Hyperlink, string>(nameof(NavigateUri), string.Empty);

    /// <summary>
    ///     Registering the event in the constructor
    ///     Action triggered when the button is clicked.
    /// </summary>
    public Hyperlink()
    {
        Click += RequestNavigate;
    }

    /// <summary>
    ///     The URL (or application shortcut) to open.
    /// </summary>
    public string NavigateUri
    {
        get => GetValue(NavigateUriProperty);
        set => SetValue(NavigateUriProperty, value);
    }

    /// <summary>
    ///     Handles the execution of the click.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RequestNavigate(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(NavigateUri)) return;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            ProcessStartInfo processStartInfo = new(NavigateUri)
            {
                UseShellExecute = true
            };
            Process.Start(processStartInfo);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            Process.Start("xdg-open", NavigateUri);
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            Process.Start("open", NavigateUri);
        }
    }
}
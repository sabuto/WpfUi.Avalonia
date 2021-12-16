using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Themes.Fluent;

namespace WpfUi.Avalonia;

public enum WpfUiThemeMode
{
    Light,
    Dark
}

/// <summary>
///     Includes the WpfUi theme in an application.
/// </summary>
public class WpfUiTheme : IStyle, IResourceProvider
{
    private readonly Uri _baseUri;
    private bool _isLoading;
    private IStyle[]? _loaded;

    /// <summary>
    ///     Initializes a new instance of the <see cref="FluentTheme" /> class.
    /// </summary>
    /// <param name="baseUri">The base URL for the XAML context.</param>
    public WpfUiTheme(Uri baseUri)
    {
        _baseUri = baseUri;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FluentTheme" /> class.
    /// </summary>
    /// <param name="serviceProvider">The XAML service provider.</param>
    public WpfUiTheme(IServiceProvider serviceProvider)
    {
        _baseUri = ((IUriContext)serviceProvider.GetService(typeof(IUriContext))!).BaseUri;
    }

    /// <summary>
    ///     Gets or sets the mode of the fluent theme (light, dark).
    /// </summary>
    public WpfUiThemeMode Mode { get; set; }

    /// <summary>
    ///     Gets the loaded style.
    /// </summary>
    public IStyle Loaded
    {
        get
        {
            if (_loaded == null)
            {
                _isLoading = true;
                IStyle loaded = (IStyle)AvaloniaXamlLoader.Load(GetUri(), _baseUri);
                _loaded = new[] { loaded };
                _isLoading = false;
            }

            return _loaded?[0]!;
        }
    }

    public IResourceHost? Owner => (Loaded as IResourceProvider)?.Owner;

    bool IResourceNode.HasResources => (Loaded as IResourceProvider)?.HasResources ?? false;

    public event EventHandler OwnerChanged
    {
        add
        {
            if (Loaded is IResourceProvider rp) rp.OwnerChanged += value;
        }
        remove
        {
            if (Loaded is IResourceProvider rp) rp.OwnerChanged -= value;
        }
    }

    public bool TryGetResource(object key, out object? value)
    {
        if (!_isLoading && Loaded is IResourceProvider p) return p.TryGetResource(key, out value);

        value = null;
        return false;
    }

    void IResourceProvider.AddOwner(IResourceHost owner)
    {
        (Loaded as IResourceProvider)?.AddOwner(owner);
    }

    void IResourceProvider.RemoveOwner(IResourceHost owner)
    {
        (Loaded as IResourceProvider)?.RemoveOwner(owner);
    }

    IReadOnlyList<IStyle> IStyle.Children => _loaded ?? Array.Empty<IStyle>();

    public SelectorMatchResult TryAttach(IStyleable target, IStyleHost? host)
    {
        return Loaded.TryAttach(target, host);
    }

    private Uri GetUri()
    {
        return Mode switch
        {
            WpfUiThemeMode.Dark => new Uri("avares://WpfUi.Avalonia/WpfUiDark.axaml", UriKind.Absolute),
            _ => new Uri("avares://WpfUi.Avalonia/WpfUiLight.axaml", UriKind.Absolute)
        };
    }
}
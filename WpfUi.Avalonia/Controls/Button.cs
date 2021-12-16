using Avalonia;
using WpfUi.Avalonia.Core.Common;

namespace WpfUi.Avalonia.Controls;

/// <summary>
///     Inherited from the <see cref="Avalonia.Controls.Button" />, adding <see cref="Common.Icon" />.
/// </summary>
public class Button : global::Avalonia.Controls.Button
{
    /// <summary>
    ///     Property for <see cref="Glyph" />.
    /// </summary>
    public static readonly StyledProperty<Core.Common.Icon> GlyphProperty =
        AvaloniaProperty.Register<Button, Core.Common.Icon>(nameof(Glyph));

    /// <summary>
    ///     Property for <see cref="Appearance" />.
    /// </summary>
    public static readonly StyledProperty<Appearance> AppearanceProperty =
        AvaloniaProperty.Register<Button, Appearance>(nameof(Appearance));

    /// <summary>
    ///     Property for <see cref="IsGlyph" />.
    /// </summary>
    public static readonly StyledProperty<bool> IsGlyphProperty =
        AvaloniaProperty.Register<Button, bool>(nameof(IsGlyph));

    public static readonly StyledProperty<string> RawGlyphProperty =
        AvaloniaProperty.Register<Button, string>(nameof(RawGlyph));

    static Button()
    {
        GlyphProperty.Changed.AddClassHandler<Button>(GlyphChanged);
    }


    /// <summary>
    ///     Gets or sets displayed <see cref="Icon" />.
    /// </summary>
    public Core.Common.Icon Glyph
    {
        get => GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    /// <summary>
    ///     Gets or sets the <see cref="Core.Common.Appearance" /> of the control, if available.
    /// </summary>
    public Appearance Appearance
    {
        get => GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }

    /// <summary>
    ///     Gets information whether the <see cref="Glyph" /> is set.
    /// </summary>
    public bool IsGlyph
    {
        get => GetValue(IsGlyphProperty);
        internal set => SetValue(IsGlyphProperty, value);
    }

    private string RawGlyph => GetValue(RawGlyphProperty);

    private static void GlyphChanged(Button x, AvaloniaPropertyChangedEventArgs e)
    {
        x.SetValue(IsGlyphProperty, true);
        x.SetValue(RawGlyphProperty, Core.Common.Glyph.ToString(x.Glyph));
    }
}
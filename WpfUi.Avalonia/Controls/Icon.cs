using Avalonia;
using Avalonia.Controls;
using WpfUi.Avalonia.Core.Common;

namespace WpfUi.Avalonia.Controls;

public class Icon : Label
{
    /// <summary>
    ///     Property for <see cref="Glyph" />.
    /// </summary>
    public static readonly StyledProperty<Core.Common.Icon> GlyphProperty =
        AvaloniaProperty.Register<Button, Core.Common.Icon>(nameof(Glyph));

    /// <summary>
    ///     Property for <see cref="Filled" />.
    /// </summary>
    public static readonly StyledProperty<bool> FilledProperty =
        AvaloniaProperty.Register<Button, bool>(nameof(Filled));

    public static readonly StyledProperty<string> RawGlyphProperty =
        AvaloniaProperty.Register<Button, string>(nameof(RawGlyph));

    static Icon()
    {
        GlyphProperty.Changed.AddClassHandler<Icon>(GlyphChanged);
    }

    /// <summary>
    ///     Gets or sets displayed <see cref="Core.Common.Icon" />.
    /// </summary>
    public Core.Common.Icon Glyph
    {
        get => GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    /// <summary>
    ///     Gets or sets displayed <see cref="Core.Common.Icon" /> as <see langword="string" />.
    /// </summary>
    public string RawGlyph => GetValue(RawGlyphProperty);

    /// <summary>
    ///     Defines whether or not we should use the <see cref="IconFilled" />.
    /// </summary>
    public bool Filled
    {
        get => GetValue(FilledProperty);
        set => SetValue(FilledProperty, value);
    }

    private static void GlyphChanged(Icon icon, AvaloniaPropertyChangedEventArgs avaloniaPropertyChangedEventArgs)
    {
        if (icon.GetValue(FilledProperty))
            icon.SetValue(RawGlyphProperty, Core.Common.Glyph.ToString(Core.Common.Glyph.Swap(icon.Glyph) as Core.Common.Icon?));
        else
            icon.SetValue(RawGlyphProperty, Core.Common.Glyph.ToString(icon.Glyph));
    }
}
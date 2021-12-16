using System;

namespace WpfUi.Avalonia.Core.Common;

public class Glyph
{
    private const Icon DefaultIcon = Icon.Heart28;
    private static readonly IconFilled _defaultFilledIcon = IconFilled.Heart28;

    /// <summary>
    ///     Converts <see cref="Icon" /> to <see langword="char" /> based on the ID, if <see langword="null" /> or error,
    ///     returns <see cref="Glyph.Play16" />
    /// </summary>
    public static char ToGlyph(Icon? icon)
    {
        if (icon == null)
            return ToChar(DefaultIcon);

        return ToChar(icon);
    }

    /// <summary>
    ///     Converts <see cref="Icon" /> to <see langword="string" /> based on the ID, if <see langword="null" /> or error,
    ///     returns <see cref="Glyph.Play16" />
    /// </summary>
    public static string ToString(Icon? icon)
    {
        return ToGlyph(icon).ToString();
    }

    public static Icon Parse(string name)
    {
        return (Icon)Enum.Parse(typeof(Icon), name);
    }

    private static char ToChar(Icon? icon)
    {
        if (icon == null)
            icon = DefaultIcon;

        return Convert.ToChar(icon);
    }

    /// <summary>
    ///     Replaces <see cref="Icon" /> with <see cref="IconFilled" />.
    /// </summary>
    /// <param name="icon"></param>
    /// <returns></returns>
    public static IconFilled Swap(Icon icon)
    {
        return ParseFilled(icon.ToString());
    }

    /// <summary>
    ///     Finds icon based on name.
    /// </summary>
    /// <param name="name">Name of the icon.</param>
    /// <returns></returns>
    public static IconFilled ParseFilled(string name)
    {
        if (string.IsNullOrEmpty(name)) return _defaultFilledIcon;

        return (IconFilled)Enum.Parse(typeof(IconFilled), name);
    }
}
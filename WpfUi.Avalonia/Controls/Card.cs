using Avalonia;
using Avalonia.Controls;

namespace WpfUi.Avalonia.Controls;

public class Card : ContentControl
{
    public static readonly StyledProperty<CornerRadius> CornerRadiusProperty =
        AvaloniaProperty.Register<Card, CornerRadius>(nameof(CornerRadius), new CornerRadius(4));

    public static readonly StyledProperty<bool> InsideClippingProperty =
        AvaloniaProperty.Register<Card, bool>(nameof(InsideClipping), true);

    /// <summary>
    ///     Gets or sets the radius of the border rounded corners.
    /// </summary>
    public CornerRadius CornerRadius
    {
        get => GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    ///     Get or set the inside border clipping.
    /// </summary>
    public bool InsideClipping
    {
        get => GetValue(InsideClippingProperty);
        set => SetValue(InsideClippingProperty, value);
    }
}
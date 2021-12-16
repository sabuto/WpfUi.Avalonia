// This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
// If a copy of the MPL was not distributed with this file, You can obtain one at http://mozilla.org/MPL/2.0/.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// Copyright (C) Robert Dunne (Sabuto, Sacrementus) and WPF Ui.Avalonia Contributors.
// All Rights Reserved.

using System.Globalization;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace WpfUi.Avalonia.Controls;

public class NumberBox : TextBox
{
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<NumberBox, double>(nameof(Value));

    public static readonly StyledProperty<double> StepProperty =
        AvaloniaProperty.Register<NumberBox, double>(nameof(Step), 1.0);

    public static readonly StyledProperty<double> MaxProperty =
        AvaloniaProperty.Register<NumberBox, double>(nameof(Max), double.MaxValue);

    public static readonly StyledProperty<double> MinProperty =
        AvaloniaProperty.Register<NumberBox, double>(nameof(Min), double.MinValue);

    public static readonly StyledProperty<int> DecimalPlacesProperty =
        AvaloniaProperty.Register<NumberBox, int>(nameof(DecimalPlaces), 2);

    public static readonly StyledProperty<string> MaskProperty =
        AvaloniaProperty.Register<NumberBox, string>(nameof(Mask), string.Empty);

    public static readonly StyledProperty<string> PlaceholderProperty =
        AvaloniaProperty.Register<NumberBox, string>(nameof(Placeholder), string.Empty);

    public static readonly StyledProperty<bool> PlaceholderVisibleProperty =
        AvaloniaProperty.Register<NumberBox, bool>(nameof(PlaceholderVisible), true);

    public static readonly StyledProperty<bool> ControlsVisibleProperty =
        AvaloniaProperty.Register<NumberBox, bool>(nameof(ControlsVisible), true);

    public static readonly StyledProperty<bool> IntegersOnlyProperty =
        AvaloniaProperty.Register<NumberBox, bool>(nameof(IntegersOnly));

    private static Regex _patternRegex;

    static NumberBox()
    {
        // _patternRegex = IntegersOnly ? new Regex("[^0-9]+") : new Regex("[^0-9.,]+");
        ValueProperty.Changed.AddClassHandler<NumberBox>(ValueChanged);
        DecimalPlacesProperty.Changed.AddClassHandler<NumberBox>(DecimalPlacesChanged);
        IntegersOnlyProperty.Changed.AddClassHandler<NumberBox>(IntegersOnlyChanged);
    }

    // TODO: button command

    /// <summary>
    ///     Current numeric value.
    /// </summary>
    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    ///     Gets or sets value by which the given number will be increased or decreased after pressing the button.
    /// </summary>
    public double Step
    {
        get => GetValue(StepProperty);
        set => SetValue(StepProperty, value);
    }

    /// <summary>
    ///     Maximum allowable value.
    /// </summary>
    public double Max
    {
        get => GetValue(MaxProperty);
        set => SetValue(MaxProperty, value);
    }

    /// <summary>
    ///     Minimum allowable value.
    /// </summary>
    public double Min
    {
        get => GetValue(MinProperty);
        set => SetValue(MinProperty, value);
    }

    /// <summary>
    ///     Number of decimal places.
    /// </summary>
    public int DecimalPlaces
    {
        get => GetValue(DecimalPlacesProperty);
        set => SetValue(DecimalPlacesProperty, value);
    }

    /// <summary>
    ///     Gets or sets numbers pattern.
    /// </summary>
    public string Mask
    {
        get => GetValue(MaskProperty);
        set => SetValue(MaskProperty, value);
    }

    /// <summary>
    ///     Gets or sets numbers pattern.
    /// </summary>
    public string Placeholder
    {
        get => GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    /// <summary>
    ///     Gets or sets value determining whether to display the placeholder.
    /// </summary>
    public bool PlaceholderVisible
    {
        get => GetValue(PlaceholderVisibleProperty);
        set => SetValue(PlaceholderVisibleProperty, value);
    }

    /// <summary>
    ///     Gets or sets value determining whether to display the button controls.
    /// </summary>
    public bool ControlsVisible
    {
        get => GetValue(ControlsVisibleProperty);
        set => SetValue(ControlsVisibleProperty, value);
    }

    /// <summary>
    ///     Gets or sets value which determines whether only integers can be entered.
    /// </summary>
    public bool IntegersOnly
    {
        get => GetValue(IntegersOnlyProperty);
        set => SetValue(IntegersOnlyProperty, value);
    }

    private static void IntegersOnlyChanged(NumberBox s, AvaloniaPropertyChangedEventArgs e)
    {
        s._patternRegex = s.IntegersOnly ? new Regex("[^0-9]+") : new Regex("[^0-9.,]+");
    }

    private static void DecimalPlacesChanged(NumberBox s, AvaloniaPropertyChangedEventArgs e)
    {
        if (s.DecimalPlaces < 0)
            s.DecimalPlaces = 0;

        if (s.DecimalPlaces > 4)
            s.DecimalPlaces = 4;
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Up:
                IncrementValue();
                break;
            case Key.Down:
                DecrementValue();
                break;
        }
    }

    protected override void OnTextInput(TextInputEventArgs e)
    {
        if (PlaceholderVisible && e.Text.Length > 0)
            PlaceholderVisible = false;

        if (!PlaceholderVisible && e.Text.Length < 1)
            PlaceholderVisible = true;

        double.TryParse(Text, out double number);

        Value = number;

        e.Handled = Validate(e.Text);
    }

    private bool Validate(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return true;

        if (input.StartsWith(".") || input.EndsWith(".") || input.StartsWith(",") || input.EndsWith(","))
            return false;

        if (!_patternRegex.IsMatch(input))
            return false;

        return true;
    }

    private void DecrementValue()
    {
        string currentText = Text;

        if (string.IsNullOrEmpty(currentText))
        {
            Text = "-" + Step.ToString("F0");
            return;
        }

        double.TryParse(currentText, out double number);

        if (number - Step < Min)
            return;

        if ((currentText.Contains(".") || currentText.Contains(",")) && !IntegersOnly)
            Text = (number - Step).ToString("F" + DecimalPlaces, CultureInfo.InvariantCulture);
        else
            Text = (number - Step).ToString("F0", CultureInfo.InvariantCulture);
    }

    private void IncrementValue()
    {
        string currentText = Text;

        if (string.IsNullOrEmpty(currentText))
        {
            Text = Step.ToString("F0");
            return;
        }

        double.TryParse(currentText, out double number);

        if (number + Step > Max)
            return;

        if ((currentText.Contains(".") || currentText.Contains(",")) && !IntegersOnly)
            Text = (number + Step).ToString("F" + DecimalPlaces, CultureInfo.InvariantCulture);
        else
            Text = (number + Step).ToString("F0", CultureInfo.InvariantCulture);
    }

    private static void ValueChanged(NumberBox s, AvaloniaPropertyChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(s.Text))
            return;

        if (s.Value % 1 != 0 && !s.IntegersOnly)
            s.Text = s.Value.ToString("F" + s.DecimalPlaces, CultureInfo.InvariantCulture);
        else
            s.Text = s.Value.ToString("F0", CultureInfo.InvariantCulture);
    }
}
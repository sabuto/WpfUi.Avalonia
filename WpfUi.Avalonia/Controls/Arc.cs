using System;
using System.Globalization;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Threading;

namespace WpfUi.Avalonia.Controls;

// Taken from https://github.com/AvaloniaCommunity/Material.Avalonia/blob/dev/Material.Styles/Controls/Arc.cs
public class Arc : Control
{
    public static readonly StyledProperty<IBrush> ArcBrushProperty =
        AvaloniaProperty.Register<Arc, IBrush>(nameof(ArcBrush), new SolidColorBrush(Colors.White),
            true, BindingMode.TwoWay);

    public static readonly StyledProperty<double> StrokeProperty =
        AvaloniaProperty.Register<Arc, double>(nameof(Stroke), 10, true,
            BindingMode.TwoWay);

    public static readonly StyledProperty<double> StartAngleProperty =
        AvaloniaProperty.Register<Arc, double>(nameof(StartAngle), 0, true,
            BindingMode.TwoWay);

    public static readonly StyledProperty<double> SweepAngleProperty =
        AvaloniaProperty.Register<Arc, double>(nameof(SweepAngle), 90, true,
            BindingMode.TwoWay);

    static Arc()
    {
        AffectsRender<Arc>(ArcBrushProperty,
            StrokeProperty,
            StartAngleProperty,
            SweepAngleProperty);
    }

    public IBrush ArcBrush
    {
        get => GetValue(ArcBrushProperty);
        set => SetValue(ArcBrushProperty, value);
    }

    public double Stroke
    {
        get => GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    public double StartAngle
    {
        get => GetValue(StartAngleProperty);
        set => SetValue(StartAngleProperty, value);
    }

    public double SweepAngle
    {
        get => GetValue(SweepAngleProperty);
        set => SetValue(SweepAngleProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        double offsetStroke = 0.5;
        double o = Stroke + offsetStroke;

        // Create main circle for draw circle
        EllipseGeometry mainCircle =
            new EllipseGeometry(new Rect(o / 2, o / 2, Bounds.Width - o, Bounds.Height - o));

        Pen paint = new Pen(ArcBrush, Stroke);

        // Push generated clip geometry for clipping circle figure
        context.PlatformImpl.PushGeometryClip(GetClip().PlatformImpl);
        context.PlatformImpl.DrawGeometry(SolidColorBrush.Parse("Transparent"), paint, mainCircle.PlatformImpl);
        context.PlatformImpl.PopGeometryClip();
        // Pop clip geometry

        Dispatcher.UIThread.InvokeAsync(InvalidateVisual, DispatcherPriority.Background);
    }

    // Clip geometry generator
    private StreamGeometry GetClip()
    {
        double offset = StartAngle - 90;

        double w = Bounds.Width;
        double h = Bounds.Height;

        double halfW = w / 2;
        double halfH = h / 2;

        double sweep = (offset + SweepAngle) / 360;

        StringBuilder path = new StringBuilder(
            $"M {halfW.ToString(CultureInfo.InvariantCulture)} {halfH.ToString(CultureInfo.InvariantCulture)}");

        int length = 24;

        for (int i = 0; i < length; i++)
        {
            double limit = offset / 360 + i / (double)length;

            if (limit > sweep)
                break;

            double r2 = limit * (Math.PI * 2);
            double x2 = halfW + Math.Round(halfW * Math.Cos(r2), 4);
            double y2 = halfH + Math.Round(halfH * Math.Sin(r2), 4);

            path.Append(
                $" {x2.ToString(CultureInfo.InvariantCulture)} {y2.ToString(CultureInfo.InvariantCulture)}");
        }

        double r3 = sweep * (Math.PI * 2);
        double x3 = halfW + Math.Round(halfW * Math.Cos(r3), 4);
        double y3 = halfH + Math.Round(halfH * Math.Sin(r3), 4);

        path.Append($" {x3.ToString(CultureInfo.InvariantCulture)} {y3.ToString(CultureInfo.InvariantCulture)}");

        path.Append(" Z");
        string result = path.ToString().Replace(',', '.');
        return StreamGeometry.Parse(result);
    }
}
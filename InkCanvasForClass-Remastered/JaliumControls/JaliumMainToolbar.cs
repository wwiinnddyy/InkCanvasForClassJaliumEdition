using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Input;

namespace InkCanvasForClass_Remastered.JaliumControls;

public enum ToolType
{
    Cursor,
    Pen,
    Highlighter,
    Eraser,
    Gesture,
    Clear,
    Settings
}

public class JaliumMainToolbar
{
    private Window? _toolBarWindow;

    private System.Windows.Controls.Control? _cursorBtn;
    private ToggleButton? _penBtn;
    private ToggleButton? _highlighterBtn;
    private ToggleButton? _eraserBtn;
    private ToggleButton? _gestureBtn;
    private Button? _clearBtn;
    private Button? _settingsBtn;

    private Popup? _penPopup;
    private Popup? _highlighterPopup;
    private Popup? _eraserPopup;
    private Popup? _gesturePopup;

    private double _penWidth = 5.0;
    private double _penAlpha = 255.0;
    private double _highlighterWidth = 20.0;
    private double _eraserSize = 3.0;
    private string _penColor = "Black";

    public event EventHandler<ToolType>? ToolSelected;
    public event EventHandler<double>? PenWidthChanged;
    public event EventHandler<double>? PenAlphaChanged;
    public event EventHandler<string>? PenColorChanged;
    public event EventHandler<double>? HighlighterWidthChanged;
    public event EventHandler<double>? EraserSizeChanged;
    public event EventHandler<bool>? MultiTouchChanged;
    public event EventHandler<bool>? TwoFingerZoomChanged;
    public event EventHandler<bool>? TwoFingerTranslateChanged;
    public event EventHandler<bool>? TwoFingerRotateChanged;

    public double PenWidth
    {
        get => _penWidth;
        set
        {
            _penWidth = value;
            PenWidthChanged?.Invoke(this, _penWidth);
        }
    }

    public double PenAlpha
    {
        get => _penAlpha;
        set
        {
            _penAlpha = value;
            PenAlphaChanged?.Invoke(this, _penAlpha);
        }
    }

    public double HighlighterWidth
    {
        get => _highlighterWidth;
        set
        {
            _highlighterWidth = value;
            HighlighterWidthChanged?.Invoke(this, _highlighterWidth);
        }
    }

    public double EraserSize
    {
        get => _eraserSize;
        set
        {
            _eraserSize = value;
            EraserSizeChanged?.Invoke(this, _eraserSize);
        }
    }

    public string PenColor
    {
        get => _penColor;
        set
        {
            _penColor = value;
            PenColorChanged?.Invoke(this, _penColor);
        }
    }

    public void Show()
    {
        if (_toolBarWindow == null)
        {
            _toolBarWindow = CreateMainWindow();
        }
        _toolBarWindow.Show();
    }

    public void Close()
    {
        _toolBarWindow?.Close();
    }

    public void Hide()
    {
        if (_toolBarWindow != null)
        {
            _toolBarWindow.Hide();
        }
    }

    public void ShowToolBar()
    {
        if (_toolBarWindow != null && !_toolBarWindow.IsVisible)
        {
            _toolBarWindow.Show();
        }
    }

    private Window CreateMainWindow()
    {
        var window = new Window
        {
            Width = 400,
            Height = 70,
            ResizeMode = ResizeMode.NoResize,
            WindowStyle = WindowStyle.None,
            AllowsTransparency = true,
            Background = new SolidColorBrush(Colors.Transparent),
            Topmost = true,
            ShowInTaskbar = false,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Left = 100,
            Top = 10
        };

        var mainBorder = new System.Windows.Controls.Border
        {
            Background = new SolidColorBrush(ToColor("#fafafa")),
            BorderBrush = new SolidColorBrush(ToColor("#2563eb")),
            BorderThickness = new System.Windows.Thickness(1),
            CornerRadius = new System.Windows.CornerRadius(8),
            Padding = new System.Windows.Thickness(4, 2, 4, 2),
            Child = CreateToolBarPanel()
        };

        window.Content = mainBorder;
        return window;
    }

    private StackPanel CreateToolBarPanel()
    {
        var panel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Margin = new System.Windows.Thickness(0)
        };

        _cursorBtn = CreateToolButton("鼠标", GetCursorIconPath(), ToolType.Cursor);
        panel.Children.Add(_cursorBtn);

        _penBtn = CreateToggleToolButtonWithIcon("批注", GetPenIconPath(), ToolType.Pen);
        _penBtn.Checked += (s, e) => ShowPopup(_penPopup);
        _penBtn.Unchecked += (s, e) => HidePopup(_penPopup);
        panel.Children.Add(_penBtn);

        _highlighterBtn = CreateToggleToolButtonWithIcon("荧光笔", GetHighlighterIconPath(), ToolType.Highlighter);
        _highlighterBtn.Checked += (s, e) => ShowPopup(_highlighterPopup);
        _highlighterBtn.Unchecked += (s, e) => HidePopup(_highlighterPopup);
        panel.Children.Add(_highlighterBtn);

        _eraserBtn = CreateToggleToolButtonWithIcon("橡皮", GetEraserIconPath(), ToolType.Eraser);
        _eraserBtn.Checked += (s, e) => ShowPopup(_eraserPopup);
        _eraserBtn.Unchecked += (s, e) => HidePopup(_eraserPopup);
        panel.Children.Add(_eraserBtn);

        _gestureBtn = CreateToggleToolButtonWithIcon("手势", GetGestureIconPath(), ToolType.Gesture);
        _gestureBtn.Checked += (s, e) => ShowPopup(_gesturePopup);
        _gestureBtn.Unchecked += (s, e) => HidePopup(_gesturePopup);
        panel.Children.Add(_gestureBtn);

        _clearBtn = CreateToolButton("清空", GetClearIconPath(), ToolType.Clear);
        panel.Children.Add(_clearBtn);

        _settingsBtn = CreateToolButton("设置", GetSettingsIconPath(), ToolType.Settings);
        panel.Children.Add(_settingsBtn);

        _penPopup = CreatePenPopup();
        _highlighterPopup = CreateHighlighterPopup();
        _eraserPopup = CreateEraserPopup();
        _gesturePopup = CreateGesturePopup();

        return panel;
    }

    private ToggleButton CreateToggleToolButtonWithIcon(string label, string iconPath, ToolType toolType)
    {
        var btn = new ToggleButton
        {
            MinWidth = 36,
            MinHeight = 55,
            Margin = new System.Windows.Thickness(2),
            Padding = new System.Windows.Thickness(4, 2, 4, 2),
            Tag = toolType,
            Cursor = Cursors.Hand,
            Background = new SolidColorBrush(Colors.Transparent),
            BorderThickness = new System.Windows.Thickness(0)
        };

        btn.Click += (s, e) =>
        {
            if (toolType != ToolType.Clear && toolType != ToolType.Settings)
            {
                UncheckOtherTools(btn);
            }
            ToolSelected?.Invoke(this, toolType);
        };

        var content = new StackPanel
        {
            Orientation = Orientation.Vertical,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        var viewbox = new Viewbox
        {
            Width = 28,
            Height = 17,
            Margin = new System.Windows.Thickness(0, 3, 0, 0)
        };

        var path = new System.Windows.Shapes.Path
        {
            Data = Geometry.Parse(iconPath),
            Fill = new SolidColorBrush(ToColor("#1b1b1b")),
            Stretch = Stretch.Uniform
        };
        viewbox.Child = path;

        content.Children.Add(viewbox);

        var textBlock = new System.Windows.Controls.TextBlock
        {
            Text = label,
            FontSize = 8,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new System.Windows.Thickness(0, 1, 0, 0),
            Foreground = new SolidColorBrush(ToColor("#1b1b1b"))
        };
        content.Children.Add(textBlock);

        btn.Content = content;

        return btn;
    }

    private Button CreateToolButton(string label, string iconPath, ToolType toolType)
    {
        var btn = new Button
        {
            MinWidth = 36,
            MinHeight = 55,
            Margin = new System.Windows.Thickness(2),
            Padding = new System.Windows.Thickness(4, 2, 4, 2),
            Tag = toolType,
            Cursor = Cursors.Hand,
            Background = new SolidColorBrush(Colors.Transparent),
            BorderThickness = new System.Windows.Thickness(0)
        };

        btn.Click += (s, e) =>
        {
            ToolSelected?.Invoke(this, toolType);
        };

        var content = new StackPanel
        {
            Orientation = Orientation.Vertical,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        var viewbox = new Viewbox
        {
            Width = 28,
            Height = 17,
            Margin = new System.Windows.Thickness(0, 3, 0, 0)
        };

        var pathColor = toolType == ToolType.Clear ? ToColor("#b91c1c") : ToColor("#1b1b1b");
        var path = new System.Windows.Shapes.Path
        {
            Data = Geometry.Parse(iconPath),
            Fill = new SolidColorBrush(pathColor),
            Stretch = Stretch.Uniform
        };
        viewbox.Child = path;

        content.Children.Add(viewbox);

        var textBlock = new System.Windows.Controls.TextBlock
        {
            Text = label,
            FontSize = 8,
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new System.Windows.Thickness(0, 1, 0, 0),
            Foreground = new SolidColorBrush(pathColor)
        };
        content.Children.Add(textBlock);

        btn.Content = content;

        return btn;
    }

    private void UncheckOtherTools(ToggleButton except)
    {
        if (_penBtn != except) _penBtn.IsChecked = false;
        if (_highlighterBtn != except) _highlighterBtn.IsChecked = false;
        if (_eraserBtn != except) _eraserBtn.IsChecked = false;
        if (_gestureBtn != except) _gestureBtn.IsChecked = false;
    }

    private void ShowPopup(Popup? popup)
    {
        HideAllPopups();
        if (popup != null)
        {
            popup.IsOpen = true;
        }
    }

    private void HidePopup(Popup? popup)
    {
        if (popup != null)
        {
            popup.IsOpen = false;
        }
    }

    private void HideAllPopups()
    {
        if (_penPopup != null) _penPopup.IsOpen = false;
        if (_highlighterPopup != null) _highlighterPopup.IsOpen = false;
        if (_eraserPopup != null) _eraserPopup.IsOpen = false;
        if (_gesturePopup != null) _gesturePopup.IsOpen = false;
    }

    private Popup CreatePenPopup()
    {
        var popup = new Popup
        {
            AllowsTransparency = true,
            Placement = PlacementMode.Top,
            PlacementTarget = _penBtn,
            PopupAnimation = PopupAnimation.Fade,
            StaysOpen = false,
            HorizontalOffset = -50,
            VerticalOffset = 5
        };

        var border = new System.Windows.Controls.Border
        {
            MinWidth = 280,
            Background = new SolidColorBrush(ToColor("#fafafa")),
            BorderBrush = new SolidColorBrush(ToColor("#2563eb")),
            BorderThickness = new System.Windows.Thickness(1),
            CornerRadius = new System.Windows.CornerRadius(8),
            Padding = new System.Windows.Thickness(16),
            Margin = new System.Windows.Thickness(0, 5, 0, 0)
        };

        var panel = new StackPanel();

        panel.Children.Add(new System.Windows.Controls.TextBlock
        {
            Text = "笔刷设置",
            FontSize = 14,
            FontWeight = FontWeights.Bold,
            Foreground = new SolidColorBrush(ToColor("#18181b")),
            Margin = new System.Windows.Thickness(0, 0, 0, 12)
        });

        var widthPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new System.Windows.Thickness(0, 0, 0, 8) };
        widthPanel.Children.Add(new System.Windows.Controls.TextBlock
        {
            Text = "粗细",
            VerticalAlignment = VerticalAlignment.Center,
            Width = 45,
            Foreground = new SolidColorBrush(ToColor("#18181b"))
        });
        var widthSlider = new System.Windows.Controls.Slider { Width = 160, Minimum = 1, Maximum = 20, Value = _penWidth };
        var widthValue = new System.Windows.Controls.TextBlock
        {
            Text = $"{_penWidth:F1}",
            VerticalAlignment = VerticalAlignment.Center,
            Width = 40,
            Margin = new System.Windows.Thickness(8, 0, 0, 0),
            Foreground = new SolidColorBrush(ToColor("#18181b"))
        };
        widthSlider.ValueChanged += (s, e) =>
        {
            _penWidth = e.NewValue;
            widthValue.Text = $"{_penWidth:F1}";
            PenWidthChanged?.Invoke(this, _penWidth);
        };
        widthPanel.Children.Add(widthSlider);
        widthPanel.Children.Add(widthValue);
        panel.Children.Add(widthPanel);

        var alphaPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new System.Windows.Thickness(0, 0, 0, 12) };
        alphaPanel.Children.Add(new System.Windows.Controls.TextBlock
        {
            Text = "透明度",
            VerticalAlignment = VerticalAlignment.Center,
            Width = 45,
            Foreground = new SolidColorBrush(ToColor("#18181b"))
        });
        var alphaSlider = new System.Windows.Controls.Slider { Width = 160, Minimum = 1, Maximum = 255, Value = _penAlpha };
        var alphaValue = new System.Windows.Controls.TextBlock
        {
            Text = $"{_penAlpha:F0}",
            VerticalAlignment = VerticalAlignment.Center,
            Width = 40,
            Margin = new System.Windows.Thickness(8, 0, 0, 0),
            Foreground = new SolidColorBrush(ToColor("#18181b"))
        };
        alphaSlider.ValueChanged += (s, e) =>
        {
            _penAlpha = e.NewValue;
            alphaValue.Text = $"{_penAlpha:F0}";
            PenAlphaChanged?.Invoke(this, _penAlpha);
        };
        alphaPanel.Children.Add(alphaSlider);
        alphaPanel.Children.Add(alphaValue);
        panel.Children.Add(alphaPanel);

        panel.Children.Add(new System.Windows.Controls.TextBlock
        {
            Text = "颜色",
            FontSize = 14,
            FontWeight = FontWeights.Bold,
            Foreground = new SolidColorBrush(ToColor("#18181b")),
            Margin = new System.Windows.Thickness(0, 0, 0, 8)
        });

        var colorPanel = new WrapPanel();
        var colors = new (string Name, string Hex)[]
        {
            ("Black", "#000000"),
            ("White", "#ffffff"),
            ("Red", "#dc2626"),
            ("Yellow", "#eab308"),
            ("Green", "#16a34a"),
            ("Blue", "#2563eb")
        };

        foreach (var (name, hex) in colors)
        {
            var colorBtn = new System.Windows.Controls.Border
            {
                Width = 32,
                Height = 32,
                Margin = new System.Windows.Thickness(2),
                Background = new SolidColorBrush(ToColor(hex)),
                BorderBrush = new SolidColorBrush(ToColor(name == "White" ? "#d1d5db" : hex)),
                BorderThickness = new System.Windows.Thickness(1),
                CornerRadius = new System.Windows.CornerRadius(4),
                Cursor = Cursors.Hand,
                Tag = name
            };
            var capturedName = name;
            colorBtn.MouseLeftButtonDown += (s, e) =>
            {
                _penColor = capturedName;
                PenColorChanged?.Invoke(this, _penColor);
                _penBtn.IsChecked = false;
            };
            colorPanel.Children.Add(colorBtn);
        }
        panel.Children.Add(colorPanel);

        border.Child = panel;
        popup.Child = border;

        return popup;
    }

    private Popup CreateHighlighterPopup()
    {
        var popup = new Popup
        {
            AllowsTransparency = true,
            Placement = PlacementMode.Top,
            PlacementTarget = _highlighterBtn,
            PopupAnimation = PopupAnimation.Fade,
            StaysOpen = false,
            HorizontalOffset = -50,
            VerticalOffset = 5
        };

        var border = new System.Windows.Controls.Border
        {
            MinWidth = 280,
            Background = new SolidColorBrush(ToColor("#fafafa")),
            BorderBrush = new SolidColorBrush(ToColor("#eab308")),
            BorderThickness = new System.Windows.Thickness(2),
            CornerRadius = new System.Windows.CornerRadius(8),
            Padding = new System.Windows.Thickness(16),
            Margin = new System.Windows.Thickness(0, 5, 0, 0)
        };

        var panel = new StackPanel();

        panel.Children.Add(new System.Windows.Controls.TextBlock
        {
            Text = "荧光笔设置",
            FontSize = 14,
            FontWeight = FontWeights.Bold,
            Foreground = new SolidColorBrush(ToColor("#18181b")),
            Margin = new System.Windows.Thickness(0, 0, 0, 12)
        });

        var widthPanel = new StackPanel { Orientation = Orientation.Horizontal };
        widthPanel.Children.Add(new System.Windows.Controls.TextBlock
        {
            Text = "粗细",
            VerticalAlignment = VerticalAlignment.Center,
            Width = 45,
            Foreground = new SolidColorBrush(ToColor("#18181b"))
        });
        var widthSlider = new System.Windows.Controls.Slider { Width = 160, Minimum = 15, Maximum = 45, Value = _highlighterWidth };
        var widthValue = new System.Windows.Controls.TextBlock
        {
            Text = $"{_highlighterWidth:F0}",
            VerticalAlignment = VerticalAlignment.Center,
            Width = 40,
            Margin = new System.Windows.Thickness(8, 0, 0, 0),
            Foreground = new SolidColorBrush(ToColor("#18181b"))
        };
        widthSlider.ValueChanged += (s, e) =>
        {
            _highlighterWidth = e.NewValue;
            widthValue.Text = $"{_highlighterWidth:F0}";
            HighlighterWidthChanged?.Invoke(this, _highlighterWidth);
        };
        widthPanel.Children.Add(widthSlider);
        widthPanel.Children.Add(widthValue);
        panel.Children.Add(widthPanel);

        border.Child = panel;
        popup.Child = border;

        return popup;
    }

    private Popup CreateEraserPopup()
    {
        var popup = new Popup
        {
            AllowsTransparency = true,
            Placement = PlacementMode.Top,
            PlacementTarget = _eraserBtn,
            PopupAnimation = PopupAnimation.Fade,
            StaysOpen = false,
            HorizontalOffset = -50,
            VerticalOffset = 5
        };

        var border = new System.Windows.Controls.Border
        {
            MinWidth = 220,
            Background = new SolidColorBrush(ToColor("#fafafa")),
            BorderBrush = new SolidColorBrush(ToColor("#71717a")),
            BorderThickness = new System.Windows.Thickness(1),
            CornerRadius = new System.Windows.CornerRadius(8),
            Padding = new System.Windows.Thickness(16),
            Margin = new System.Windows.Thickness(0, 5, 0, 0)
        };

        var panel = new StackPanel();

        panel.Children.Add(new System.Windows.Controls.TextBlock
        {
            Text = "橡皮设置",
            FontSize = 14,
            FontWeight = FontWeights.Bold,
            Foreground = new SolidColorBrush(ToColor("#18181b")),
            Margin = new System.Windows.Thickness(0, 0, 0, 12)
        });

        var sizePanel = new StackPanel { Orientation = Orientation.Horizontal };
        sizePanel.Children.Add(new System.Windows.Controls.TextBlock
        {
            Text = "大小",
            VerticalAlignment = VerticalAlignment.Center,
            Width = 45,
            Foreground = new SolidColorBrush(ToColor("#18181b"))
        });
        var sizeSlider = new System.Windows.Controls.Slider { Width = 110, Minimum = 1, Maximum = 10, Value = _eraserSize };
        var sizeValue = new System.Windows.Controls.TextBlock
        {
            Text = $"{_eraserSize:F0}",
            VerticalAlignment = VerticalAlignment.Center,
            Width = 30,
            Margin = new System.Windows.Thickness(8, 0, 0, 0),
            Foreground = new SolidColorBrush(ToColor("#18181b"))
        };
        sizeSlider.ValueChanged += (s, e) =>
        {
            _eraserSize = e.NewValue;
            sizeValue.Text = $"{_eraserSize:F0}";
            EraserSizeChanged?.Invoke(this, _eraserSize);
        };
        sizePanel.Children.Add(sizeSlider);
        sizePanel.Children.Add(sizeValue);
        panel.Children.Add(sizePanel);

        border.Child = panel;
        popup.Child = border;

        return popup;
    }

    private Popup CreateGesturePopup()
    {
        var popup = new Popup
        {
            AllowsTransparency = true,
            Placement = PlacementMode.Top,
            PlacementTarget = _gestureBtn,
            PopupAnimation = PopupAnimation.Fade,
            StaysOpen = false,
            HorizontalOffset = -50,
            VerticalOffset = 5
        };

        var border = new System.Windows.Controls.Border
        {
            MinWidth = 240,
            Background = new SolidColorBrush(ToColor("#fafafa")),
            BorderBrush = new SolidColorBrush(ToColor("#2563eb")),
            BorderThickness = new System.Windows.Thickness(1),
            CornerRadius = new System.Windows.CornerRadius(8),
            Padding = new System.Windows.Thickness(16),
            Margin = new System.Windows.Thickness(0, 5, 0, 0)
        };

        var panel = new StackPanel();

        panel.Children.Add(new System.Windows.Controls.TextBlock
        {
            Text = "手势设置",
            FontSize = 14,
            FontWeight = FontWeights.Bold,
            Foreground = new SolidColorBrush(ToColor("#18181b")),
            Margin = new System.Windows.Thickness(0, 0, 0, 12)
        });

        var multiTouchCheck = new System.Windows.Controls.CheckBox
        {
            Content = "多指书写",
            IsChecked = false,
            Margin = new System.Windows.Thickness(0, 4, 0, 0),
            Foreground = new SolidColorBrush(ToColor("#18181b"))
        };
        multiTouchCheck.Checked += (s, e) => MultiTouchChanged?.Invoke(this, true);
        multiTouchCheck.Unchecked += (s, e) => MultiTouchChanged?.Invoke(this, false);
        panel.Children.Add(multiTouchCheck);

        var zoomCheck = new System.Windows.Controls.CheckBox
        {
            Content = "双指缩放",
            IsChecked = false,
            Margin = new System.Windows.Thickness(0, 4, 0, 0),
            Foreground = new SolidColorBrush(ToColor("#18181b"))
        };
        zoomCheck.Checked += (s, e) => TwoFingerZoomChanged?.Invoke(this, true);
        zoomCheck.Unchecked += (s, e) => TwoFingerZoomChanged?.Invoke(this, false);
        panel.Children.Add(zoomCheck);

        var translateCheck = new System.Windows.Controls.CheckBox
        {
            Content = "双指移动",
            IsChecked = true,
            Margin = new System.Windows.Thickness(0, 4, 0, 0),
            Foreground = new SolidColorBrush(ToColor("#18181b"))
        };
        translateCheck.Checked += (s, e) => TwoFingerTranslateChanged?.Invoke(this, true);
        translateCheck.Unchecked += (s, e) => TwoFingerTranslateChanged?.Invoke(this, false);
        panel.Children.Add(translateCheck);

        var rotateCheck = new System.Windows.Controls.CheckBox
        {
            Content = "双指旋转",
            IsChecked = false,
            Margin = new System.Windows.Thickness(0, 4, 0, 0),
            Foreground = new SolidColorBrush(ToColor("#18181b"))
        };
        rotateCheck.Checked += (s, e) => TwoFingerRotateChanged?.Invoke(this, true);
        rotateCheck.Unchecked += (s, e) => TwoFingerRotateChanged?.Invoke(this, false);
        panel.Children.Add(rotateCheck);

        border.Child = panel;
        popup.Child = border;

        return popup;
    }

    private static string GetCursorIconPath()
    {
        return "F0 M24,24z M0,0z M3.85151,2.7073C3.52422,2.57095 3.147,2.64558 2.89629,2.89629 2.64558,3.147 2.57095,3.52422 2.7073,3.85151L9.7773,20.8215C9.91729,21.1575 10.2507,21.3718 10.6145,21.3595 10.9783,21.3473 11.2965,21.1111 11.4135,20.7664L13.4711,14.7085 18.8963,20.1337C19.238,20.4754 19.792,20.4754 20.1337,20.1337 20.4754,19.792 20.4754,19.238 20.1337,18.8963L14.7085,13.4711 20.7664,11.4135C21.1111,11.2965 21.3473,10.9783 21.3595,10.6145 21.3718,10.2507 21.1575,9.91729 20.8215,9.7773L3.85151,2.7073z M10.5017,18.0097L5.13984,5.13984 18.0097,10.5017 12.8136,12.2665C12.5561,12.3539,12.3539,12.5561,12.2665,12.8136L10.5017,18.0097z";
    }

    private static string GetPenIconPath()
    {
        return "F0 M24,24z M0,0z M20.8643,15.0859L20.8643,15.0859C21.0451,15.2667 21.0451,15.5637 20.8643,15.7445L18.7445,17.8643C18.5637,18.0451 18.2667,18.0451 18.0859,17.8643L15.0859,14.8643L18.0859,11.8643C18.2667,11.6835 18.5637,11.6835 18.7445,11.8643L20.8643,13.9841C21.0451,14.1649 21.0451,14.4619 20.8643,14.6427L15.8643,19.6427L13.8643,17.6427L13.8643,5.86426L19.8643,5.86426C20.0451,5.86426 20.3421,5.86426 20.5229,5.68348C20.7037,5.5027 20.7037,5.2057 20.5229,5.02492L20.5229,3.68348L3.68348,3.68348L3.68348,20.5229L5.02492,20.5229C5.2057,20.7037 5.5027,20.7037 5.68348,20.5229C5.86426,20.3421 5.86426,20.0451 5.68348,19.8643L3.68348,19.8643L3.68348,5.02492C3.68348,4.84414 3.68348,4.54714 3.86426,4.36636C4.04504,4.18558 4.34204,4.18558 4.52282,4.36636L20.5229,4.36636C20.7037,4.54714 21.0007,4.54714 21.1815,4.36636C21.3622,4.18558 21.3622,3.88858 21.1815,3.7078L21.1815,2.36636C21.1815,1.58558 20.7037,1.1078 19.9229,1.1078L4.52282,1.1078C3.74204,1.1078 3.26426,1.58558 3.26426,2.36636L3.26426,20.5229L19.8643,20.5229C19.8639,20.5229 19.8635,20.5229 19.863,20.5229C19.8626,20.5229 19.8621,20.5229 19.8617,20.5229C19.8613,20.5229 19.8608,20.5229 19.8604,20.5229L19.8604,20.5229L17.8604,18.5229L20.8643,15.0859z";
    }

    private static string GetHighlighterIconPath()
    {
        return "F0 M24,24z M0,0z M20.8643,15.0859L20.8643,15.0859C21.0451,15.2667 21.0451,15.5637 20.8643,15.7445L18.7445,17.8643C18.5637,18.0451 18.2667,18.0451 18.0859,17.8643L15.0859,14.8643L18.0859,11.8643C18.2667,11.6835 18.5637,11.6835 18.7445,11.8643L20.8643,13.9841C21.0451,14.1649 21.0451,14.4619 20.8643,14.6427L15.8643,19.6427L13.8643,17.6427L13.8643,5.86426L19.8643,5.86426C20.0451,5.86426 20.3421,5.86426 20.5229,5.68348C20.7037,5.5027 20.7037,5.2057 20.5229,5.02492L20.5229,3.68348L3.68348,3.68348L3.68348,20.5229L5.02492,20.5229C5.2057,20.7037 5.5027,20.7037 5.68348,20.5229C5.86426,20.3421 5.86426,20.0451 5.68348,19.8643L3.68348,19.8643L3.68348,5.02492C3.68348,4.84414 3.68348,4.54714 3.86426,4.36636C4.04504,4.18558 4.34204,4.18558 4.52282,4.36636L20.5229,4.36636C20.7037,4.54714 21.0007,4.54714 21.1815,4.36636C21.3622,4.18558 21.3622,3.88858 21.1815,3.7078L21.1815,2.36636C21.1815,1.58558 20.7037,1.1078 19.9229,1.1078L4.52282,1.1078C3.74204,1.1078 3.26426,1.58558 3.26426,2.36636L3.26426,20.5229L19.8643,20.5229C19.8639,20.5229 19.8635,20.5229 19.863,20.5229C19.8626,20.5229 19.8621,20.5229 19.8617,20.5229C19.8613,20.5229 19.8608,20.5229 19.8604,20.5229L19.8604,20.5229L17.8604,18.5229L20.8643,15.0859z";
    }

    private static string GetEraserIconPath()
    {
        return "F0 M24,24z M0,0z M16.3292,4.41421C16.1846,4.26959 15.9806,4.20187 15.7818,4.22966L13.7818,4.47966C13.583,4.50745 13.4106,4.62612 13.3133,4.80275L9.31329,11.3027C9.21601,11.4794 9.22463,11.689 9.33648,11.8545L14.3365,18.8545C14.4483,19.02 14.6452,19.1115 14.8545,19.0941L16.8545,18.8441C17.0638,18.8267 17.2512,18.7065 17.3478,18.5268L21.3478,11.5268C21.4444,11.3471 21.4254,11.1347 21.2988,10.9732L16.3292,4.41421z M15.5,6.5L19.2071,11.7929L16.7929,13.2071L13.0858,7.91421L15.5,6.5z M15.7929,17.7929L14.5,16.5L17.5,11.5L18.7929,12.7929L15.7929,17.7929z M4.82843,5.17157C4.89204,5.23518 4.96602,5.28683 5.04673,5.32323L10.0467,7.32323C10.2465,7.40456 10.4653,7.39012 10.6547,7.28328L15.6547,4.78328C15.8441,4.67643 15.9824,4.48778 16.0355,4.26268L16.2855,3.26268C16.3386,3.03758 16.2953,2.80076 16.1678,2.61047C16.0403,2.42018 15.8398,2.29413 15.6129,2.26464L7.61292,1.26464C7.38603,1.23515 7.16456,1.31443 7.00711,1.48223C6.84966,1.65002 6.77234,1.88896 6.79554,2.13247L6.79554,2.13247L7.04554,6.13247C7.06874,6.37598 7.1958,6.59745 7.39594,6.74036C7.59607,6.88326 7.84965,6.93364 8.08985,6.87868L4.82843,5.17157z";
    }

    private static string GetGestureIconPath()
    {
        return "F0 M24,24z M0,0z M9.87868,11.8787C10.4413,11.3161 11.2043,11 12,11 12.7957,11 13.5587,11.3161 14.1213,11.8787 14.6839,12.4413 15,13.2043 15,14 15,14.7957 14.6839,15.5587 14.1213,16.1213 13.5587,16.6839 12.7957,17 12,17 11.2043,17 10.4413,16.6839 9.87868,16.1213 9.31607,15.5587 9,14.7957 9,14 9,13.2043 9.31607,12.4413 9.87868,11.8787z M12,13C11.7348,13 11.4804,13.1054 11.2929,13.2929 11.1054,13.4804 11,13.7348 11,14 11,14.2652 11.1054,14.5196 11.2929,14.7071 11.4804,14.8946 11.7348,15 12,15 12.2652,15 12.5196,14.8946 12.7071,14.7071 12.8946,14.5196 13,14.2652 13,14 13,13.7348 12.8946,13.4804 12.7071,13.2929 12.5196,13.1054 12.2652,13 12,13z M9.0364,7.7636C8.68492,7.41213 8.11508,7.41213 7.7636,7.7636 7.41213,8.11508 7.41213,8.68492 7.7636,9.0364L10.2609,11.5338C10.2212,11.6825 10.2,11.8387 10.2,12 10.2,12.9941 11.0059,13.8 12,13.8 12.9941,13.8 13.8,12.9941 13.8,12 13.8,11.0059 12.9941,10.2 12,10.2 11.8387,10.2 11.6825,10.2212 11.5338,10.2609L9.0364,7.7636z";
    }

    private static string GetClearIconPath()
    {
        return "F0 M24,24z M0,0z M10.1573,10.0421C10.7298,10.0421,11.1938,10.5062,11.1938,11.0787L11.1938,16.6067C11.1938,17.1792 10.7298,17.6433 10.1573,17.6433 9.58485,17.6433 9.12079,17.1792 9.12079,16.6067L9.12079,11.0787C9.12079,10.5062,9.58485,10.0421,10.1573,10.0421z M13.8427,10.0421C14.4151,10.0421,14.8792,10.5062,14.8792,11.0787L14.8792,16.6067C14.8792,17.1792 14.4151,17.6433 13.8427,17.6433 13.2702,17.6433 12.8062,17.1792 12.8062,16.6067L12.8062,11.0787C12.8062,10.5062,13.2702,10.0421,13.8427,10.0421z M3.70787,5.43539C3.13541,5.43539 2.67135,5.89946 2.67135,6.47191 2.67135,7.04436 3.13541,7.50843 3.70787,7.50843L4.51405,7.50843 4.51405,19.3708C4.51405,20.1686 4.9025,20.8796 5.39348,21.3706 5.88445,21.8615 6.59548,22.25 7.39326,22.25L16.6067,22.25C17.4045,22.25 18.1155,21.8615 18.6065,21.3706 19.0975,20.8796 19.486,20.1686 19.486,19.3708L19.486,7.50843 20.2921,7.50843C20.8646,7.50843 21.3287,7.04436 21.3287,6.47191 21.3287,5.89946 20.8646,5.43539 20.2921,5.43539L16.7219,5.43539 16.7219,4.62921C16.7219,3.83143 16.3335,3.12041 15.8425,2.62943 15.3515,2.13845 14.6405,1.75 13.8427,1.75L10.1573,1.75C9.35952,1.75 8.6485,2.13845 8.15752,2.62943 7.66654,3.12041 7.27809,3.83143 7.27809,4.62921L7.27809,5.43539 3.70787,5.43539z M6.58708,19.3708C6.58708,19.4944 6.6593,19.7047 6.85933,19.9047 7.05937,20.1047 7.26969,20.177 7.39326,20.177L16.6067,20.177C16.7303,20.177 16.9406,20.1047 17.1407,19.9047 17.3407,19.7047 17.4129,19.4944 17.4129,19.3708L17.4129,7.50843 6.58708,7.50843 6.58708,19.3708z M9.62338,4.09529C9.42334,4.29532,9.35112,4.50565,9.35112,4.62921L9.35112,5.43539 14.6489,5.43539 14.6489,4.62921C14.6489,4.50565 14.5767,4.29532 14.3766,4.09529 14.1766,3.89525 13.9663,3.82303 13.8427,3.82303L10.1573,3.82303C10.0337,3.82303,9.82341,3.89525,9.62338,4.09529z";
    }

    private static string GetSettingsIconPath()
    {
        return "F0 M24,24z M0,0z M4.66591,7.13141L11.4017,3.15976C11.5957,3.0552 11.8126,3.00041 12.033,3.00041 12.2534,3.00041 12.4704,3.0552 12.6643,3.15977L19.2182,7.02415C19.2676,7.06721 19.3219,7.10586 19.3806,7.13926 19.5699,7.24687 19.727,7.40296 19.8358,7.59146 19.9447,7.77997 20.0014,7.99407 20,8.21175L20,8.21175 20,8.218 20,15.502C20,15.943 19.7585,16.3548 19.3603,16.5737 19.3424,16.5835 19.3247,16.5939 19.3074,16.6049L12.5874,20.8559C12.4062,20.9506 12.2047,21.0001 12,21.0001 11.7953,21.0001 11.5938,20.9506 11.4126,20.8559L4.69261,16.6049C4.6746,16.5935 4.65624,16.5827 4.63755,16.5725 4.44494,16.4672 4.28416,16.3122 4.172,16.1235 4.05999,15.9351 4.00059,15.72 4,15.5008L4,8.217C4,7.77653 4.24107,7.36544 4.63968,7.14635 4.6485,7.1415 4.65724,7.13652 4.66591,7.13141z M20.4159,5.40859C20.4791,5.44583 20.5369,5.4892 20.589,5.53759 20.9895,5.81003 21.3244,6.16988 21.5678,6.59125 21.8538,7.08656 22.003,7.649 22,8.22093L22,15.502C22,16.6678,21.3677,17.7387,20.353,18.31L13.6266,22.5651C13.6092,22.5761 13.5914,22.5866 13.5733,22.5966 13.0911,22.8613 12.55,23.0001 12,23.0001 11.45,23.0001 10.9089,22.8613 10.4267,22.5966 10.4086,22.5866 10.3908,22.5761 10.3734,22.5651L3.64791,18.3106C3.15439,18.0339 2.74214,17.6322 2.45282,17.1455 2.15755,16.6488 2.00116,16.0818 2,15.504L2,15.502 2,8.217C2,7.04497,2.63892,5.97063,3.6619,5.40163L10.4001,1.42859C10.4084,1.4237 10.4167,1.41894 10.4252,1.41429 10.9176,1.1428 11.4707,1.00041 12.033,1.00041 12.5953,1.00041 13.1484,1.1428 13.6408,1.41429 13.6493,1.41894 13.6576,1.4237 13.6659,1.42859L20.4159,5.40859z M12,8C10.9391,8 9.92172,8.42143 9.17157,9.17157 8.42143,9.92172 8,10.9391 8,12 8,13.0609 8.42143,14.0783 9.17157,14.8284 9.92172,15.5786 10.9391,16 12,16 13.0609,16 14.0783,15.5786 14.8284,14.8284 15.5786,14.0783 16,13.0609 16,12 16,10.9391 15.5786,9.92172 14.8284,9.17157 14.0783,8.42143 13.0609,8 12,8z";
    }

    private static Color ToColor(string hex)
    {
        if (hex.StartsWith("#"))
            hex = hex.Substring(1);

        if (hex.Length == 6)
        {
            var r = Convert.ToByte(hex.Substring(0, 2), 16);
            var g = Convert.ToByte(hex.Substring(2, 2), 16);
            var b = Convert.ToByte(hex.Substring(4, 2), 16);
            return Color.FromArgb(255, r, g, b);
        }
        return Colors.Black;
    }
}
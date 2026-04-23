using System;
using System.Collections.Generic;
using Jalium.UI;
using Jalium.UI.Controls;
using Jalium.UI.Media;
using Jalium.UI.Markup;

namespace InkCanvasForClass_Remastered.JaliumSettings;

public class JaliumSettingsWindow
{
    private NavigationView? _navigationView;
    private StackPanel? _contentPanel;
    private Window? _window;
    private readonly Models.Settings _settings;

    public event EventHandler? SettingsClosed;

    public JaliumSettingsWindow(Models.Settings settings)
    {
        _settings = settings;
    }

    public void Show()
    {
        var app = new Application();

        _window = new Window
        {
            Title = "ICC-Re 设置",
            Width = 1000,
            Height = 700,
            MinWidth = 800,
            MinHeight = 600,
            Background = new SolidColorBrush(ToColor("#202020"))
        };

        var grid = new Grid();

        _navigationView = new NavigationView
        {
            PaneDisplayMode = NavigationViewPaneDisplayMode.Left,
            IsPaneOpen = true
        };

        var paneHeader = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Margin = new Thickness(16, 8, 16, 8)
        };
        paneHeader.Children.Add(new TextBlock
        {
            Text = "ICC-Re",
            FontSize = 20,
            FontWeight = FontWeights.Bold,
            Foreground = new SolidColorBrush(Colors.White),
            VerticalAlignment = VerticalAlignment.Center
        });
        paneHeader.Children.Add(new TextBlock
        {
            Text = "设置",
            FontSize = 20,
            Foreground = new SolidColorBrush(ToColor("#9ca3af")),
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(4, 0, 0, 0)
        });
        _navigationView.PaneHeader = paneHeader;

        CreateMenuItems();
        CreateFooterMenuItems();

        _contentPanel = new StackPanel { Margin = new Thickness(32) };

        var scrollViewer = new ScrollViewer
        {
            PanningMode = PanningMode.VerticalFirst,
            IsScrollInertiaEnabled = true,
            IsScrollBarAutoHideEnabled = true,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto
        };
        scrollViewer.Content = _contentPanel;
        _navigationView.Content = scrollViewer;

        _navigationView.ItemInvoked += OnItemInvoked;

        grid.Children.Add(_navigationView);
        _window.Content = grid;

        _window.Closed += (s, e) => SettingsClosed?.Invoke(this, EventArgs.Empty);

        NavigateTo("ink");

        app.Run(_window);
    }

    public void Close()
    {
        _window?.Close();
    }

    private void CreateMenuItems()
    {
        var canvasItem = new NavigationViewItem
        {
            Content = "画布",
            Icon = "Edit",
            Tag = "hub_canvas"
        };
        canvasItem.MenuItems.Add(new NavigationViewItem { Content = "墨迹设置", Tag = "ink" });
        canvasItem.MenuItems.Add(new NavigationViewItem { Content = "橡皮设置", Tag = "eraser" });
        canvasItem.MenuItems.Add(new NavigationViewItem { Content = "高光设置", Tag = "highlighter" });
        _navigationView!.MenuItems.Add(canvasItem);

        var gestureItem = new NavigationViewItem
        {
            Content = "手势",
            Icon = "People",
            Tag = "hub_gesture"
        };
        gestureItem.MenuItems.Add(new NavigationViewItem { Content = "触控手势", Tag = "touch" });
        gestureItem.MenuItems.Add(new NavigationViewItem { Content = "笔势识别", Tag = "gesture" });
        _navigationView!.MenuItems.Add(gestureItem);

        var appearanceItem = new NavigationViewItem
        {
            Content = "外观",
            Icon = "Color",
            Tag = "hub_appearance"
        };
        appearanceItem.MenuItems.Add(new NavigationViewItem { Content = "主题", Tag = "theme" });
        appearanceItem.MenuItems.Add(new NavigationViewItem { Content = "字体", Tag = "font" });
        _navigationView!.MenuItems.Add(appearanceItem);

        var pptItem = new NavigationViewItem
        {
            Content = "PowerPoint",
            Icon = "SlideLayout",
            Tag = "hub_powerpoint"
        };
        pptItem.MenuItems.Add(new NavigationViewItem { Content = "放映设置", Tag = "slideshow" });
        pptItem.MenuItems.Add(new NavigationViewItem { Content = "墨迹保存", Tag = "inksave" });
        _navigationView!.MenuItems.Add(pptItem);

        var startupItem = new NavigationViewItem
        {
            Content = "启动",
            Icon = "Clock",
            Tag = "hub_startup"
        };
        startupItem.MenuItems.Add(new NavigationViewItem { Content = "基本设置", Tag = "startup" });
        startupItem.MenuItems.Add(new NavigationViewItem { Content = "自动化", Tag = "automation" });
        _navigationView!.MenuItems.Add(startupItem);

        var advancedItem = new NavigationViewItem
        {
            Content = "高级",
            Icon = "Settings",
            Tag = "hub_advanced"
        };
        advancedItem.MenuItems.Add(new NavigationViewItem { Content = "触控设置", Tag = "touch_advanced" });
        advancedItem.MenuItems.Add(new NavigationViewItem { Content = "系统集成", Tag = "system" });
        _navigationView!.MenuItems.Add(advancedItem);
    }

    private void CreateFooterMenuItems()
    {
        _navigationView!.FooterMenuItems.Add(new NavigationViewItem
        {
            Content = "关于",
            Icon = "Info",
            Tag = "about"
        });
    }

    private void OnItemInvoked(object? sender, NavigationViewItemInvokedEventArgs e)
    {
        var item = e.InvokedItem as NavigationViewItem;
        if (item == null) return;

        var tag = item.Tag as string;
        if (tag != null)
        {
            NavigateTo(tag);
        }
        else if (item.Content is string content)
        {
            switch (content)
            {
                case "关于":
                    NavigateTo("about");
                    break;
            }
        }
    }

    private void NavigateTo(string page)
    {
        if (_contentPanel == null) return;

        _contentPanel.Children.Clear();

        switch (page)
        {
            case "hub_canvas":
                ShowCanvasHub();
                break;
            case "hub_gesture":
                ShowGestureHub();
                break;
            case "hub_appearance":
                ShowAppearanceHub();
                break;
            case "hub_powerpoint":
                ShowPowerPointHub();
                break;
            case "hub_startup":
                ShowStartupHub();
                break;
            case "hub_advanced":
                ShowAdvancedHub();
                break;
            case "ink":
                ShowInkSettings();
                break;
            case "eraser":
                ShowEraserSettings();
                break;
            case "highlighter":
                ShowHighlighterSettings();
                break;
            case "touch":
                ShowTouchSettings();
                break;
            case "gesture":
                ShowGestureSettings();
                break;
            case "theme":
                ShowThemeSettings();
                break;
            case "font":
                ShowFontSettings();
                break;
            case "slideshow":
                ShowSlideshowSettings();
                break;
            case "inksave":
                ShowInkSaveSettings();
                break;
            case "startup":
                ShowStartupSettings();
                break;
            case "automation":
                ShowAutomationSettings();
                break;
            case "touch_advanced":
                ShowTouchAdvancedSettings();
                break;
            case "system":
                ShowSystemIntegrationSettings();
                break;
            case "about":
                ShowAbout();
                break;
            default:
                ShowInkSettings();
                break;
        }
    }

    private void ShowCanvasHub()
    {
        _contentPanel!.Children.Add(CreateHeader("画布", "选择画布相关的设置选项"));
        _contentPanel.Children.Add(CreateHubGrid(new[]
        {
            ("ink", "墨迹设置", "配置墨迹宽度、颜色、压力敏感度等", "✏️"),
            ("eraser", "橡皮设置", "配置橡皮模式、大小、擦除行为等", "🧽"),
            ("highlighter", "高光设置", "配置高亮笔的宽度，透明度和颜色", "🖍️")
        }));
    }

    private void ShowGestureHub()
    {
        _contentPanel!.Children.Add(CreateHeader("手势", "选择手势相关的设置选项"));
        _contentPanel.Children.Add(CreateHubGrid(new[]
        {
            ("touch", "触控手势", "配置多点触控、双指缩放、移动等", "👆"),
            ("gesture", "笔势识别", "配置手写识别和预设手势", "✍️")
        }));
    }

    private void ShowAppearanceHub()
    {
        _contentPanel!.Children.Add(CreateHeader("外观", "选择外观相关的设置选项"));
        _contentPanel.Children.Add(CreateHubGrid(new[]
        {
            ("theme", "主题", "配置颜色模式、Windows效果和自定义选项", "🎨"),
            ("font", "字体", "配置界面字体和墨迹文字识别", "🔤")
        }));
    }

    private void ShowPowerPointHub()
    {
        _contentPanel!.Children.Add(CreateHeader("PowerPoint", "选择PowerPoint相关的设置选项"));
        _contentPanel.Children.Add(CreateHubGrid(new[]
        {
            ("slideshow", "放映设置", "配置PPT放映时的墨迹标注行为", "📽️"),
            ("inksave", "墨迹保存", "配置墨迹保存格式和历史版本管理", "💾")
        }));
    }

    private void ShowStartupHub()
    {
        _contentPanel!.Children.Add(CreateHeader("启动", "选择启动相关的设置选项"));
        _contentPanel.Children.Add(CreateHubGrid(new[]
        {
            ("startup", "基本设置", "开机自启、隐藏工具栏等", "🚀"),
            ("automation", "自动化", "自动折叠、截图保存等", "⚡")
        }));
    }

    private void ShowAdvancedHub()
    {
        _contentPanel!.Children.Add(CreateHeader("高级", "选择高级设置选项"));
        _contentPanel.Children.Add(CreateHubGrid(new[]
        {
            ("touch_advanced", "触控设置", "触控参数、边界设置等", "👆"),
            ("system", "系统集成", "边缘手势、窗口模式等", "⚙️")
        }));
    }

    private StackPanel CreateHubGrid((string tag, string title, string description, string icon)[] items)
    {
        var panel = new StackPanel();

        var grid = new Grid();
        grid.Margin = new Thickness(0, 0, 0, 16);

        for (int i = 0; i < items.Length; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
        }
        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var card = CreateHubCard(item.tag, item.title, item.description, item.icon);
            Grid.SetRow(card, i);
            grid.Children.Add(card);
        }

        panel.Children.Add(grid);
        return panel;
    }

    private Border CreateHubCard(string tag, string title, string description, string icon)
    {
        var card = new Border
        {
            Background = new SolidColorBrush(ToColor("#2d2d2d")),
            CornerRadius = new CornerRadius(8),
            Margin = new Thickness(0, 0, 0, 12),
            Tag = tag,
            IsManipulationEnabled = true
        };

        var content = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(20)
        };

        var iconBorder = new Border
        {
            Background = new SolidColorBrush(ToColor("#3f3f3f")),
            CornerRadius = new CornerRadius(8),
            Width = 48,
            Height = 48,
            Margin = new Thickness(0, 0, 16, 0)
        };

        var iconText = new TextBlock
        {
            Text = icon,
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        iconBorder.Child = iconText;
        content.Children.Add(iconBorder);

        var textPanel = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
        textPanel.Children.Add(new TextBlock
        {
            Text = title,
            FontSize = 16,
            FontWeight = FontWeights.SemiBold,
            Foreground = new SolidColorBrush(Colors.White)
        });
        textPanel.Children.Add(new TextBlock
        {
            Text = description,
            FontSize = 13,
            Foreground = new SolidColorBrush(ToColor("#9ca3af")),
            Margin = new Thickness(0, 4, 0, 0)
        });
        content.Children.Add(textPanel);

        var arrow = new TextBlock
        {
            Text = ">",
            FontSize = 24,
            Foreground = new SolidColorBrush(ToColor("#6b7280")),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 0, 16, 0)
        };
        content.Children.Add(arrow);

        card.Child = content;

        card.TouchDown += (s, e) => NavigateTo(tag);
        card.PointerPressed += (s, e) => NavigateTo(tag);

        return card;
    }

    private void ShowInkSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("墨迹设置", "配置墨迹书写行为"));
        _contentPanel.Children.Add(CreateSettingsCard("墨迹", new FrameworkElement[]
        {
            CreateNumberSetting("墨迹宽度", "新笔触的默认线条粗细", _settings.InkWidth, 0.5, 20, 0.5, v => _settings.InkWidth = v),
            CreateColorSetting("墨迹颜色", "默认墨迹颜色"),
            CreateToggleSetting("压力敏感度", "根据笔压力度调整线条粗细", true, v => { }),
            CreateToggleSetting("显示笔迹光标", "在画布上显示当前笔/橡皮位置", _settings.IsShowCursor, v => _settings.IsShowCursor = v),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("笔迹样式", new FrameworkElement[]
        {
            CreateComboSetting("笔迹渲染模式", "墨迹的渲染效果", new[] { "圆头", "平头", "书法", "毛笔" }, _settings.InkStyle, v => _settings.InkStyle = v),
            CreateToggleSetting("平滑处理", "对笔迹进行平滑处理", _settings.FitToCurve, v => _settings.FitToCurve = v),
        }));
    }

    private void ShowEraserSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("橡皮设置", "配置橡皮擦行为"));
        _contentPanel.Children.Add(CreateSettingsCard("橡皮", new FrameworkElement[]
        {
            CreateComboSetting("橡皮形状", "橡皮擦的形状", new[] { "圆形擦", "黑板擦" }, _settings.EraserShapeType, v => _settings.EraserShapeType = v),
            CreateComboSetting("橡皮模式", "橡皮擦除方式", new[] { "按笔画擦除", "按点擦除" }, 0, v => { }),
            CreateSliderSetting("橡皮大小", "橡皮擦的默认大小 (1-10)", _settings.EraserSize, 1, 10, 1, v => _settings.EraserSize = (int)v),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("高级", new FrameworkElement[]
        {
            CreateToggleSetting("擦除时显示预览", "显示将被擦除的区域", true, v => { }),
            CreateToggleSetting("精确擦除", "精确匹配笔画边缘", false, v => { }),
        }));
    }

    private void ShowHighlighterSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("高光设置", "配置高亮笔行为"));
        _contentPanel.Children.Add(CreateSettingsCard("高光", new FrameworkElement[]
        {
            CreateNumberSetting("高光宽度", "高亮笔的线条粗细", _settings.HighlighterWidth, 5, 100, 5, v => _settings.HighlighterWidth = v),
            CreateSliderSetting("高光透明度", "高亮笔的透明度", 50, 10, 100, 10, v => { }),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("行为", new FrameworkElement[]
        {
            CreateToggleSetting("高光叠加模式", "高光覆盖在其他墨迹上", true, v => { }),
        }));
    }

    private void ShowTouchSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("触控手势", "配置触控操作"));
        _contentPanel.Children.Add(CreateSettingsCard("多点触控", new FrameworkElement[]
        {
            CreateToggleSetting("启用多点触控", "启用多点触控操作", _settings.IsEnableMultiTouchMode, v => _settings.IsEnableMultiTouchMode = v),
            CreateToggleSetting("双指缩放", "使用双指缩放画布", _settings.IsEnableTwoFingerZoom, v => _settings.IsEnableTwoFingerZoom = v),
            CreateToggleSetting("双指移动", "使用双指移动画布", _settings.IsEnableTwoFingerTranslate, v => _settings.IsEnableTwoFingerTranslate = v),
            CreateToggleSetting("双指旋转", "使用双指旋转选区", _settings.IsEnableTwoFingerRotation, v => _settings.IsEnableTwoFingerRotation = v),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("捏合手势", new FrameworkElement[]
        {
            CreateToggleSetting("捏合缩放", "捏合手势缩放画布", true, v => { }),
            CreateSliderSetting("缩放范围", "允许的缩放比例", 100, 25, 400, 25, v => { }),
        }));
    }

    private void ShowGestureSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("笔势识别", "配置手写识别"));
        _contentPanel.Children.Add(CreateSettingsCard("预设手势", new FrameworkElement[]
        {
            CreateToggleSetting("直线", "绘制水平或垂直直线", true, v => { }),
            CreateToggleSetting("箭头", "绘制箭头符号", true, v => { }),
            CreateToggleSetting("圆形", "绘制圆形/椭圆", true, v => { }),
            CreateToggleSetting("三角形", "绘制三角形", false, v => { }),
            CreateToggleSetting("矩形", "绘制矩形", false, v => { }),
        }));
    }

    private void ShowThemeSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("主题", "配置应用外观"));
        _contentPanel.Children.Add(CreateSettingsCard("颜色模式", new FrameworkElement[]
        {
            CreateComboSetting("主题模式", "应用颜色主题", new[] { "跟随系统", "浅色", "深色" }, _settings.Theme, v => _settings.Theme = v),
            CreateToggleSetting("Windows 效果", "启用 Mica/Acrylic 效果", true, v => { }),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("浮动工具栏", new FrameworkElement[]
        {
            CreateSliderSetting("工具栏透明度", "浮动工具栏透明度", _settings.ViewboxFloatingBarOpacityValue * 100, 30, 100, 10, v => _settings.ViewboxFloatingBarOpacityValue = v / 100),
            CreateSliderSetting("工具栏缩放", "浮动工具栏缩放比例", _settings.ViewboxFloatingBarScaleTransformValue * 100, 50, 150, 10, v => _settings.ViewboxFloatingBarScaleTransformValue = v / 100),
        }));
    }

    private void ShowFontSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("字体", "配置文本显示"));
        _contentPanel.Children.Add(CreateSettingsCard("字体设置", new FrameworkElement[]
        {
            CreateComboSetting("界面字体", "应用界面使用的字体", new[] { "默认", "微软雅黑", "思源黑体", "霞鹜文楷" }, 0, v => { }),
            CreateSliderSetting("界面字号", "界面文字的大小", 14, 10, 24, 1, v => { }),
        }));
    }

    private void ShowSlideshowSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("放映设置", "配置PPT放映时的行为"));
        _contentPanel.Children.Add(CreateSettingsCard("放映模式", new FrameworkElement[]
        {
            CreateToggleSetting("启用墨迹标注", "在PPT放映时启用画布", _settings.PowerPointSupport, v => _settings.PowerPointSupport = v),
            CreateToggleSetting("显示页码", "在角落显示当前页码", _settings.IsShowPPTPageNumbers, v => _settings.IsShowPPTPageNumbers = v),
            CreateToggleSetting("显示翻页按钮", "在PPT放映时显示翻页按钮", _settings.ShowPPTButton, v => _settings.ShowPPTButton = v),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("显示", new FrameworkElement[]
        {
            CreateSliderSetting("标注透明度", "画布覆盖的透明度", _settings.ViewboxFloatingBarOpacityInPPTValue * 100, 30, 100, 10, v => _settings.ViewboxFloatingBarOpacityInPPTValue = v / 100),
        }));
    }

    private void ShowInkSaveSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("墨迹保存", "配置墨迹保存行为"));
        _contentPanel.Children.Add(CreateSettingsCard("保存", new FrameworkElement[]
        {
            CreateToggleSetting("自动保存墨迹", "退出PPT时自动保存墨迹", _settings.IsAutoSaveStrokesInPowerPoint, v => _settings.IsAutoSaveStrokesInPowerPoint = v),
            CreateToggleSetting("截图时保存墨迹", "截图时同时保存墨迹", _settings.IsAutoSaveStrokesAtScreenshot, v => _settings.IsAutoSaveStrokesAtScreenshot = v),
            CreateToggleSetting("清空时保存墨迹", "清空画布时自动保存", _settings.IsAutoSaveStrokesAtClear, v => _settings.IsAutoSaveStrokesAtClear = v),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("历史", new FrameworkElement[]
        {
            CreateToggleSetting("保留历史版本", "保存墨迹修改历史", true, v => { }),
            CreateSliderSetting("历史版本数", "保留的历史版本数量", 5, 1, 20, 1, v => { }),
        }));
    }

    private void ShowStartupSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("基本设置", "配置启动相关选项"));
        _contentPanel.Children.Add(CreateSettingsCard("启动", new FrameworkElement[]
        {
            CreateToggleSetting("开机自启", "开机时自动启动应用", _settings.IsAutoStartEnabled, v => _settings.IsAutoStartEnabled = v),
            CreateToggleSetting("启动后隐藏", "软件启动后自动隐藏工具栏", _settings.IsHideFloatingBarOnStart, v => _settings.IsHideFloatingBarOnStart = v),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("工具栏", new FrameworkElement[]
        {
            CreateToggleSetting("显示笔尖模式按钮", "在调色盘窗口中显示笔尖模式按钮", _settings.IsEnableDisPlayNibModeToggler, v => _settings.IsEnableDisPlayNibModeToggler = v),
            CreateToggleSetting("启用托盘图标", "在系统托盘显示图标", _settings.EnableTrayIcon, v => _settings.EnableTrayIcon = v),
        }));
    }

    private void ShowAutomationSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("自动化", "配置自动行为"));
        _contentPanel.Children.Add(CreateSettingsCard("自动折叠", new FrameworkElement[]
        {
            CreateToggleSetting("EasiNote中自动折叠", "检测到EasiNote时自动折叠", _settings.IsAutoFoldInEasiNote, v => _settings.IsAutoFoldInEasiNote = v),
            CreateToggleSetting("EasiCamera中自动折叠", "检测到EasiCamera时自动折叠", _settings.IsAutoFoldInEasiCamera, v => _settings.IsAutoFoldInEasiCamera = v),
            CreateToggleSetting("PPT放映中自动折叠", "进入PPT放映时自动折叠", _settings.IsAutoFoldInPPTSlideShow, v => _settings.IsAutoFoldInPPTSlideShow = v),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("自动保存", new FrameworkElement[]
        {
            CreateSliderSetting("最小墨迹数", "触发自动保存的最小墨迹数", _settings.MinimumAutomationStrokeNumber * 10, 1, 10, 1, v => _settings.MinimumAutomationStrokeNumber = v / 10),
            CreateNumberSetting("自动删除天数", "自动删除超过指定天数的文件", _settings.AutoDelSavedFilesDays, 1, 365, 1, v => _settings.AutoDelSavedFilesDays = (int)v),
        }));
    }

    private void ShowTouchAdvancedSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("触控设置", "配置高级触控参数"));
        _contentPanel.Children.Add(CreateSettingsCard("触控参数", new FrameworkElement[]
        {
            CreateSliderSetting("触摸乘数", "触摸输入的灵敏度", _settings.TouchMultiplier * 10, 1, 10, 1, v => _settings.TouchMultiplier = v / 10),
            CreateNumberSetting("手指模式边界宽度", "手指模式下的边界宽度", _settings.FingerModeBoundsWidth, 1, 100, 1, v => _settings.FingerModeBoundsWidth = (int)v),
            CreateNumberSetting("笔尖模式边界宽度", "笔尖模式下的边界宽度", _settings.NibModeBoundsWidth, 1, 100, 1, v => _settings.NibModeBoundsWidth = (int)v),
            CreateToggleSetting("橡皮绑定触摸乘数", "橡皮大小也受触摸乘数影响", _settings.EraserBindTouchMultiplier, v => _settings.EraserBindTouchMultiplier = v),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("笔尖模式", new FrameworkElement[]
        {
            CreateToggleSetting("启用笔尖模式", "自动切换到笔尖模式", _settings.IsEnableNibMode, v => _settings.IsEnableNibMode = v),
        }));
    }

    private void ShowSystemIntegrationSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("系统集成", "配置系统集成选项"));
        _contentPanel.Children.Add(CreateSettingsCard("窗口", new FrameworkElement[]
        {
            CreateComboSetting("窗口模式", "启动时的窗口模式", new[] { "最大化", "全屏" }, _settings.WindowMode, v => _settings.WindowMode = v),
            CreateToggleSetting("无焦点模式", "窗口不获取焦点", _settings.IsWindowNoActivate, v => _settings.IsWindowNoActivate = v),
            CreateToggleSetting("强制全屏", "始终保持全屏状态", _settings.IsEnableForceFullScreen, v => _settings.IsEnableForceFullScreen = v),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("系统检测", new FrameworkElement[]
        {
            CreateToggleSetting("检测分辨率变化", "检测显示器分辨率变化", _settings.IsEnableResolutionChangeDetection, v => _settings.IsEnableResolutionChangeDetection = v),
            CreateToggleSetting("检测DPI变化", "检测系统DPI缩放变化", _settings.IsEnableDPIChangeDetection, v => _settings.IsEnableDPIChangeDetection = v),
            CreateToggleSetting("禁用边缘手势", "禁用Windows边缘手势", _settings.IsEnableEdgeGestureUtil, v => _settings.IsEnableEdgeGestureUtil = v),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("确认", new FrameworkElement[]
        {
            CreateToggleSetting("关机确认", "关闭应用时显示确认对话框", _settings.IsSecondConfirmWhenShutdownApp, v => _settings.IsSecondConfirmWhenShutdownApp = v),
        }));
    }

    private void ShowAbout()
    {
        _contentPanel!.Children.Add(CreateHeader("关于", "应用信息"));

        var aboutCard = new Border
        {
            Background = new SolidColorBrush(ToColor("#2d2d2d")),
            CornerRadius = new CornerRadius(8),
            Padding = new Thickness(24),
            Margin = new Thickness(0, 0, 0, 16)
        };

        var aboutContent = new StackPanel();

        aboutContent.Children.Add(new TextBlock
        {
            Text = "ICC-Re",
            FontSize = 32,
            FontWeight = FontWeights.Bold,
            Foreground = new SolidColorBrush(Colors.White),
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 0, 0, 8)
        });

        aboutContent.Children.Add(new TextBlock
        {
            Text = "墨迹课堂",
            FontSize = 16,
            Foreground = new SolidColorBrush(ToColor("#60a5fa")),
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 0, 0, 16)
        });

        aboutContent.Children.Add(new TextBlock
        {
            Text = "版本 2.0.0",
            FontSize = 14,
            Foreground = new SolidColorBrush(ToColor("#9ca3af")),
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 0, 0, 8)
        });

        aboutContent.Children.Add(new TextBlock
        {
            Text = "基于 Jalium.UI 框架构建",
            FontSize = 12,
            Foreground = new SolidColorBrush(ToColor("#6b7280")),
            HorizontalAlignment = HorizontalAlignment.Center
        });

        aboutContent.Children.Add(new Border
        {
            Height = 1,
            Background = new SolidColorBrush(ToColor("#3f3f3f")),
            Margin = new Thickness(0, 24, 0, 24)
        });

        var infoGrid = new Grid();
        infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        AddInfoRow(infoGrid, "框架版本", "Jalium.UI 26.10.1", 0);
        AddInfoRow(infoGrid, ".NET 版本", ".NET 10.0", 1);
        AddInfoRow(infoGrid, "渲染后端", "DirectX 12", 2);
        AddInfoRow(infoGrid, "构建时间", DateTime.Now.ToString("yyyy-MM-dd"), 3);

        aboutContent.Children.Add(infoGrid);

        aboutContent.Children.Add(new Border
        {
            Height = 1,
            Background = new SolidColorBrush(ToColor("#3f3f3f")),
            Margin = new Thickness(0, 24, 0, 24)
        });

        var buttonPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var checkUpdateBtn = new Button
        {
            Content = "检查更新",
            Padding = new Thickness(24, 10, 24, 10),
            Background = new SolidColorBrush(ToColor("#60a5fa")),
            Foreground = new SolidColorBrush(Colors.White)
        };
        buttonPanel.Children.Add(checkUpdateBtn);

        var repoBtn = new Button
        {
            Content = "开源仓库",
            Padding = new Thickness(24, 10, 24, 10),
            Background = new SolidColorBrush(ToColor("#3f3f3f")),
            Foreground = new SolidColorBrush(Colors.White),
            Margin = new Thickness(12, 0, 0, 0)
        };
        buttonPanel.Children.Add(repoBtn);

        aboutContent.Children.Add(buttonPanel);

        aboutContent.Children.Add(new TextBlock
        {
            Text = "Copyright © 2024-2026 ICC-Re Team. All rights reserved.",
            FontSize = 11,
            Foreground = new SolidColorBrush(ToColor("#6b7280")),
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 24, 0, 0)
        });

        aboutCard.Child = aboutContent;
        _contentPanel.Children.Add(aboutCard);
    }

    private void AddInfoRow(Grid grid, string label, string value, int row)
    {
        grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        var labelBlock = new TextBlock
        {
            Text = label,
            Foreground = new SolidColorBrush(ToColor("#9ca3af")),
            FontSize = 13
        };
        var valueBlock = new TextBlock
        {
            Text = value,
            Foreground = new SolidColorBrush(Colors.White),
            FontSize = 13
        };
        Grid.SetRow(labelBlock, row);
        Grid.SetRow(valueBlock, row);
        Grid.SetColumn(labelBlock, 0);
        Grid.SetColumn(valueBlock, 1);
        grid.Children.Add(labelBlock);
        grid.Children.Add(valueBlock);
    }

    private StackPanel CreateHeader(string title, string subtitle)
    {
        var panel = new StackPanel { Margin = new Thickness(0, 0, 0, 24) };
        panel.Children.Add(new TextBlock
        {
            Text = title,
            FontSize = 28,
            FontWeight = FontWeights.Bold,
            Foreground = new SolidColorBrush(Colors.White),
            Margin = new Thickness(0, 0, 0, 8)
        });
        panel.Children.Add(new TextBlock
        {
            Text = subtitle,
            FontSize = 14,
            Foreground = new SolidColorBrush(ToColor("#9ca3af"))
        });
        return panel;
    }

    private Border CreateSettingsCard(string section, FrameworkElement[] settings)
    {
        var card = new Border
        {
            Background = new SolidColorBrush(ToColor("#2d2d2d")),
            CornerRadius = new CornerRadius(8),
            Padding = new Thickness(20),
            Margin = new Thickness(0, 0, 0, 16)
        };

        var panel = new StackPanel();

        panel.Children.Add(new TextBlock
        {
            Text = section,
            FontSize = 16,
            FontWeight = FontWeights.SemiBold,
            Foreground = new SolidColorBrush(ToColor("#60a5fa")),
            Margin = new Thickness(0, 0, 0, 16)
        });

        foreach (var setting in settings)
        {
            panel.Children.Add(setting);
        }

        card.Child = panel;
        return card;
    }

    private Grid CreateNumberSetting(string title, string description, double value, double min, double max, double step, Action<double> onChanged)
    {
        var grid = new Grid { Margin = new Thickness(0, 0, 0, 16), IsManipulationEnabled = true };
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });

        var textPanel = new StackPanel();
        textPanel.Children.Add(new TextBlock
        {
            Text = title,
            Foreground = new SolidColorBrush(Colors.White),
            FontSize = 14
        });
        textPanel.Children.Add(new TextBlock
        {
            Text = description,
            Foreground = new SolidColorBrush(ToColor("#9ca3af")),
            FontSize = 12
        });
        Grid.SetColumn(textPanel, 0);
        grid.Children.Add(textPanel);

        var numberBox = new NumberBox
        {
            Value = value,
            Minimum = min,
            Maximum = max,
            SmallChange = step,
            Width = 100,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        numberBox.ValueChanged += (s, e) => onChanged(e.NewValue);
        Grid.SetColumn(numberBox, 1);
        grid.Children.Add(numberBox);

        return grid;
    }

    private Grid CreateSliderSetting(string title, string description, double value, double min, double max, double step, Action<double> onChanged)
    {
        var grid = new Grid { Margin = new Thickness(0, 0, 0, 16), IsManipulationEnabled = true };
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });

        var textPanel = new StackPanel();
        textPanel.Children.Add(new TextBlock
        {
            Text = title,
            Foreground = new SolidColorBrush(Colors.White),
            FontSize = 14
        });
        textPanel.Children.Add(new TextBlock
        {
            Text = description,
            Foreground = new SolidColorBrush(ToColor("#9ca3af")),
            FontSize = 12
        });
        Grid.SetColumn(textPanel, 0);
        grid.Children.Add(textPanel);

        var sliderPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center };
        var slider = new Slider
        {
            Value = value,
            Minimum = min,
            Maximum = max,
            TickFrequency = step,
            IsSnapToTickEnabled = true,
            Width = 100
        };
        slider.ValueChanged += (s, e) =>
        {
            onChanged(e.NewValue);
            valueText.Text = $" {e.NewValue}%";
        };
        sliderPanel.Children.Add(slider);
        var valueText = new TextBlock
        {
            Text = $" {value}%",
            Foreground = new SolidColorBrush(ToColor("#9ca3af")),
            VerticalAlignment = VerticalAlignment.Center
        };
        sliderPanel.Children.Add(valueText);
        Grid.SetColumn(sliderPanel, 1);
        grid.Children.Add(sliderPanel);

        return grid;
    }

    private Grid CreateToggleSetting(string title, string description, bool isOn, Action<bool> onChanged)
    {
        var grid = new Grid { Margin = new Thickness(0, 0, 0, 16), IsManipulationEnabled = true };
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });

        var textPanel = new StackPanel();
        textPanel.Children.Add(new TextBlock
        {
            Text = title,
            Foreground = new SolidColorBrush(Colors.White),
            FontSize = 14
        });
        textPanel.Children.Add(new TextBlock
        {
            Text = description,
            Foreground = new SolidColorBrush(ToColor("#9ca3af")),
            FontSize = 12
        });
        Grid.SetColumn(textPanel, 0);
        grid.Children.Add(textPanel);

        var toggle = new ToggleSwitch { IsOn = isOn };
        toggle.Toggled += (s, e) => onChanged(toggle.IsOn);
        Grid.SetColumn(toggle, 1);
        grid.Children.Add(toggle);

        return grid;
    }

    private Grid CreateComboSetting(string title, string description, string[] items, int selectedIndex, Action<int> onChanged)
    {
        var grid = new Grid { Margin = new Thickness(0, 0, 0, 16), IsManipulationEnabled = true };
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });

        var textPanel = new StackPanel();
        textPanel.Children.Add(new TextBlock
        {
            Text = title,
            Foreground = new SolidColorBrush(Colors.White),
            FontSize = 14
        });
        textPanel.Children.Add(new TextBlock
        {
            Text = description,
            Foreground = new SolidColorBrush(ToColor("#9ca3af")),
            FontSize = 12
        });
        Grid.SetColumn(textPanel, 0);
        grid.Children.Add(textPanel);

        var comboBox = new ComboBox { MinWidth = 150, SelectedIndex = selectedIndex };
        foreach (var item in items)
        {
            comboBox.Items.Add(new ComboBoxItem { Content = item });
        }
        comboBox.SelectionChanged += (s, e) => onChanged(comboBox.SelectedIndex);
        Grid.SetColumn(comboBox, 1);
        grid.Children.Add(comboBox);

        return grid;
    }

    private Grid CreateColorSetting(string title, string description)
    {
        var grid = new Grid { Margin = new Thickness(0, 0, 0, 16), IsManipulationEnabled = true };
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });

        var textPanel = new StackPanel();
        textPanel.Children.Add(new TextBlock
        {
            Text = title,
            Foreground = new SolidColorBrush(Colors.White),
            FontSize = 14
        });
        textPanel.Children.Add(new TextBlock
        {
            Text = description,
            Foreground = new SolidColorBrush(ToColor("#9ca3af")),
            FontSize = 12
        });
        Grid.SetColumn(textPanel, 0);
        grid.Children.Add(textPanel);

        var colorPicker = new ColorPicker { Width = 100, HorizontalAlignment = HorizontalAlignment.Right };
        Grid.SetColumn(colorPicker, 1);
        grid.Children.Add(colorPicker);

        return grid;
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

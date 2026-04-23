using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Jalium.UI;
using Jalium.UI.Controls;
using Jalium.UI.Media;
using Jalium.UI.Markup;

namespace JaliumSettingsPreview;

public class Program
{
    private static NavigationView? _navigationView;
    private static StackPanel? _contentPanel;

    [STAThread]
    public static void Main(string[] args)
    {
        var app = new Application();

        var window = new Window
        {
            Title = "设置",
            Width = 1000,
            Height = 700,
            MinWidth = 800,
            MinHeight = 600,
            Background = new SolidColorBrush(ToColor("#202020"))
        };

        var grid = new Grid();
        var border = new Border
        {
            Background = new SolidColorBrush(ToColor("#202020")),
            CornerRadius = new CornerRadius(8)
        };

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

        border.Child = _navigationView;
        grid.Children.Add(border);
        window.Content = grid;

        NavigateTo("ink");

        app.Run(window);
    }

    private static void CreateMenuItems()
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
    }

    private static void CreateFooterMenuItems()
    {
        _navigationView!.FooterMenuItems.Add(new NavigationViewItem
        {
            Content = "关于",
            Icon = "Info",
            Tag = "about"
        });
    }

    private static void OnItemInvoked(object? sender, NavigationViewItemInvokedEventArgs e)
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

    private static void NavigateTo(string page)
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
            case "about":
                ShowAbout();
                break;
            default:
                ShowInkSettings();
                break;
        }
    }

    private static void ShowCanvasHub()
    {
        _contentPanel!.Children.Add(CreateHeader("画布", "选择画布相关的设置选项"));
        _contentPanel.Children.Add(CreateHubGrid(new[]
        {
            ("ink", "墨迹设置", "配置墨迹宽度、颜色、压力敏感度等", "✏️"),
            ("eraser", "橡皮设置", "配置橡皮模式、大小、擦除行为等", "🧽"),
            ("highlighter", "高光设置", "配置高亮笔的宽度，透明度和颜色", "🖍️")
        }));
    }

    private static void ShowGestureHub()
    {
        _contentPanel!.Children.Add(CreateHeader("手势", "选择手势相关的设置选项"));
        _contentPanel.Children.Add(CreateHubGrid(new[]
        {
            ("touch", "触控手势", "配置多点触控、双指缩放、移动等", "👆"),
            ("gesture", "笔势识别", "配置手写识别和预设手势", "✍️")
        }));
    }

    private static void ShowAppearanceHub()
    {
        _contentPanel!.Children.Add(CreateHeader("外观", "选择外观相关的设置选项"));
        _contentPanel.Children.Add(CreateHubGrid(new[]
        {
            ("theme", "主题", "配置颜色模式、Windows效果和自定义选项", "🎨"),
            ("font", "字体", "配置界面字体和墨迹文字识别", "🔤")
        }));
    }

    private static void ShowPowerPointHub()
    {
        _contentPanel!.Children.Add(CreateHeader("PowerPoint", "选择PowerPoint相关的设置选项"));
        _contentPanel.Children.Add(CreateHubGrid(new[]
        {
            ("slideshow", "放映设置", "配置PPT放映时的墨迹标注行为", "📽️"),
            ("inksave", "墨迹保存", "配置墨迹保存格式和历史版本管理", "💾")
        }));
    }

    private static StackPanel CreateHubGrid((string tag, string title, string description, string icon)[] items)
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

    private static Border CreateHubCard(string tag, string title, string description, string icon)
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

        card.TouchDown += (s, e) =>
        {
            NavigateTo(tag);
        };

        card.PointerPressed += (s, e) =>
        {
            NavigateTo(tag);
        };

        return card;
    }

    private static void ShowInkSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("墨迹设置", "配置墨迹书写行为"));
        _contentPanel.Children.Add(CreateSettingsCard("墨迹", new FrameworkElement[]
        {
            CreateNumberSetting("默认墨迹宽度", "新笔触的默认线条粗细", 2.5, 0.5, 20, 0.5),
            CreateColorSetting("墨迹颜色", "默认墨迹颜色"),
            CreateToggleSetting("压力敏感度", "根据笔压力度调整线条粗细", true),
            CreateToggleSetting("显示笔迹光标", "在画布上显示当前笔/橡皮位置", true),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("笔迹样式", new FrameworkElement[]
        {
            CreateComboSetting("笔迹渲染模式", "墨迹的渲染效果", new[] { "圆头", "平头", "书法", "毛笔" }, 0),
            CreateToggleSetting("平滑处理", "对笔迹进行平滑处理", true),
            CreateSliderSetting("笔迹压缩质量", "保存时墨迹的压缩比例", 80, 10, 100, 10),
        }));
    }

    private static void ShowEraserSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("橡皮设置", "配置橡皮擦行为"));
        _contentPanel.Children.Add(CreateSettingsCard("橡皮", new FrameworkElement[]
        {
            CreateComboSetting("橡皮模式", "橡皮擦除方式", new[] { "按笔画擦除", "按点擦除" }, 0),
            CreateSliderSetting("默认橡皮大小", "橡皮擦的默认大小", 1, 1, 10, 1),
            CreateToggleSetting("擦除时显示预览", "显示将被擦除的区域", true),
            CreateToggleSetting("橡皮覆盖模式", "橡皮只影响新绘制的墨迹", false),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("高级", new FrameworkElement[]
        {
            CreateToggleSetting("精确擦除", "精确匹配笔画边缘", false),
            CreateSliderSetting("擦除容差", "擦除时的容错范围", 5, 1, 20, 1),
        }));
    }

    private static void ShowHighlighterSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("高光设置", "配置高亮笔行为"));
        _contentPanel.Children.Add(CreateSettingsCard("高光", new FrameworkElement[]
        {
            CreateNumberSetting("高光宽度", "高亮笔的线条粗细", 20, 5, 100, 5),
            CreateSliderSetting("高光透明度", "高亮笔的透明度", 50, 10, 100, 10),
            CreateColorSetting("高光颜色", "高亮笔的默认颜色"),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("行为", new FrameworkElement[]
        {
            CreateToggleSetting("高光叠加模式", "高光覆盖在其他墨迹上", true),
            CreateToggleSetting("高光水印效果", "高光呈现水印样式", false),
        }));
    }

    private static void ShowTouchSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("触控手势", "配置触控操作"));
        _contentPanel.Children.Add(CreateSettingsCard("多点触控", new FrameworkElement[]
        {
            CreateToggleSetting("启用多点触控", "启用多点触控操作", true),
            CreateToggleSetting("双指缩放", "使用双指缩放画布", false),
            CreateToggleSetting("双指移动", "使用双指移动画布", true),
            CreateToggleSetting("双指旋转", "使用双指旋转选区", false),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("捏合手势", new FrameworkElement[]
        {
            CreateToggleSetting("捏合缩放", "捏合手势缩放画布", true),
            CreateSliderSetting("缩放范围", "允许的缩放比例", 50, 25, 400, 25),
            CreateToggleSetting("缩放平滑过渡", "缩放时使用平滑动画", true),
        }));
    }

    private static void ShowGestureSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("笔势识别", "配置手写识别"));
        _contentPanel.Children.Add(CreateSettingsCard("识别", new FrameworkElement[]
        {
            CreateToggleSetting("启用笔势识别", "识别手写手势", true),
            CreateComboSetting("识别语言", "手势识别的语言", new[] { "简体中文", "繁体中文", "英文", "混合" }, 0),
            CreateSliderSetting("识别灵敏度", "手势识别的灵敏度", 70, 30, 100, 10),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("预设手势", new FrameworkElement[]
        {
            CreateToggleSetting("直线", "绘制水平或垂直直线", true),
            CreateToggleSetting("箭头", "绘制箭头符号", true),
            CreateToggleSetting("圆形", "绘制圆形/椭圆", true),
            CreateToggleSetting("三角形", "绘制三角形", false),
            CreateToggleSetting("矩形", "绘制矩形", false),
        }));
    }

    private static void ShowThemeSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("主题", "配置应用外观"));
        _contentPanel.Children.Add(CreateSettingsCard("颜色模式", new FrameworkElement[]
        {
            CreateComboSetting("主题模式", "应用颜色主题", new[] { "跟随系统", "浅色", "深色" }, 0),
            CreateToggleSetting("Windows 效果", "启用 Mica/Acrylic 效果", true),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("自定义", new FrameworkElement[]
        {
            CreateColorSetting("强调色", "应用强调颜色"),
            CreateToggleSetting("透明模式", "窗口背景透明", false),
            CreateSliderSetting("背景模糊度", "毛玻璃效果模糊度", 30, 0, 100, 10),
        }));
    }

    private static void ShowFontSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("字体", "配置文本显示"));
        _contentPanel.Children.Add(CreateSettingsCard("字体设置", new FrameworkElement[]
        {
            CreateComboSetting("界面字体", "应用界面使用的字体", new[] { "默认", "微软雅黑", "思源黑体", "霞鹜文楷" }, 0),
            CreateSliderSetting("界面字号", "界面文字的大小", 14, 10, 24, 1),
            CreateToggleSetting("粗体界面文字", "界面文字使用粗体", false),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("墨迹文字识别", new FrameworkElement[]
        {
            CreateToggleSetting("手写文字识别", "将手写文字转换为文本", true),
            CreateComboSetting("识别字体", "识别结果的默认字体", new[] { "默认", "楷体", "宋体", "黑体" }, 0),
        }));
    }

    private static void ShowSlideshowSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("放映设置", "配置PPT放映时的行为"));
        _contentPanel.Children.Add(CreateSettingsCard("放映模式", new FrameworkElement[]
        {
            CreateToggleSetting("启用墨迹标注", "在PPT放映时启用画布", true),
            CreateToggleSetting("独立窗口", "在单独窗口中显示标注", false),
            CreateToggleSetting("全屏模式", "标注画布全屏显示", false),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("显示", new FrameworkElement[]
        {
            CreateToggleSetting("显示页码", "在角落显示当前页码", true),
            CreateToggleSetting("显示进度条", "显示演示进度", true),
            CreateSliderSetting("标注透明度", "画布覆盖的透明度", 80, 30, 100, 10),
        }));
    }

    private static void ShowInkSaveSettings()
    {
        _contentPanel!.Children.Add(CreateHeader("墨迹保存", "配置墨迹保存行为"));
        _contentPanel.Children.Add(CreateSettingsCard("保存", new FrameworkElement[]
        {
            CreateToggleSetting("自动保存墨迹", "退出PPT时自动保存墨迹", true),
            CreateComboSetting("保存格式", "墨迹保存的文件格式", new[] { "ICC墨迹格式", "PNG图片", "SVG矢量", "PDF" }, 0),
            CreateToggleSetting("保存到PPT", "将墨迹嵌入到PPT文件中", true),
        }));
        _contentPanel.Children.Add(CreateSettingsCard("历史", new FrameworkElement[]
        {
            CreateToggleSetting("保留历史版本", "保存墨迹修改历史", true),
            CreateSliderSetting("历史版本数", "保留的历史版本数量", 5, 1, 20, 1),
            CreateToggleSetting("自动清理旧版本", "自动清理超过30天的历史", false),
        }));
    }

    private static void ShowAbout()
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
            Text = "版本 1.0.0",
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

        AddInfoRow(infoGrid, "框架版本", "Jalium.UI 26.10.0", 0);
        AddInfoRow(infoGrid, ".NET 版本", ".NET 10.0", 1);
        AddInfoRow(infoGrid, "渲染后端", "DirectX 12", 2);
        AddInfoRow(infoGrid, "构建时间", "2026-04-23", 3);

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

    private static void AddInfoRow(Grid grid, string label, string value, int row)
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

    private static StackPanel CreateHeader(string title, string subtitle)
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

    private static Border CreateSettingsCard(string section, FrameworkElement[] settings)
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

    private static Grid CreateNumberSetting(string title, string description, double value, double min, double max, double step)
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
        Grid.SetColumn(numberBox, 1);
        grid.Children.Add(numberBox);

        return grid;
    }

    private static Grid CreateSliderSetting(string title, string description, double value, double min, double max, double step)
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
        sliderPanel.Children.Add(slider);
        sliderPanel.Children.Add(new TextBlock
        {
            Text = $" {value}%",
            Foreground = new SolidColorBrush(ToColor("#9ca3af")),
            VerticalAlignment = VerticalAlignment.Center
        });
        Grid.SetColumn(sliderPanel, 1);
        grid.Children.Add(sliderPanel);

        return grid;
    }

    private static Grid CreateToggleSetting(string title, string description, bool isOn)
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
        Grid.SetColumn(toggle, 1);
        grid.Children.Add(toggle);

        return grid;
    }

    private static Grid CreateComboSetting(string title, string description, string[] items, int selectedIndex)
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
        Grid.SetColumn(comboBox, 1);
        grid.Children.Add(comboBox);

        return grid;
    }

    private static Grid CreateColorSetting(string title, string description)
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

using Jalium.UI.Controls;
using Jalium.UI.Media;

namespace InkCanvasForClass_Remastered.JaliumPreview;

public class JaliumSettingsApp
{
    public static void Run()
    {
        var app = new Application();

        var window = new Window
        {
            Title = "ICC-Re 设置 - Jalium Preview",
            Width = 800,
            Height = 600,
            Content = CreateSettingsContent()
        };

        app.Run(window);
    }

    private static StackPanel CreateSettingsContent()
    {
        var panel = new StackPanel
        {
            Margin = new Thickness(24),
            Children =
            {
                new TextBlock
                {
                    Text = "ICC-Re 设置",
                    FontSize = 28,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 24)
                },
                CreateSettingsSection("画布设置", new[]
                {
                    ("墨迹宽度", "设置绘制线条的粗细"),
                    ("高光宽度", "高亮笔的宽度"),
                    ("橡皮大小", "橡皮擦的大小"),
                    ("笔迹样式", "不同的笔迹渲染效果")
                }),
                CreateSettingsSection("手势设置", new[]
                {
                    ("多点触控", "启用多点触控操作"),
                    ("双指缩放", "使用双指缩放画布"),
                    ("双指移动", "使用双指移动画布"),
                    ("双指旋转", "使用双指旋转选区")
                }),
                CreateSettingsSection("外观设置", new[]
                {
                    ("主题", "浅色/深色/跟随系统"),
                    ("透明度", "浮动工具栏透明度")
                })
            }
        };

        return panel;
    }

    private static StackPanel CreateSettingsSection(string title, (string name, string desc)[] items)
    {
        var section = new StackPanel
        {
            Margin = new Thickness(0, 16, 0, 0)
        };

        section.Children.Add(new TextBlock
        {
            Text = title,
            FontSize = 16,
            FontWeight = FontWeights.SemiBold,
            Margin = new Thickness(0, 0, 0, 8)
        });

        foreach (var (name, desc) in items)
        {
            section.Children.Add(CreateSettingsItem(name, desc));
        }

        return section;
    }

    private static Grid CreateSettingsItem(string name, string description)
    {
        var grid = new Grid
        {
            Margin = new Thickness(0, 8, 0, 8),
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(120) }
            }
        };

        var textPanel = new StackPanel { Orientation = Orientation.Vertical };
        textPanel.Children.Add(new TextBlock
        {
            Text = name,
            FontSize = 14
        });
        textPanel.Children.Add(new TextBlock
        {
            Text = description,
            FontSize = 12,
            Foreground = new SolidColorBrush(Colors.Gray)
        });

        var toggle = new ToggleSwitch { IsOn = true };

        Grid.SetColumn(textPanel, 0);
        Grid.SetColumn(toggle, 1);

        grid.Children.Add(textPanel);
        grid.Children.Add(toggle);

        return grid;
    }
}

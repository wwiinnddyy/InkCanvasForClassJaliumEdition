# Jalium UI 浮动工具栏使用指南

## 文件结构

```
InkCanvasForClass-Remastered/
├── JaliumControls/
│   ├── JaliumMainToolbar.cs          # 完整的 Jalium UI 浮动工具栏
│   ├── JaliumFloatingToolBar.cs      # 简化版工具栏
│   └── JaliumSettingsTemplate.jalxaml # 设置窗口模板
└── Controls/
    └── JaliumFloatingBar.xaml       # WPF UserControl 版本工具栏
```

## 快速开始

### 方式一：使用 JaliumMainToolbar（推荐）

```csharp
using InkCanvasForClass_Remastered.JaliumControls;

// 在 MainWindow.xaml.cs 中

private JaliumMainToolbar? _jaliumToolbar;

private void InitializeJaliumToolbar()
{
    _jaliumToolbar = new JaliumMainToolbar();

    // 订阅工具选择事件
    _jaliumToolbar.ToolSelected += (s, toolType) =>
    {
        switch (toolType)
        {
            case ToolType.Cursor:
                // 切换到光标模式
                break;
            case ToolType.Pen:
                // 切换到笔模式
                break;
            case ToolType.Highlighter:
                // 切换到荧光笔模式
                break;
            case ToolType.Eraser:
                // 切换到橡皮模式
                break;
            case ToolType.Gesture:
                // 切换到手势模式
                break;
            case ToolType.Clear:
                // 清空画布
                ClearCanvas();
                break;
            case ToolType.Settings:
                // 打开设置
                OpenSettings();
                break;
        }
    };

    // 订阅设置变更事件
    _jaliumToolbar.PenWidthChanged += (s, width) =>
    {
        // 更新笔宽度
        Settings.InkWidth = width;
    };

    _jaliumToolbar.PenColorChanged += (s, colorName) =>
    {
        // 更新笔颜色
        SetPenColor(colorName);
    };

    // 显示工具栏
    _jaliumToolbar.Show();
}
```

### 方式二：使用 WPF UserControl 版本

```csharp
// 在 MainWindow.xaml 中添加命名空间
xmlns:jalium="clr-namespace:InkCanvasForClass_Remastered.Controls"

// 添加控件
<jalium:JaliumFloatingBar
    x:Name="JaliumFloatingBarControl"
    PenSelected="JaliumFloatingBar_PenSelected"
    HighlighterSelected="JaliumFloatingBar_HighlighterSelected"
    EraserSelected="JaliumFloatingBar_EraserSelected"
    ClearSelected="JaliumFloatingBar_ClearSelected"
    SettingsSelected="JaliumFloatingBar_SettingsSelected"
    CursorSelected="JaliumFloatingBar_CursorSelected"
    PenColorChanged="JaliumFloatingBar_PenColorChanged"
    PenWidthChanged="JaliumFloatingBar_PenWidthChanged"/>
```

## JaliumMainToolbar 事件列表

| 事件 | 说明 | 参数 |
|------|------|------|
| `ToolSelected` | 工具被选中 | `ToolType` 枚举 |
| `PenWidthChanged` | 笔宽度变更 | `double` |
| `PenAlphaChanged` | 笔透明度变更 | `double` |
| `PenColorChanged` | 笔颜色变更 | `string` (颜色名称) |
| `HighlighterWidthChanged` | 荧光笔宽度变更 | `double` |
| `EraserSizeChanged` | 橡皮大小变更 | `double` |
| `MultiTouchChanged` | 多指书写开关变更 | `bool` |
| `TwoFingerZoomChanged` | 双指缩放开关变更 | `bool` |
| `TwoFingerTranslateChanged` | 双指移动开关变更 | `bool` |
| `TwoFingerRotateChanged` | 双指旋转开关变更 | `bool` |

## ToolType 枚举

```csharp
public enum ToolType
{
    Cursor,      // 鼠标/光标模式
    Pen,         // 批注/笔模式
    Highlighter, // 荧光笔模式
    Eraser,      // 橡皮模式
    Gesture,     // 手势模式
    Clear,       // 清空画布
    Settings     // 设置
}
```

## 属性列表

| 属性 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| `PenWidth` | `double` | 5.0 | 笔宽度 |
| `PenAlpha` | `double` | 255.0 | 笔透明度 |
| `PenColor` | `string` | "Black" | 笔颜色名称 |
| `HighlighterWidth` | `double` | 20.0 | 荧光笔宽度 |
| `EraserSize` | `double` | 3.0 | 橡皮大小 |

## 颜色名称

支持以下颜色名称：
- `Black` - 黑色
- `White` - 白色
- `Red` - 红色
- `Yellow` - 黄色
- `Green` - 绿色
- `Blue` - 蓝色

## 方法列表

| 方法 | 说明 |
|------|------|
| `Initialize()` | 初始化工具栏窗口 |
| `Show()` | 显示工具栏窗口 |
| `Close()` | 关闭工具栏窗口 |
| `Hide()` | 隐藏工具栏窗口 |
| `ShowToolBar()` | 如果工具栏被隐藏则显示 |

## 注意事项

1. **Jalium UI 独立窗口**：`JaliumMainToolbar` 创建的是一个独立的 `Jalium.UI.Application` 和 `Window`，它与主 WPF 应用程序的消息循环是分开的。

2. **WPF 版本限制**：由于 Jalium UI 的控件可能与 WPF 的原生控件有冲突，建议将 `JaliumFloatingBar.xaml` 作为独立的用户控件使用，而不是在同一个 Application 中混合使用。

3. **窗口定位**：`JaliumMainToolbar` 的窗口默认位置是 `Margin = new Thickness(100, 5, 0, 0)`，可以根据需要调整。

## 未来计划

- [ ] 将 `JaliumMainToolbar` 集成到 `MainWindow`
- [ ] 替换现有的 `ViewboxFloatingBar` 和相关二级菜单
- [ ] 实现 `.jalxaml` 格式的声明式 UI
- [ ] 添加主题支持（深色/浅色模式）
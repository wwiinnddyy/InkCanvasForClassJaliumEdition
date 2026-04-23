using Jalium.UI.Controls;
using Jalium.UI.Markup;

namespace InkCanvasForClass_Remastered.JaliumPreview;

public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var app = new Application();
        app.Style = ApplicationStyle.Dark;

        var xaml = """
<Window xmlns="https://jalium.dev/ui"
        Title="ICC-Re 设置 - Jalium Preview"
        Width="800"
        Height="700"
        Background="#1e1e1e">
    <ScrollViewer>
        <Grid Margin="24">
            <StackPanel>
                <!-- Header -->
                <TextBlock Text="ICC-Re 设置"
                           FontSize="28"
                           FontWeight="Bold"
                           Foreground="White"
                           Margin="0,0,0,24"/>

                <!-- 画布设置 -->
                <StackPanel Margin="0,16,0,0">
                    <TextBlock Text="画布设置"
                               FontSize="16"
                               FontWeight="SemiBold"
                               Foreground="#60a5fa"
                               Margin="0,0,0,12"/>

                    <Border Background="#2d2d2d" CornerRadius="8" Padding="16" Margin="0,0,0,8">
                        <StackPanel>
                            <Grid Margin="0,4,0,12">
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="100"/>
                                </Grid.ColumnDefinitions>
                                <StackPanel Grid.Column="0">
                                    <TextBlock Text="墨迹宽度" Foreground="White" FontSize="14"/>
                                    <TextBlock Text="设置绘制线条的粗细" Foreground="#9ca3af" FontSize="12"/>
                                </StackPanel>
                                <NumberBox Grid.Column="1" Value="2.5" Minimum="0.5" Maximum="20" SmallChange="0.5"/>
                            </Grid>

                            <Grid Margin="0,4,0,12">
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="100"/>
                                </Grid.ColumnDefinitions>
                                <StackPanel Grid.Column="0">
                                    <TextBlock Text="高光宽度" Foreground="White" FontSize="14"/>
                                    <TextBlock Text="高亮笔的宽度" Foreground="#9ca3af" FontSize="12"/>
                                </StackPanel>
                                <NumberBox Grid.Column="1" Value="20" Minimum="5" Maximum="100" SmallChange="5"/>
                            </Grid>

                            <Grid Margin="0,4,0,12">
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="100"/>
                                </Grid.ColumnDefinitions>
                                <StackPanel Grid.Column="0">
                                    <TextBlock Text="橡皮大小" Foreground="White" FontSize="14"/>
                                    <TextBlock Text="橡皮擦的大小 (1-10)" Foreground="#9ca3af" FontSize="12"/>
                                </StackPanel>
                                <Slider Grid.Column="1" Value="1" Minimum="1" Maximum="10" TickFrequency="1" IsSnapToTickEnabled="True" VerticalAlignment="Center"/>
                            </Grid>

                            <Grid Margin="0,4,0,4">
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="Auto"/>
                                </Grid.ColumnDefinitions>
                                <StackPanel Grid.Column="0">
                                    <TextBlock Text="显示光标" Foreground="White" FontSize="14"/>
                                    <TextBlock Text="在画布上显示笔迹光标" Foreground="#9ca3af" FontSize="12"/>
                                </StackPanel>
                                <ToggleSwitch Grid.Column="1" IsOn="True"/>
                            </Grid>
                        </StackPanel>
                    </Border>
                </StackPanel>

                <!-- 手势设置 -->
                <StackPanel Margin="0,16,0,0">
                    <TextBlock Text="手势设置"
                               FontSize="16"
                               FontWeight="SemiBold"
                               Foreground="#60a5fa"
                               Margin="0,0,0,12"/>

                    <Border Background="#2d2d2d" CornerRadius="8" Padding="16" Margin="0,0,0,8">
                        <StackPanel>
                            <Grid Margin="0,4,0,12">
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="Auto"/>
                                </Grid.ColumnDefinitions>
                                <StackPanel Grid.Column="0">
                                    <TextBlock Text="多点触控" Foreground="White" FontSize="14"/>
                                    <TextBlock Text="启用多点触控操作" Foreground="#9ca3af" FontSize="12"/>
                                </StackPanel>
                                <ToggleSwitch Grid.Column="1" IsOn="True"/>
                            </Grid>

                            <Grid Margin="0,4,0,12">
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="Auto"/>
                                </Grid.ColumnDefinitions>
                                <StackPanel Grid.Column="0">
                                    <TextBlock Text="双指缩放" Foreground="White" FontSize="14"/>
                                    <TextBlock Text="使用双指缩放画布" Foreground="#9ca3af" FontSize="12"/>
                                </StackPanel>
                                <ToggleSwitch Grid.Column="1" IsOn="False"/>
                            </Grid>

                            <Grid Margin="0,4,0,12">
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="Auto"/>
                                </Grid.ColumnDefinitions>
                                <StackPanel Grid.Column="0">
                                    <TextBlock Text="双指移动" Foreground="White" FontSize="14"/>
                                    <TextBlock Text="使用双指移动画布" Foreground="#9ca3af" FontSize="12"/>
                                </StackPanel>
                                <ToggleSwitch Grid.Column="1" IsOn="True"/>
                            </Grid>

                            <Grid Margin="0,4,0,4">
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="Auto"/>
                                </Grid.ColumnDefinitions>
                                <StackPanel Grid.Column="0">
                                    <TextBlock Text="双指旋转" Foreground="White" FontSize="14"/>
                                    <TextBlock Text="使用双指旋转选区" Foreground="#9ca3af" FontSize="12"/>
                                </StackPanel>
                                <ToggleSwitch Grid.Column="1" IsOn="False"/>
                            </Grid>
                        </StackPanel>
                    </Border>
                </StackPanel>

                <!-- 外观设置 -->
                <StackPanel Margin="0,16,0,0">
                    <TextBlock Text="外观设置"
                               FontSize="16"
                               FontWeight="SemiBold"
                               Foreground="#60a5fa"
                               Margin="0,0,0,12"/>

                    <Border Background="#2d2d2d" CornerRadius="8" Padding="16" Margin="0,0,0,8">
                        <StackPanel>
                            <Grid Margin="0,4,0,12">
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="120"/>
                                </Grid.ColumnDefinitions>
                                <StackPanel Grid.Column="0">
                                    <TextBlock Text="主题" Foreground="White" FontSize="14"/>
                                    <TextBlock Text="应用外观主题" Foreground="#9ca3af" FontSize="12"/>
                                </StackPanel>
                                <ComboBox Grid.Column="1" SelectedIndex="0" MinWidth="100">
                                    <ComboBoxItem Content="跟随系统"/>
                                    <ComboBoxItem Content="浅色"/>
                                    <ComboBoxItem Content="深色"/>
                                </ComboBox>
                            </Grid>

                            <Grid Margin="0,4,0,4">
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="120"/>
                                </Grid.ColumnDefinitions>
                                <StackPanel Grid.Column="0">
                                    <TextBlock Text="工具栏透明度" Foreground="White" FontSize="14"/>
                                    <TextBlock Text="浮动工具栏透明度" Foreground="#9ca3af" FontSize="12"/>
                                </StackPanel>
                                <Slider Grid.Column="1" Value="100" Minimum="30" Maximum="100" TickFrequency="10" IsSnapToTickEnabled="True" VerticalAlignment="Center"/>
                            </Grid>
                        </StackPanel>
                    </Border>
                </StackPanel>

                <!-- PowerPoint 设置 -->
                <StackPanel Margin="0,16,0,0">
                    <TextBlock Text="PowerPoint 设置"
                               FontSize="16"
                               FontWeight="SemiBold"
                               Foreground="#60a5fa"
                               Margin="0,0,0,12"/>

                    <Border Background="#2d2d2d" CornerRadius="8" Padding="16" Margin="0,0,0,8">
                        <StackPanel>
                            <Grid Margin="0,4,0,12">
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="Auto"/>
                                </Grid.ColumnDefinitions>
                                <StackPanel Grid.Column="0">
                                    <TextBlock Text="PowerPoint 支持" Foreground="White" FontSize="14"/>
                                    <TextBlock Text="在PPT放映时启用墨迹标注" Foreground="#9ca3af" FontSize="12"/>
                                </StackPanel>
                                <ToggleSwitch Grid.Column="1" IsOn="True"/>
                            </Grid>

                            <Grid Margin="0,4,0,12">
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="Auto"/>
                                </Grid.ColumnDefinitions>
                                <StackPanel Grid.Column="0">
                                    <TextBlock Text="自动保存墨迹" Foreground="White" FontSize="14"/>
                                    <TextBlock Text="退出PPT时自动保存墨迹" Foreground="#9ca3af" FontSize="12"/>
                                </StackPanel>
                                <ToggleSwitch Grid.Column="1" IsOn="True"/>
                            </Grid>

                            <Grid Margin="0,4,0,4">
                                <Grid.ColumnDefinitions>
                                    <ColumnDefinition Width="*"/>
                                    <ColumnDefinition Width="Auto"/>
                                </Grid.ColumnDefinitions>
                                <StackPanel Grid.Column="0">
                                    <TextBlock Text="显示页码" Foreground="White" FontSize="14"/>
                                    <TextBlock Text="在PPT放映时显示当前页码" Foreground="#9ca3af" FontSize="12"/>
                                </StackPanel>
                                <ToggleSwitch Grid.Column="1" IsOn="True"/>
                            </Grid>
                        </StackPanel>
                    </Border>
                </StackPanel>

                <!-- 底部按钮 -->
                <StackPanel Orientation="Orientation.Horizontal" HorizontalAlignment="Right" Margin="0,32,0,0">
                    <Button Content="取消" Padding="24,10" Margin="0,0,12,0" Background="#3f3f3f" Foreground="White"/>
                    <Button Content="保存" Padding="24,10" Background="#60a5fa" Foreground="White"/>
                </StackPanel>
            </StackPanel>
        </Grid>
    </ScrollViewer>
</Window>
""";

        var window = (Window)XamlReader.Parse(xaml);
        app.Run(window);
    }
}

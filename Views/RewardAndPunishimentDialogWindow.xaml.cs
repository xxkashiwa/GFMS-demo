using GFMS.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Windowing;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GFMS.Views
{
    /// <summary>
    /// 奖惩详细信息窗口
    /// </summary>
    public sealed partial class RewardAndPunishimentDialogWindow : Window
    {
        private Student _student;
        private ObservableCollection<RewardAndPunishmentDisplayItem> _rewardPunishmentDisplayItems;

        public string StudentName => $"学生：{_student?.Name} ({_student?.StudentId}) - 奖惩详细";

        public RewardAndPunishimentDialogWindow(Student student)
        {
            InitializeComponent();
            _student = student;
            StudentNameTextBlock.Text = StudentName;
            _rewardPunishmentDisplayItems = new ObservableCollection<RewardAndPunishmentDisplayItem>();
            ExtendsContentIntoTitleBar = true;

            // 设置窗口大小，使其比父窗口小
            SetWindowSize();

            LoadRewardPunishmentData();

            RewardsAndPunishmentsListView.ItemsSource = _rewardPunishmentDisplayItems;
        }

        private void SetWindowSize()
        {
            try
            {
                // 获取主窗口的尺寸
                var mainWindow = ((App)Application.Current)?.MainWindow;
                if (mainWindow?.AppWindow != null)
                {
                    var mainWindowSize = mainWindow.AppWindow.Size;

                    // 设置对话框为主窗口的80%大小
                    int dialogWidth = (int)(mainWindowSize.Width * 0.8);
                    int dialogHeight = (int)(mainWindowSize.Height * 0.8);

                    // 确保最小尺寸
                    dialogWidth = Math.Max(dialogWidth, 600);
                    dialogHeight = Math.Max(dialogHeight, 500);

                    // 设置窗口大小
                    AppWindow.Resize(new Windows.Graphics.SizeInt32(dialogWidth, dialogHeight));

                    // 居中显示
                    var displayArea = DisplayArea.GetFromWindowId(mainWindow.AppWindow.Id, DisplayAreaFallback.Primary);
                    if (displayArea != null)
                    {
                        var centerX = (displayArea.WorkArea.Width - dialogWidth) / 2;
                        var centerY = (displayArea.WorkArea.Height - dialogHeight) / 2;
                        AppWindow.Move(new Windows.Graphics.PointInt32(centerX, centerY));
                    }
                }
                else
                {
                    // 如果无法获取主窗口尺寸，使用默认尺寸
                    AppWindow.Resize(new Windows.Graphics.SizeInt32(800, 600));
                }
            }
            catch
            {
                // 出现异常时使用默认尺寸
                AppWindow.Resize(new Windows.Graphics.SizeInt32(800, 600));
            }
        }

        private void LoadRewardPunishmentData()
        {
            _rewardPunishmentDisplayItems.Clear();

            // 将学生的奖惩数据转换为显示项
            foreach (var rewardPunishment in _student.RewardsAndPunishments)
            {
                _rewardPunishmentDisplayItems.Add(new RewardAndPunishmentDisplayItem
                {
                    Type = rewardPunishment.Type.ToString(),
                    Details = rewardPunishment.Details,
                    Date = rewardPunishment.Date
                });
            }
        }

        private void AddRewardPunishmentButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 实现添加奖惩记录的逻辑
            // 目前先显示占位信息
            var dialog = new ContentDialog
            {
                Title = "添加奖惩记录",
                Content = "添加奖惩记录功能待实现",
                PrimaryButtonText = "确定",
                XamlRoot = this.Content.XamlRoot
            };

            _ = dialog.ShowAsync();
        }
    }
}

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
    /// ������ϸ��Ϣ����
    /// </summary>
    public sealed partial class RewardAndPunishimentDialogWindow : Window
    {
        private Student _student;
        private ObservableCollection<RewardAndPunishmentDisplayItem> _rewardPunishmentDisplayItems;

        public string StudentName => $"ѧ����{_student?.Name} ({_student?.StudentId}) - ������ϸ";

        public RewardAndPunishimentDialogWindow(Student student)
        {
            InitializeComponent();
            _student = student;
            StudentNameTextBlock.Text = StudentName;
            _rewardPunishmentDisplayItems = new ObservableCollection<RewardAndPunishmentDisplayItem>();
            ExtendsContentIntoTitleBar = true;

            // ���ô��ڴ�С��ʹ��ȸ�����С
            SetWindowSize();

            LoadRewardPunishmentData();

            RewardsAndPunishmentsListView.ItemsSource = _rewardPunishmentDisplayItems;
        }

        private void SetWindowSize()
        {
            try
            {
                // ��ȡ�����ڵĳߴ�
                var mainWindow = ((App)Application.Current)?.MainWindow;
                if (mainWindow?.AppWindow != null)
                {
                    var mainWindowSize = mainWindow.AppWindow.Size;

                    // ���öԻ���Ϊ�����ڵ�80%��С
                    int dialogWidth = (int)(mainWindowSize.Width * 0.8);
                    int dialogHeight = (int)(mainWindowSize.Height * 0.8);

                    // ȷ����С�ߴ�
                    dialogWidth = Math.Max(dialogWidth, 600);
                    dialogHeight = Math.Max(dialogHeight, 500);

                    // ���ô��ڴ�С
                    AppWindow.Resize(new Windows.Graphics.SizeInt32(dialogWidth, dialogHeight));

                    // ������ʾ
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
                    // ����޷���ȡ�����ڳߴ磬ʹ��Ĭ�ϳߴ�
                    AppWindow.Resize(new Windows.Graphics.SizeInt32(800, 600));
                }
            }
            catch
            {
                // �����쳣ʱʹ��Ĭ�ϳߴ�
                AppWindow.Resize(new Windows.Graphics.SizeInt32(800, 600));
            }
        }

        private void LoadRewardPunishmentData()
        {
            _rewardPunishmentDisplayItems.Clear();

            // ��ѧ���Ľ�������ת��Ϊ��ʾ��
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
            // TODO: ʵ����ӽ��ͼ�¼���߼�
            // Ŀǰ����ʾռλ��Ϣ
            var dialog = new ContentDialog
            {
                Title = "��ӽ��ͼ�¼",
                Content = "��ӽ��ͼ�¼���ܴ�ʵ��",
                PrimaryButtonText = "ȷ��",
                XamlRoot = this.Content.XamlRoot
            };

            _ = dialog.ShowAsync();
        }
    }
}

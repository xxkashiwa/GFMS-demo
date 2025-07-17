using GFMS.Models;
using GFMS.Services;
using GFMS.Views;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GFMS.Pages
{
    /// <summary>
    /// 数据收集页面，用于学生相关信息录入和管理
    /// </summary>
    public sealed partial class DataCollectionPage : Page
    {
        public DataCollectionPage()
        {
            InitializeComponent();

            // 设置ListView的数据源为StudentManager单例的Students集合
            StudentsListView.ItemsSource = StudentManager.Instance.Students;

        }

        // 基本信息录入按钮点击事件
        private async void BasicInfoEntryButton_Click(object sender, RoutedEventArgs e)
        {
            // 创建并显示添加学生信息对话框
            var addStudentDialog = new Views.AddStudentDialog
            {
                XamlRoot = this.XamlRoot
            };

            var result = await addStudentDialog.ShowAsync();
            
            // 如果用户点击了确定按钮，学生信息已经在对话框中添加到StudentManager
            // 由于ListView绑定到ObservableCollection，界面会自动更新
        }

        // 成绩详细按钮点击事件
        private void GradesButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取关联的学生对象
            if (sender is Button button && button.Tag is Student student)
            {
                // 创建并显示成绩详细信息子窗口
                var scoreDetailWindow = new ScoresDialogWindow(student);
                scoreDetailWindow.Activate();
            }
        }

        // 奖惩详细按钮点击事件
        private void AwardPunishmentButton_Click(object sender, RoutedEventArgs e)
        {
            // 获取关联的学生对象
            if (sender is Button button && button.Tag is Student student)
            {
                // 创建并显示奖惩详细信息子窗口
                var rewardPunishmentWindow = new RewardAndPunishimentDialogWindow(student);
                rewardPunishmentWindow.Activate();
            }
        }

    }
}

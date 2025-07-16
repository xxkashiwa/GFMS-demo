using GFMS.Models;
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
        // 学生数据集合
        private ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();

        public DataCollectionPage()
        {
            InitializeComponent();

            // 设置ListView的数据源
            StudentsListView.ItemsSource = Students;

            // 加载样例数据显示（正式数据会从服务器加载）
            LoadStudentData();
        }

        // 基本信息录入按钮点击事件
        private void BasicInfoEntryButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 显示学生基本信息录入对话框
            // 此处仅作为演示，后续需实现具体逻辑
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

        // 加载学生数据的方法（示例数据）
        private void LoadStudentData()
        {
            // 添加一些示例学生数据用于测试
            var student1 = new Student
            {
                StudentId = "2023001",
                Name = "张三",
                Gender = "男",
                GraduationDate = new DateTime(2023, 6, 15)
            };

            // 添加示例成绩数据
            student1.Scores.Add("2023年春季", new List<Score>
            {
                new Score { Subject = "数学", ScoreValue = 88.5 },
                new Score { Subject = "物理", ScoreValue = 92.0 }
            });
            student1.Scores.Add("2023年秋季", new List<Score>
            {
                new Score { Subject = "英语", ScoreValue = 89.0 },
                new Score { Subject = "化学", ScoreValue = 91.5 }
            });
            student1.Scores.Add("2022年春季", new List<Score>
            {
                new Score { Subject = "计算机", ScoreValue = 95.0 },
                new Score { Subject = "数据结构", ScoreValue = 87.5 }
            });

            // 添加示例奖惩数据
            student1.RewardsAndPunishments.Add(new RewardAndPunishment
            {
                Type = RecordType.Reward,
                Details = "获得国家奖学金",
                Date = new DateTime(2023, 3, 15)
            });
            student1.RewardsAndPunishments.Add(new RewardAndPunishment
            {
                Type = RecordType.Reward,
                Details = "优秀学生干部",
                Date = new DateTime(2023, 5, 20)
            });
            student1.RewardsAndPunishments.Add(new RewardAndPunishment
            {
                Type = RecordType.Punishment,
                Details = "迟到警告",
                Date = new DateTime(2023, 2, 10)
            });

            var student2 = new Student
            {
                StudentId = "2023002",
                Name = "李四",
                Gender = "女",
                GraduationDate = new DateTime(2023, 6, 15)
            };

            // 添加示例成绩数据
            student2.Scores.Add("2023年春季", new List<Score>
            {
                new Score { Subject = "计算机科学", ScoreValue = 95.0 },
                new Score { Subject = "软件工程", ScoreValue = 89.5 }
            });
            student2.Scores.Add("2022年秋季", new List<Score>
            {
                new Score { Subject = "数据结构", ScoreValue = 92.0 },
                new Score { Subject = "算法", ScoreValue = 88.0 }
            });

            // 添加示例奖惩数据
            student2.RewardsAndPunishments.Add(new RewardAndPunishment
            {
                Type = RecordType.Reward,
                Details = "三好学生",
                Date = new DateTime(2023, 1, 15)
            });
            student2.RewardsAndPunishments.Add(new RewardAndPunishment
            {
                Type = RecordType.Reward,
                Details = "学术竞赛一等奖",
                Date = new DateTime(2023, 4, 10)
            });

            Students.Add(student1);
            Students.Add(student2);
        }
    }
}

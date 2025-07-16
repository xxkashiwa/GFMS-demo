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
    /// 成绩详细信息窗口
    /// </summary>
    public sealed partial class ScoresDialogWindow : Window
    {
        private Student _student;
        private ObservableCollection<ScoreDisplayItem> _scoreDisplayItems;
        private ObservableCollection<SemesterSummary> _semesterSummaries;

        public string StudentName => $"学生：{_student?.Name} ({_student?.StudentId}) - 成绩详细";

        public ScoresDialogWindow(Student student)
        {
            InitializeComponent();
            _student = student;
            StudentNameTextBlock.Text = StudentName;
            _scoreDisplayItems = new ObservableCollection<ScoreDisplayItem>();
            _semesterSummaries = new ObservableCollection<SemesterSummary>();
            ExtendsContentIntoTitleBar = true;

            // 设置窗口大小，使其比父窗口小
            SetWindowSize();

            LoadScoreData();
            CalculateSemesterSummaries();

            ScoresListView.ItemsSource = _scoreDisplayItems;
            SemesterSummaryRepeater.ItemsSource = _semesterSummaries;

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

                    // 设置对话框为主窗口的70%大小
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

        private void LoadScoreData()
        {
            _scoreDisplayItems.Clear();

            // 将学生的成绩数据转换为显示项
            foreach (var semesterScores in _student.Scores)
            {
                string semester = semesterScores.Key;
                foreach (var score in semesterScores.Value)
                {
                    _scoreDisplayItems.Add(new ScoreDisplayItem
                    {
                        Semester = semester,
                        Subject = score.Subject,
                        ScoreValue = score.ScoreValue
                    });
                }
            }
        }

        private void CalculateSemesterSummaries()
        {
            _semesterSummaries.Clear();

            // 计算每个学期的平均分
            foreach (var semesterScores in _student.Scores)
            {
                string semester = semesterScores.Key;
                var scores = semesterScores.Value;

                if (scores.Count > 0)
                {
                    double averageScore = scores.Average(s => s.ScoreValue);
                    _semesterSummaries.Add(new SemesterSummary
                    {
                        Semester = semester,
                        AverageScore = averageScore,
                        SubjectCount = scores.Count
                    });
                }
            }
        }

        private void AddScoreButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 实现插入成绩的逻辑
            // 目前先显示占位信息
            var dialog = new ContentDialog
            {
                Title = "添加成绩",
                Content = "插入成绩功能待实现",
                PrimaryButtonText = "确定",
                XamlRoot = this.Content.XamlRoot
            };

            _ = dialog.ShowAsync();
        }
    }
}

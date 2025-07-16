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
    /// �ɼ���ϸ��Ϣ����
    /// </summary>
    public sealed partial class ScoresDialogWindow : Window
    {
        private Student _student;
        private ObservableCollection<ScoreDisplayItem> _scoreDisplayItems;
        private ObservableCollection<SemesterSummary> _semesterSummaries;

        public string StudentName => $"ѧ����{_student?.Name} ({_student?.StudentId}) - �ɼ���ϸ";

        public ScoresDialogWindow(Student student)
        {
            InitializeComponent();
            _student = student;
            StudentNameTextBlock.Text = StudentName;
            _scoreDisplayItems = new ObservableCollection<ScoreDisplayItem>();
            _semesterSummaries = new ObservableCollection<SemesterSummary>();
            ExtendsContentIntoTitleBar = true;

            // ���ô��ڴ�С��ʹ��ȸ�����С
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
                // ��ȡ�����ڵĳߴ�
                var mainWindow = ((App)Application.Current)?.MainWindow;
                if (mainWindow?.AppWindow != null)
                {
                    var mainWindowSize = mainWindow.AppWindow.Size;

                    // ���öԻ���Ϊ�����ڵ�70%��С
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

        private void LoadScoreData()
        {
            _scoreDisplayItems.Clear();

            // ��ѧ���ĳɼ�����ת��Ϊ��ʾ��
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

            // ����ÿ��ѧ�ڵ�ƽ����
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
            // TODO: ʵ�ֲ���ɼ����߼�
            // Ŀǰ����ʾռλ��Ϣ
            var dialog = new ContentDialog
            {
                Title = "��ӳɼ�",
                Content = "����ɼ����ܴ�ʵ��",
                PrimaryButtonText = "ȷ��",
                XamlRoot = this.Content.XamlRoot
            };

            _ = dialog.ShowAsync();
        }
    }
}

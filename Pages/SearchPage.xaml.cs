using GFMS.Models;
using GFMS.Services;
using GFMS.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GFMS.Pages
{
    /// <summary>
    /// 查询页面，支持根据学号查询档案、学生信息或档案转递申请
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        private ObservableCollection<StudentFileManagementItem> _fileManagementResults;
        private ObservableCollection<Student> _studentInfoResults;
        private ObservableCollection<FileTransferApplication> _applicationResults;

        public SearchPage()
        {
            InitializeComponent();
            
            // 初始化结果集合
            _fileManagementResults = new ObservableCollection<StudentFileManagementItem>();
            _studentInfoResults = new ObservableCollection<Student>();
            _applicationResults = new ObservableCollection<FileTransferApplication>();
            
            // 设置默认选择
            SearchTypeComboBox.SelectedIndex = 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            // 清空搜索框
            KeywordTextBox.Text = string.Empty;
            
            // 显示初始界面
            ShowInitialState();
        }

        /// <summary>
        /// 显示初始状态
        /// </summary>
        private void ShowInitialState()
        {
            HideAllResultPanels();
            InitialPanel.Visibility = Visibility.Visible;
            ResultCountText.Text = "";
        }

        /// <summary>
        /// 隐藏所有结果面板
        /// </summary>
        private void HideAllResultPanels()
        {
            FileManagementResultsListView.Visibility = Visibility.Collapsed;
            StudentInfoResultsListView.Visibility = Visibility.Collapsed;
            ApplicationResultsListView.Visibility = Visibility.Collapsed;
            NoResultsPanel.Visibility = Visibility.Collapsed;
            InitialPanel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 搜索类型选择改变事件
        /// </summary>
        private void SearchTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 如果有关键词，立即搜索
            if (!string.IsNullOrWhiteSpace(KeywordTextBox?.Text))
            {
                PerformSearch();
            }
        }

        /// <summary>
        /// 关键词输入框键盘事件
        /// </summary>
        private void KeywordTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                PerformSearch();
            }
        }

        /// <summary>
        /// 搜索按钮点击事件
        /// </summary>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        /// <summary>
        /// 执行搜索
        /// </summary>
        private void PerformSearch()
        {
            var keyword = KeywordTextBox.Text?.Trim();
            
            if (string.IsNullOrEmpty(keyword))
            {
                ShowMessage("请输入学号进行查询");
                return;
            }

            if (SearchTypeComboBox.SelectedItem is not ComboBoxItem selectedItem)
            {
                ShowMessage("请选择查询类型");
                return;
            }

            var searchType = selectedItem.Tag?.ToString();
            
            switch (searchType)
            {
                case "Files":
                    SearchFileManagement(keyword);
                    break;
                case "Students":
                    SearchStudentInfo(keyword);
                    break;
                case "Applications":
                    SearchApplications(keyword);
                    break;
                default:
                    ShowMessage("无效的查询类型");
                    break;
            }
        }

        /// <summary>
        /// 搜索档案管理信息
        /// </summary>
        private void SearchFileManagement(string studentId)
        {
            HideAllResultPanels();
            
            _fileManagementResults.Clear();
            
            // 从学生管理器中查找学生
            var students = StudentManager.Instance.Students
                .Where(s => s.StudentId.Contains(studentId, StringComparison.OrdinalIgnoreCase))
                .ToList();
            
            foreach (var student in students)
            {
                var fileManagementItem = new StudentFileManagementItem
                {
                    Student = student
                };
                fileManagementItem.UpdateFileStatuses();
                _fileManagementResults.Add(fileManagementItem);
            }
            
            if (_fileManagementResults.Count > 0)
            {
                FileManagementResultsListView.ItemsSource = _fileManagementResults;
                FileManagementResultsListView.Visibility = Visibility.Visible;
                ResultCountText.Text = $"找到 {_fileManagementResults.Count} 条档案记录";
            }
            else
            {
                NoResultsPanel.Visibility = Visibility.Visible;
                ResultCountText.Text = "未找到匹配的档案记录";
            }
        }

        /// <summary>
        /// 搜索学生信息
        /// </summary>
        private void SearchStudentInfo(string studentId)
        {
            HideAllResultPanels();
            
            _studentInfoResults.Clear();
            
            // 从学生管理器中查找学生
            var students = StudentManager.Instance.Students
                .Where(s => s.StudentId.Contains(studentId, StringComparison.OrdinalIgnoreCase))
                .ToList();
            
            foreach (var student in students)
            {
                _studentInfoResults.Add(student);
            }
            
            if (_studentInfoResults.Count > 0)
            {
                StudentInfoResultsListView.ItemsSource = _studentInfoResults;
                StudentInfoResultsListView.Visibility = Visibility.Visible;
                ResultCountText.Text = $"找到 {_studentInfoResults.Count} 条学生记录";
            }
            else
            {
                NoResultsPanel.Visibility = Visibility.Visible;
                ResultCountText.Text = "未找到匹配的学生记录";
            }
        }

        /// <summary>
        /// 搜索档案转递申请
        /// </summary>
        private void SearchApplications(string studentId)
        {
            HideAllResultPanels();
            
            _applicationResults.Clear();
            
            // 从档案转递申请管理器中查找申请
            var applications = FileTransferApplicationManager.Instance.Applications
                .Where(a => a.StudentId.Contains(studentId, StringComparison.OrdinalIgnoreCase))
                .ToList();
            
            foreach (var application in applications)
            {
                _applicationResults.Add(application);
            }
            
            if (_applicationResults.Count > 0)
            {
                ApplicationResultsListView.ItemsSource = _applicationResults;
                ApplicationResultsListView.Visibility = Visibility.Visible;
                ResultCountText.Text = $"找到 {_applicationResults.Count} 条转递申请记录";
            }
            else
            {
                NoResultsPanel.Visibility = Visibility.Visible;
                ResultCountText.Text = "未找到匹配的转递申请记录";
            }
        }

        /// <summary>
        /// 成绩详细按钮点击事件
        /// </summary>
        private void GradesButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Student student)
            {
                var scoreDetailWindow = new ScoresDialogWindow(student);
                scoreDetailWindow.Activate();
            }
        }

        /// <summary>
        /// 奖惩详细按钮点击事件
        /// </summary>
        private void AwardPunishmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Student student)
            {
                var rewardPunishmentWindow = new RewardAndPunishimentDialogWindow(student);
                rewardPunishmentWindow.Activate();
            }
        }

        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="message">消息内容</param>
        private async void ShowMessage(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "提示",
                Content = message,
                CloseButtonText = "确定",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}

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
    /// ��ѯҳ�棬֧�ָ���ѧ�Ų�ѯ������ѧ����Ϣ�򵵰�ת������
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        private ObservableCollection<StudentFileManagementItem> _fileManagementResults;
        private ObservableCollection<Student> _studentInfoResults;
        private ObservableCollection<FileTransferApplication> _applicationResults;

        public SearchPage()
        {
            InitializeComponent();
            
            // ��ʼ���������
            _fileManagementResults = new ObservableCollection<StudentFileManagementItem>();
            _studentInfoResults = new ObservableCollection<Student>();
            _applicationResults = new ObservableCollection<FileTransferApplication>();
            
            // ����Ĭ��ѡ��
            SearchTypeComboBox.SelectedIndex = 0;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            // ���������
            KeywordTextBox.Text = string.Empty;
            
            // ��ʾ��ʼ����
            ShowInitialState();
        }

        /// <summary>
        /// ��ʾ��ʼ״̬
        /// </summary>
        private void ShowInitialState()
        {
            HideAllResultPanels();
            InitialPanel.Visibility = Visibility.Visible;
            ResultCountText.Text = "";
        }

        /// <summary>
        /// �������н�����
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
        /// ��������ѡ��ı��¼�
        /// </summary>
        private void SearchTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ����йؼ��ʣ���������
            if (!string.IsNullOrWhiteSpace(KeywordTextBox?.Text))
            {
                PerformSearch();
            }
        }

        /// <summary>
        /// �ؼ������������¼�
        /// </summary>
        private void KeywordTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                PerformSearch();
            }
        }

        /// <summary>
        /// ������ť����¼�
        /// </summary>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        /// <summary>
        /// ִ������
        /// </summary>
        private void PerformSearch()
        {
            var keyword = KeywordTextBox.Text?.Trim();
            
            if (string.IsNullOrEmpty(keyword))
            {
                ShowMessage("������ѧ�Ž��в�ѯ");
                return;
            }

            if (SearchTypeComboBox.SelectedItem is not ComboBoxItem selectedItem)
            {
                ShowMessage("��ѡ���ѯ����");
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
                    ShowMessage("��Ч�Ĳ�ѯ����");
                    break;
            }
        }

        /// <summary>
        /// ��������������Ϣ
        /// </summary>
        private void SearchFileManagement(string studentId)
        {
            HideAllResultPanels();
            
            _fileManagementResults.Clear();
            
            // ��ѧ���������в���ѧ��
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
                ResultCountText.Text = $"�ҵ� {_fileManagementResults.Count} ��������¼";
            }
            else
            {
                NoResultsPanel.Visibility = Visibility.Visible;
                ResultCountText.Text = "δ�ҵ�ƥ��ĵ�����¼";
            }
        }

        /// <summary>
        /// ����ѧ����Ϣ
        /// </summary>
        private void SearchStudentInfo(string studentId)
        {
            HideAllResultPanels();
            
            _studentInfoResults.Clear();
            
            // ��ѧ���������в���ѧ��
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
                ResultCountText.Text = $"�ҵ� {_studentInfoResults.Count} ��ѧ����¼";
            }
            else
            {
                NoResultsPanel.Visibility = Visibility.Visible;
                ResultCountText.Text = "δ�ҵ�ƥ���ѧ����¼";
            }
        }

        /// <summary>
        /// ��������ת������
        /// </summary>
        private void SearchApplications(string studentId)
        {
            HideAllResultPanels();
            
            _applicationResults.Clear();
            
            // �ӵ���ת������������в�������
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
                ResultCountText.Text = $"�ҵ� {_applicationResults.Count} ��ת�������¼";
            }
            else
            {
                NoResultsPanel.Visibility = Visibility.Visible;
                ResultCountText.Text = "δ�ҵ�ƥ���ת�������¼";
            }
        }

        /// <summary>
        /// �ɼ���ϸ��ť����¼�
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
        /// ������ϸ��ť����¼�
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
        /// ��ʾ��Ϣ
        /// </summary>
        /// <param name="message">��Ϣ����</param>
        private async void ShowMessage(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "��ʾ",
                Content = message,
                CloseButtonText = "ȷ��",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}

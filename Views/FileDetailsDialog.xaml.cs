using GFMS.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Text;
using System.Linq;

namespace GFMS.Views
{
    /// <summary>
    /// ������ϸ��Ϣ�Ի���
    /// </summary>
    public sealed partial class FileDetailsDialog : ContentDialog
    {
        private StudentFileManagementItem _studentItem;

        /// <summary>
        /// ��ʼ��������ϸ��Ϣ�Ի���
        /// </summary>
        /// <param name="studentItem">ѧ������������</param>
        public FileDetailsDialog(StudentFileManagementItem studentItem)
        {
            this.InitializeComponent();
            _studentItem = studentItem;
            InitializeDialog();
        }

        /// <summary>
        /// ��ʼ���Ի�������
        /// </summary>
        private void InitializeDialog()
        {
            // ���ñ���
            this.Title = $"������ϸ��Ϣ - {_studentItem.Student.Name} ({_studentItem.Student.StudentId})";

            // ����ѧ����Ϣ
            StudentIdTextBlock.Text = _studentItem.Student.StudentId;
            StudentNameTextBlock.Text = _studentItem.Student.Name;

            // �����ļ��б�
            LoadFileList();
        }

        /// <summary>
        /// �����ļ��б�
        /// </summary>
        private void LoadFileList()
        {
            // ��������ļ��б�
            FileListPanel.Children.Clear();

            // ��ȡ���ϴ����ļ�
            var uploadedFiles = _studentItem.Student.Files.Where(f => !string.IsNullOrEmpty(f.FilePath)).ToList();

            if (uploadedFiles.Count == 0)
            {
                // ��ʾ���ļ���ʾ
                NoFilesTextBlock.Visibility = Visibility.Visible;
                return;
            }

            // �������ļ���ʾ
            NoFilesTextBlock.Visibility = Visibility.Collapsed;

            // Ϊÿ���ļ�������
            foreach (var file in uploadedFiles)
            {
                CreateFileRow(file);
            }
        }

        /// <summary>
        /// �����ļ���
        /// </summary>
        /// <param name="file">�ļ���Ϣ</param>
        private void CreateFileRow(StudentFile file)
        {
            var fileGrid = new Grid
            {
                Background = (Microsoft.UI.Xaml.Media.Brush)Application.Current.Resources["CardBackgroundFillColorSecondaryBrush"],
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(16, 8, 16, 8),
                Margin = new Thickness(0, 2, 0, 2),
                MinHeight = 48
            };

            // �����ж���
            fileGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            fileGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            fileGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            fileGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });

            // �ļ�����
            var fileTypeText = new TextBlock
            {
                Text = file.FileType,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 14
            };

            // �ϴ�ʱ��
            var uploadTimeText = new TextBlock
            {
                Text = file.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 14
            };

            // ״̬
            var statusText = new TextBlock
            {
                Text = GetFileStateDisplayName(file.State),
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.SemiBold,
                FontSize = 14
            };

            // ������ť���
            var operationPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center,
                Spacing = 6
            };

            // ����˰�ť
            var approveButton = new Button
            {
                Content = "�����",
                Background = (Microsoft.UI.Xaml.Media.Brush)Application.Current.Resources["AccentFillColorDefaultBrush"],
                Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White),
                Padding = new Thickness(8, 4, 8, 4),
                CornerRadius = new CornerRadius(4),
                FontSize = 12,
                Tag = file
            };
            approveButton.Click += (s, args) => ApproveFile(file, fileGrid);

            // ���ذ�ť
            var rejectButton = new Button
            {
                Content = "����",
                Background = (Microsoft.UI.Xaml.Media.Brush)Application.Current.Resources["SystemFillColorCriticalBrush"],
                Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White),
                Padding = new Thickness(8, 4, 8, 4),
                CornerRadius = new CornerRadius(4),
                FontSize = 12,
                Tag = file
            };
            rejectButton.Click += (s, args) => RejectFile(file, fileGrid);

            operationPanel.Children.Add(approveButton);
            operationPanel.Children.Add(rejectButton);

            // ��������λ��
            Grid.SetColumn(fileTypeText, 0);
            Grid.SetColumn(uploadTimeText, 1);
            Grid.SetColumn(statusText, 2);
            Grid.SetColumn(operationPanel, 3);

            // ��ӵ�����
            fileGrid.Children.Add(fileTypeText);
            fileGrid.Children.Add(uploadTimeText);
            fileGrid.Children.Add(statusText);
            fileGrid.Children.Add(operationPanel);

            // ��ӵ��ļ��б����
            FileListPanel.Children.Add(fileGrid);
        }

        /// <summary>
        /// ���ͨ���ļ�
        /// </summary>
        /// <param name="file">�ļ���Ϣ</param>
        /// <param name="fileGrid">�ļ�����</param>
        private void ApproveFile(StudentFile file, Grid fileGrid)
        {
            // �����ļ�״̬
            file.State = FileState.�����;

            // ����ѧ������������״̬
            _studentItem.UpdateFileStatuses();

            // ǿ��ˢ�¸�ҳ�������
            RefreshParentPageData();

            // �ӽ������Ƴ�����
            FileListPanel.Children.Remove(fileGrid);

            // ����Ƿ����ļ������û������ʾ���ļ���ʾ
            CheckAndShowNoFilesMessage();
        }

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="file">�ļ���Ϣ</param>
        /// <param name="fileGrid">�ļ�����</param>
        private void RejectFile(StudentFile file, Grid fileGrid)
        {
            // �����ļ�״̬
            file.State = FileState.����;

            // ����ѧ������������״̬
            _studentItem.UpdateFileStatuses();

            // ǿ��ˢ�¸�ҳ�������
            RefreshParentPageData();

            // �ӽ������Ƴ�����
            FileListPanel.Children.Remove(fileGrid);

            // ����Ƿ����ļ������û������ʾ���ļ���ʾ
            CheckAndShowNoFilesMessage();
        }


        /// <summary>
        /// ˢ�¸�ҳ������
        /// </summary>
        private void RefreshParentPageData()
        {
            // �� WinUI 3 ��ʹ�� Application.Current ����ȡ��ǰӦ�ó���ʵ��
            var app = Application.Current as App;
            var window = app?.MainWindow;

            if (window?.Content is Frame frame && frame.Content is GFMS.Pages.FileManagementPage fileManagementPage)
            {
                // ͨ���������˽�з��� SyncStudentData
                var method = fileManagementPage.GetType().GetMethod("SyncStudentData",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                method?.Invoke(fileManagementPage, null);
            }
        }

        /// <summary>
        /// ��鲢��ʾ���ļ���Ϣ
        /// </summary>
        private void CheckAndShowNoFilesMessage()
        {
            if (FileListPanel.Children.Count == 0)
            {
                NoFilesTextBlock.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// ��ȡ�ļ�״̬��ʾ����
        /// </summary>
        /// <param name="state">�ļ�״̬</param>
        /// <returns>��ʾ����</returns>
        private string GetFileStateDisplayName(FileState state)
        {
            return state switch
            {
                FileState.���ϴ� => "���ϴ�",
                FileState.����� => "�����",
                FileState.���� => "����",
                _ => "δ֪"
            };
        }
    }
}
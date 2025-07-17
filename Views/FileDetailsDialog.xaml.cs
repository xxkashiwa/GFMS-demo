using GFMS.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Text;
using System.Linq;

namespace GFMS.Views
{
    /// <summary>
    /// 档案详细信息对话框
    /// </summary>
    public sealed partial class FileDetailsDialog : ContentDialog
    {
        private StudentFileManagementItem _studentItem;

        /// <summary>
        /// 初始化档案详细信息对话框
        /// </summary>
        /// <param name="studentItem">学生档案管理项</param>
        public FileDetailsDialog(StudentFileManagementItem studentItem)
        {
            this.InitializeComponent();
            _studentItem = studentItem;
            InitializeDialog();
        }

        /// <summary>
        /// 初始化对话框内容
        /// </summary>
        private void InitializeDialog()
        {
            // 设置标题
            this.Title = $"档案详细信息 - {_studentItem.Student.Name} ({_studentItem.Student.StudentId})";

            // 设置学生信息
            StudentIdTextBlock.Text = _studentItem.Student.StudentId;
            StudentNameTextBlock.Text = _studentItem.Student.Name;

            // 加载文件列表
            LoadFileList();
        }

        /// <summary>
        /// 加载文件列表
        /// </summary>
        private void LoadFileList()
        {
            // 清空现有文件列表
            FileListPanel.Children.Clear();

            // 获取已上传的文件
            var uploadedFiles = _studentItem.Student.Files.Where(f => !string.IsNullOrEmpty(f.FilePath)).ToList();

            if (uploadedFiles.Count == 0)
            {
                // 显示无文件提示
                NoFilesTextBlock.Visibility = Visibility.Visible;
                return;
            }

            // 隐藏无文件提示
            NoFilesTextBlock.Visibility = Visibility.Collapsed;

            // 为每个文件创建行
            foreach (var file in uploadedFiles)
            {
                CreateFileRow(file);
            }
        }

        /// <summary>
        /// 创建文件行
        /// </summary>
        /// <param name="file">文件信息</param>
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

            // 设置列定义
            fileGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            fileGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            fileGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            fileGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });

            // 文件类型
            var fileTypeText = new TextBlock
            {
                Text = file.FileType,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 14
            };

            // 上传时间
            var uploadTimeText = new TextBlock
            {
                Text = file.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 14
            };

            // 状态
            var statusText = new TextBlock
            {
                Text = GetFileStateDisplayName(file.State),
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.SemiBold,
                FontSize = 14
            };

            // 操作按钮面板
            var operationPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center,
                Spacing = 6
            };

            // 已审核按钮
            var approveButton = new Button
            {
                Content = "已审核",
                Background = (Microsoft.UI.Xaml.Media.Brush)Application.Current.Resources["AccentFillColorDefaultBrush"],
                Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White),
                Padding = new Thickness(8, 4, 8, 4),
                CornerRadius = new CornerRadius(4),
                FontSize = 12,
                Tag = file
            };
            approveButton.Click += (s, args) => ApproveFile(file, fileGrid);

            // 驳回按钮
            var rejectButton = new Button
            {
                Content = "驳回",
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

            // 设置网格位置
            Grid.SetColumn(fileTypeText, 0);
            Grid.SetColumn(uploadTimeText, 1);
            Grid.SetColumn(statusText, 2);
            Grid.SetColumn(operationPanel, 3);

            // 添加到网格
            fileGrid.Children.Add(fileTypeText);
            fileGrid.Children.Add(uploadTimeText);
            fileGrid.Children.Add(statusText);
            fileGrid.Children.Add(operationPanel);

            // 添加到文件列表面板
            FileListPanel.Children.Add(fileGrid);
        }

        /// <summary>
        /// 审核通过文件
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="fileGrid">文件网格</param>
        private void ApproveFile(StudentFile file, Grid fileGrid)
        {
            // 更新文件状态
            file.State = FileState.已审核;

            // 更新学生档案管理项状态
            _studentItem.UpdateFileStatuses();

            // 强制刷新父页面的数据
            RefreshParentPageData();

            // 从界面中移除该行
            FileListPanel.Children.Remove(fileGrid);

            // 检查是否还有文件，如果没有则显示无文件提示
            CheckAndShowNoFilesMessage();
        }

        /// <summary>
        /// 驳回文件
        /// </summary>
        /// <param name="file">文件信息</param>
        /// <param name="fileGrid">文件网格</param>
        private void RejectFile(StudentFile file, Grid fileGrid)
        {
            // 更新文件状态
            file.State = FileState.驳回;

            // 更新学生档案管理项状态
            _studentItem.UpdateFileStatuses();

            // 强制刷新父页面的数据
            RefreshParentPageData();

            // 从界面中移除该行
            FileListPanel.Children.Remove(fileGrid);

            // 检查是否还有文件，如果没有则显示无文件提示
            CheckAndShowNoFilesMessage();
        }


        /// <summary>
        /// 刷新父页面数据
        /// </summary>
        private void RefreshParentPageData()
        {
            // 在 WinUI 3 中使用 Application.Current 来获取当前应用程序实例
            var app = Application.Current as App;
            var window = app?.MainWindow;

            if (window?.Content is Frame frame && frame.Content is GFMS.Pages.FileManagementPage fileManagementPage)
            {
                // 通过反射调用私有方法 SyncStudentData
                var method = fileManagementPage.GetType().GetMethod("SyncStudentData",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                method?.Invoke(fileManagementPage, null);
            }
        }

        /// <summary>
        /// 检查并显示无文件消息
        /// </summary>
        private void CheckAndShowNoFilesMessage()
        {
            if (FileListPanel.Children.Count == 0)
            {
                NoFilesTextBlock.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 获取文件状态显示名称
        /// </summary>
        /// <param name="state">文件状态</param>
        /// <returns>显示名称</returns>
        private string GetFileStateDisplayName(FileState state)
        {
            return state switch
            {
                FileState.已上传 => "已上传",
                FileState.已审核 => "已审核",
                FileState.驳回 => "驳回",
                _ => "未知"
            };
        }
    }
}
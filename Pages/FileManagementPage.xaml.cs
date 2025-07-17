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
using Microsoft.UI.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GFMS.Pages
{
    /// <summary>
    /// 档案管理页面，用于管理学生档案文件
    /// </summary>
    public sealed partial class FileManagementPage : Page
    {
        /// <summary>
        /// 用于绑定到ListView的学生档案管理项集合
        /// </summary>
        private ObservableCollection<StudentFileManagementItem> _fileManagementItems;

        public FileManagementPage()
        {
            InitializeComponent();

            // 初始化档案管理项集合
            _fileManagementItems = new ObservableCollection<StudentFileManagementItem>();

            // 设置ListView的数据源
            FileManagementListView.ItemsSource = _fileManagementItems;

            // 加载学生数据
            LoadStudentData();
        }

        /// <summary>
        /// 页面导航时的处理
        /// </summary>
        /// <param name="e">导航参数</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // 每次进入页面时同步学生数据
            SyncStudentData();
        }

        /// <summary>
        /// 加载学生数据
        /// </summary>
        private void LoadStudentData()
        {
            SyncStudentData();
        }

        /// <summary>
        /// 同步学生数据，检测StudentManager中的新学生并追加到列表中
        /// </summary>
        private void SyncStudentData()
        {
            // 遍历StudentManager中的所有学生
            foreach (var student in StudentManager.Instance.Students)
            {
                // 检查当前集合中是否已存在该学生
                var existingItem = _fileManagementItems.FirstOrDefault(item => item.StudentId == student.StudentId);

                if (existingItem == null)
                {
                    // 如果不存在，则创建新的档案管理项并添加到集合中
                    var newItem = new StudentFileManagementItem
                    {
                        Student = student
                    };
                    newItem.UpdateFileStatuses();
                    _fileManagementItems.Add(newItem);
                }
                else
                {
                    // 如果存在，则更新学生信息和档案状态
                    existingItem.Student = student;
                    existingItem.UpdateFileStatuses();
                }
            }

            // 移除在StudentManager中不存在的学生
            var itemsToRemove = _fileManagementItems.Where(item =>
                !StudentManager.Instance.Students.Any(student => student.StudentId == item.StudentId)).ToList();

            foreach (var item in itemsToRemove)
            {
                _fileManagementItems.Remove(item);
            }
        }

        /// <summary>
        /// 检查当前用户是否有Teacher权限
        /// </summary>
        private bool IsTeacher()
        {
            return UserManager.Instance.IsAuthed &&
                   UserManager.Instance.AuthedUser?.GrantedType == "Teacher";
        }

        /// <summary>
        /// 检查当前用户是否有Admin权限
        /// </summary>
        private bool IsAdmin()
        {
            return UserManager.Instance.IsAuthed &&
                   UserManager.Instance.AuthedUser?.GrantedType == "Admin";
        }

        /// <summary>
        /// 处理文件操作（上传或打开）
        /// </summary>
        private async System.Threading.Tasks.Task HandleFileOperation(StudentFileManagementItem item, string fileType)
        {
            if (!IsTeacher() && !IsAdmin())
            {
                await ShowPermissionDeniedDialog();
                return;
            }

            var existingFile = item.Student.Files.FirstOrDefault(f => f.FileType == fileType);

            if (existingFile == null) // 未上传状态
            {
                // 上传文件
                await UploadFile(item, fileType);
            }
            else
            {
                // 打开文件
                await OpenFile(existingFile);
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        private async System.Threading.Tasks.Task UploadFile(StudentFileManagementItem item, string fileType)
        {
            try
            {
                var picker = new FileOpenPicker();
                picker.FileTypeFilter.Add(".pdf");

                // 获取当前窗口句柄
                var window = (Application.Current as App)?.MainWindow;
                if (window != null)
                {
                    var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                    WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
                }

                var file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    // 创建目标文件夹
                    var documentsFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(
                        "StudentFiles", CreationCollisionOption.OpenIfExists);
                    var studentFolder = await documentsFolder.CreateFolderAsync(
                        item.Student.StudentId, CreationCollisionOption.OpenIfExists);

                    // 复制文件到应用数据文件夹
                    var fileName = $"{fileType}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                    var destinationFile = await studentFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                    await file.CopyAndReplaceAsync(destinationFile);

                    // 更新或添加文件记录
                    var existingFile = item.Student.Files.FirstOrDefault(f => f.FileType == fileType);
                    if (existingFile != null)
                    {
                        existingFile.FilePath = destinationFile.Path;
                        existingFile.UpdatedAt = DateTime.Now;
                        existingFile.State = FileState.已上传;
                    }
                    else
                    {
                        var studentFile = new StudentFile
                        {
                            StudentId = item.Student.StudentId,
                            FileType = fileType,
                            FilePath = destinationFile.Path,
                            State = FileState.已上传,
                            UpdatedAt = DateTime.Now
                        };
                        item.Student.Files.Add(studentFile);
                    }

                    // 更新显示状态
                    item.UpdateFileStatuses();

                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        private async System.Threading.Tasks.Task OpenFile(StudentFile file)
        {
            try
            {
                if (File.Exists(file.FilePath))
                {
                    // 使用默认程序打开PDF文件
                    var process = new Process();
                    process.StartInfo.FileName = file.FilePath;
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 显示权限不足对话框
        /// </summary>
        private async System.Threading.Tasks.Task ShowPermissionDeniedDialog()
        {
            var dialog = new ContentDialog()
            {
                Title = "权限不足",
                Content = "您没有执行此操作的权限。",
                CloseButtonText = "确定",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }


        /// <summary>
        /// 毕业登记表按钮点击事件
        /// </summary>
        private void GraduationFormButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is StudentFileManagementItem item)
            {
                _ = HandleFileOperation(item, "毕业登记表");
            }
        }

        /// <summary>
        /// 体检表按钮点击事件
        /// </summary>
        private void MedicalExamButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is StudentFileManagementItem item)
            {
                _ = HandleFileOperation(item, "体检表");
            }
        }

        /// <summary>
        /// 实习报告按钮点击事件
        /// </summary>
        private void InternshipReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is StudentFileManagementItem item)
            {
                _ = HandleFileOperation(item, "实习报告");
            }
        }

        /// <summary>
        /// 档案详细按钮点击事件
        /// </summary>
        private async void FileDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is StudentFileManagementItem item)
            {
                if (IsTeacher())
                {
                    await ShowPermissionDeniedDialog();
                    return;
                }

                if (IsAdmin())
                {
                    await ShowFileDetailsDialog(item);
                }
                else
                {
                    await ShowPermissionDeniedDialog();
                }
            }
        }

        /// <summary>
        /// 显示档案详细信息对话框
        /// </summary>
        private async System.Threading.Tasks.Task ShowFileDetailsDialog(StudentFileManagementItem item)
        {
            // 创建文件详细信息的内容
            var contentPanel = new StackPanel { Spacing = 12 };

            // 学生信息
            var studentInfoPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 16,
                Margin = new Thickness(0, 0, 0, 16)
            };
            studentInfoPanel.Children.Add(new TextBlock { Text = "学号:", FontWeight = FontWeights.SemiBold });
            studentInfoPanel.Children.Add(new TextBlock { Text = item.Student.StudentId });
            studentInfoPanel.Children.Add(new TextBlock { Text = "姓名:", FontWeight = FontWeights.SemiBold, Margin = new Thickness(32, 0, 0, 0) });
            studentInfoPanel.Children.Add(new TextBlock { Text = item.Student.Name });
            contentPanel.Children.Add(studentInfoPanel);

            // 表头
            var headerGrid = new Grid
            {
                Background = (Microsoft.UI.Xaml.Media.Brush)Application.Current.Resources["CardBackgroundFillColorDefaultBrush"],
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(16, 8, 16, 8),
                Margin = new Thickness(0, 0, 0, 8)
            };
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });

            var fileTypeHeader = new TextBlock { Text = "文件类型", FontWeight = FontWeights.SemiBold };
            var uploadTimeHeader = new TextBlock { Text = "上传时间", FontWeight = FontWeights.SemiBold };
            var statusHeader = new TextBlock { Text = "当前状态", FontWeight = FontWeights.SemiBold };
            var operationHeader = new TextBlock { Text = "操作", FontWeight = FontWeights.SemiBold };

            Grid.SetColumn(fileTypeHeader, 0);
            Grid.SetColumn(uploadTimeHeader, 1);
            Grid.SetColumn(statusHeader, 2);
            Grid.SetColumn(operationHeader, 3);

            headerGrid.Children.Add(fileTypeHeader);
            headerGrid.Children.Add(uploadTimeHeader);
            headerGrid.Children.Add(statusHeader);
            headerGrid.Children.Add(operationHeader);
            contentPanel.Children.Add(headerGrid);

            // 文件列表
            var uploadedFiles = item.Student.Files.Where(f => !string.IsNullOrEmpty(f.FilePath)).ToList();

            foreach (var file in uploadedFiles)
            {
                var fileGrid = new Grid
                {
                    Background = (Microsoft.UI.Xaml.Media.Brush)Application.Current.Resources["CardBackgroundFillColorSecondaryBrush"],
                    CornerRadius = new CornerRadius(4),
                    Padding = new Thickness(16, 8, 16, 8),
                    Margin = new Thickness(0, 2, 0, 2),
                    MinHeight = 48
                };
                fileGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                fileGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                fileGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
                fileGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });

                var fileTypeText = new TextBlock
                {
                    Text = file.FileType,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 14
                };
                var uploadTimeText = new TextBlock
                {
                    Text = file.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 14
                };
                var statusText = new TextBlock
                {
                    Text = GetFileStateDisplayName(file.State),
                    VerticalAlignment = VerticalAlignment.Center,
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 14
                };

                var operationPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Center,
                    Spacing = 6
                };

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
                approveButton.Click += async (s, args) =>
                {
                    file.State = FileState.已审核;
                    item.UpdateFileStatuses();
                    // 从对话框中移除该行
                    contentPanel.Children.Remove(fileGrid);
                    
                    // 如果没有更多文件了，显示无文件提示
                    if (contentPanel.Children.Count <= 2) // 只剩学生信息和表头
                    {
                        var noFilesText = new TextBlock
                        {
                            Text = "该学生暂未上传任何档案文件",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            FontStyle = Windows.UI.Text.FontStyle.Italic,
                            Margin = new Thickness(0, 20, 0, 20)
                        };
                        contentPanel.Children.Add(noFilesText);
                    }
                };

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
                rejectButton.Click += async (s, args) =>
                {
                    file.State = FileState.驳回;
                    item.UpdateFileStatuses();
                    // 从对话框中移除该行
                    contentPanel.Children.Remove(fileGrid);
                    
                    // 如果没有更多文件了，显示无文件提示
                    if (contentPanel.Children.Count <= 2) // 只剩学生信息和表头
                    {
                        var noFilesText = new TextBlock
                        {
                            Text = "该学生暂未上传任何档案文件",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            FontStyle = Windows.UI.Text.FontStyle.Italic,
                            Margin = new Thickness(0, 20, 0, 20)
                        };
                        contentPanel.Children.Add(noFilesText);
                    }
                };

                operationPanel.Children.Add(approveButton);
                operationPanel.Children.Add(rejectButton);

                Grid.SetColumn(fileTypeText, 0);
                Grid.SetColumn(uploadTimeText, 1);
                Grid.SetColumn(statusText, 2);
                Grid.SetColumn(operationPanel, 3);

                fileGrid.Children.Add(fileTypeText);
                fileGrid.Children.Add(uploadTimeText);
                fileGrid.Children.Add(statusText);
                fileGrid.Children.Add(operationPanel);

                contentPanel.Children.Add(fileGrid);
            }

            if (uploadedFiles.Count == 0)
            {
                var noFilesText = new TextBlock
                {
                    Text = "该学生暂未上传任何档案文件",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontStyle = Windows.UI.Text.FontStyle.Italic,
                    Margin = new Thickness(0, 20, 0, 20)
                };
                contentPanel.Children.Add(noFilesText);
            }

            var scrollViewer = new ScrollViewer
            {
                Content = contentPanel,
                MaxHeight = 400
            };

            var dialog = new ContentDialog()
            {
                Title = $"档案详细信息 - {item.Student.Name} ({item.Student.StudentId})",
                Content = scrollViewer,
                CloseButtonText = "关闭",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }

        /// <summary>
        /// 获取文件状态显示名称
        /// </summary>
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

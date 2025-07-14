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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace GFMS.Pages
{
    /// <summary>
    /// 档案材料管理页面
    /// </summary>
    public sealed partial class FileManagementPage : Page
    {
        // 静态数据模型，确保数据在页面导航间持久化
        private static ObservableCollection<ArchiveFile>? _uploadedFiles;
        private static ObservableCollection<ArchiveFile>? _reviewFiles;
        private static ObservableCollection<ArchiveFile>? _archivedFiles;
        private static bool _isDataLoaded = false;
        
        public ObservableCollection<ArchiveFile> UploadedFiles { get; set; }
        public ObservableCollection<ArchiveFile> ReviewFiles { get; set; }
        public ObservableCollection<ArchiveFile> ArchivedFiles { get; set; }
        
        private ArchiveFile _selectedFile;

        public FileManagementPage()
        {
            InitializeComponent();
            InitializeData();
            SetupEventHandlers();
        }

        private void InitializeData()
        {
            // 初始化或使用现有的静态数据集合
            if (_uploadedFiles == null)
            {
                _uploadedFiles = new ObservableCollection<ArchiveFile>();
                _reviewFiles = new ObservableCollection<ArchiveFile>();
                _archivedFiles = new ObservableCollection<ArchiveFile>();
            }
            
            UploadedFiles = _uploadedFiles;
            ReviewFiles = _reviewFiles;
            ArchivedFiles = _archivedFiles;

            // 只在第一次初始化时加载示例数据
            if (!_isDataLoaded)
            {
                LoadSampleData();
                _isDataLoaded = true;
            }

            // 绑定数据源
            UploadedFilesList.ItemsSource = UploadedFiles;
            ReviewFilesList.ItemsSource = ReviewFiles;
            ArchivedFilesList.ItemsSource = ArchivedFiles;
        }

        private void LoadSampleData()
        {
            // 示例上传文件
            UploadedFiles.Add(new ArchiveFile
            {
                StudentId = "2021001",
                StudentName = "张三",
                FileName = "毕业生登记表.pdf",
                FileType = "毕业生登记表",
                FileSize = "2.3 MB",
                UploadTime = DateTime.Now.AddDays(-1),
                Status = "已上传"
            });

            // 示例待审核文件
            ReviewFiles.Add(new ArchiveFile
            {
                StudentId = "2021002",
                StudentName = "李四",
                FileName = "体检表.pdf",
                FileType = "体检表",
                FileSize = "1.8 MB",
                UploadTime = DateTime.Now.AddHours(-2),
                Status = "待审核"
            });

            // 示例已归档文件
            ArchivedFiles.Add(new ArchiveFile
            {
                StudentId = "2021003",
                StudentName = "王五",
                FileName = "档案袋",
                FileType = "完整档案",
                FileSize = "15.6 MB",
                UploadTime = DateTime.Now.AddDays(-5),
                Status = "已归档",
                MaterialCount = "8/8",
                ArchiveTime = DateTime.Now.AddDays(-3)
            });
        }

        private void SetupEventHandlers()
        {
            // 为XAML中的按钮添加事件处理
            Loaded += FileManagementPage_Loaded;
        }

        private void FileManagementPage_Loaded(object sender, RoutedEventArgs e)
        {
            // 页面加载完成后的初始化
        }

        #region 材料上传标签页事件处理

        private async void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var picker = new FileOpenPicker();
                picker.FileTypeFilter.Add(".pdf");
                picker.FileTypeFilter.Add(".doc");
                picker.FileTypeFilter.Add(".docx");
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".png");
                
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(((App)Application.Current).MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
                
                var files = await picker.PickMultipleFilesAsync();
                if (files != null && files.Count > 0)
                {
                    await UploadFiles(files);
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog("文件选择失败", $"选择文件时发生错误：{ex.Message}");
            }
        }

        private async Task UploadFiles(IReadOnlyList<StorageFile> files)
        {
            foreach (var file in files)
            {
                try
                {
                    // 模拟文件上传过程
                    UploadProgressBar.Value = 0;
                    
                    for (int i = 0; i <= 100; i += 10)
                    {
                        UploadProgressBar.Value = i;
                        await Task.Delay(100); // 模拟上传延迟
                    }

                    // 添加到上传列表
                    var archiveFile = new ArchiveFile
                    {
                        StudentId = GetSelectedStudentId(),
                        StudentName = GetSelectedStudentName(),
                        FileName = file.Name,
                        FileType = GetSelectedMaterialType(),
                        FileSize = await GetFileSizeString(file),
                        UploadTime = DateTime.Now,
                        Status = "已上传"
                    };

                    UploadedFiles.Add(archiveFile);
                    
                    ShowInfoDialog("上传成功", $"文件 {file.Name} 上传完成！");
                }
                catch (Exception ex)
                {
                    ShowErrorDialog("上传失败", $"文件 {file.Name} 上传失败：{ex.Message}");
                }
            }
        }

        private string GetSelectedStudentId()
        {
            if (StudentSelector.SelectedItem is ComboBoxItem item)
            {
                var text = item.Content.ToString();
                var start = text.IndexOf('(') + 1;
                var end = text.IndexOf(')');
                return text.Substring(start, end - start);
            }
            return "未选择";
        }

        private string GetSelectedStudentName()
        {
            if (StudentSelector.SelectedItem is ComboBoxItem item)
            {
                var text = item.Content.ToString();
                var end = text.IndexOf(' ');
                return text.Substring(0, end);
            }
            return "未选择";
        }

        private string GetSelectedMaterialType()
        {
            if (MaterialTypeSelector.SelectedItem is ComboBoxItem item)
            {
                return item.Content.ToString();
            }
            return "其他材料";
        }

        private async Task<string> GetFileSizeString(StorageFile file)
        {
            var properties = await file.GetBasicPropertiesAsync();
            var sizeInMB = properties.Size / (1024.0 * 1024.0);
            return $"{sizeInMB:F1} MB";
        }

        private async void BatchUploadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var picker = new FileOpenPicker();
                picker.FileTypeFilter.Add(".xlsx");
                picker.FileTypeFilter.Add(".xls");
                
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(((App)Application.Current).MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
                
                var file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    // TODO: 实现批量上传逻辑
                    ShowInfoDialog("批量上传", "批量上传功能开发中...");
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog("批量上传失败", $"批量上传时发生错误：{ex.Message}");
            }
        }

        private async void DownloadTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var picker = new FileSavePicker();
                picker.FileTypeChoices.Add("Excel文件", new List<string> { ".xlsx" });
                picker.SuggestedFileName = "档案材料上传模板";
                
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(((App)Application.Current).MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
                
                var file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    // TODO: 生成模板文件
                    ShowInfoDialog("下载成功", "模板文件已保存！");
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog("下载失败", $"模板下载时发生错误：{ex.Message}");
            }
        }

        private void PreviewFileButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 实现文件预览功能
            ShowInfoDialog("文件预览", "文件预览功能开发中...");
        }

        private void DeleteFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ArchiveFile file)
            {
                ShowConfirmDialog("确认删除", $"确定要删除文件 {file.FileName} 吗？", () =>
                {
                    UploadedFiles.Remove(file);
                    ShowInfoDialog("删除成功", "文件已删除！");
                });
            }
        }

        private void UploadInstructionButton_Click(object sender, RoutedEventArgs e)
        {
            ShowInfoDialog("上传说明", "请按照要求上传相关档案材料，确保文件格式正确、内容清晰。");
        }

        private void BatchDownloadTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            ShowInfoDialog("批量下载模板", "批量下载模板功能开发中...");
        }

        private void ViewUploadHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            ShowInfoDialog("查看上传历史", "上传历史查看功能开发中...");
        }

        private void ExportFileListButton_Click(object sender, RoutedEventArgs e)
        {
            ShowInfoDialog("导出文件清单", "文件清单导出功能开发中...");
        }

        #endregion

        #region 材料审核标签页事件处理

        private void ReviewFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ArchiveFile file)
            {
                _selectedFile = file;
                // TODO: 显示审核详情面板
                ShowInfoDialog("审核", $"正在审核 {file.StudentName} 的 {file.FileType}");
            }
        }

        private void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedFile != null)
            {
                _selectedFile.Status = "已通过";
                ReviewFiles.Remove(_selectedFile);
                ShowInfoDialog("审核完成", "材料审核通过！");
                _selectedFile = null;
            }
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedFile != null)
            {
                _selectedFile.Status = "已退回";
                ShowInfoDialog("审核完成", "材料已退回，请查看审核意见！");
                _selectedFile = null;
            }
        }

        private void SaveDraftButton_Click(object sender, RoutedEventArgs e)
        {
            ShowInfoDialog("暂存成功", "审核意见已暂存！");
        }

        private void BatchReviewButton_Click(object sender, RoutedEventArgs e)
        {
            ShowInfoDialog("批量审核", "批量审核功能开发中...");
        }

        private void ExportReportButton_Click(object sender, RoutedEventArgs e)
        {
            ShowInfoDialog("导出报告", "报告导出功能开发中...");
        }

        private void ReviewStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem item)
            {
                var status = item.Content.ToString();
                FilterReviewFilesByStatus(status);
            }
        }

        private void ReviewMajorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem item)
            {
                var major = item.Content.ToString();
                FilterReviewFilesByMajor(major);
            }
        }

        private void OpenPreviewButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedFile != null)
            {
                ShowInfoDialog("文件预览", $"正在预览 {_selectedFile.FileName}");
            }
            else
            {
                ShowInfoDialog("文件预览", "请先选择要预览的文件");
            }
        }

        private void FilterReviewFilesByStatus(string status)
        {
            // TODO: 实现按状态筛选审核文件的逻辑
            ShowInfoDialog("筛选", $"按状态筛选：{status}");
        }

        private void FilterReviewFilesByMajor(string major)
        {
            // TODO: 实现按专业筛选审核文件的逻辑
            ShowInfoDialog("筛选", $"按专业筛选：{major}");
        }

        #endregion

        #region 材料归档标签页事件处理

        private void GenerateArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            ShowInfoDialog("生成档案袋", "电子档案袋生成功能开发中...");
        }

        private void BatchArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            ShowInfoDialog("批量归档", "批量归档功能开发中...");
        }

        private void ExportStatisticsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowInfoDialog("导出统计", "统计导出功能开发中...");
        }

        private void ViewArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ArchiveFile file)
            {
                ShowInfoDialog("查看档案袋", $"正在查看 {file.StudentName} 的档案袋");
            }
        }

        private void DownloadArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ArchiveFile file)
            {
                ShowInfoDialog("下载档案", $"正在下载 {file.StudentName} 的档案");
            }
        }

        private void ArchiveStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem item)
            {
                var status = item.Content.ToString();
                FilterArchivedFilesByStatus(status);
            }
        }

        private void FilterArchivedFilesByStatus(string status)
        {
            // TODO: 实现按归档状态筛选文件的逻辑
            ShowInfoDialog("筛选", $"按归档状态筛选：{status}");
        }

        #endregion

        #region 通用对话框和辅助方法

        private async void ShowInfoDialog(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "确定",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async void ShowErrorDialog(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "确定",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async void ShowConfirmDialog(string title, string message, Action onConfirm)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                PrimaryButtonText = "确定",
                CloseButtonText = "取消",
                XamlRoot = this.XamlRoot
            };
            
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                onConfirm?.Invoke();
            }
        }

        #endregion
    }

    // 档案文件数据模型
    public class ArchiveFile
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public DateTime UploadTime { get; set; }
        public string Status { get; set; }
        public string MaterialCount { get; set; }
        public DateTime? ArchiveTime { get; set; }
    }
}

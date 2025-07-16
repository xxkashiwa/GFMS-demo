using GFMS.Models;
using GFMS.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace GFMS.Dialogs
{
    public sealed partial class ArchiveRequestDialog : ContentDialog
    {
        private ListView AttachmentsList;
        private TextBox ReceiverNameTextBox;
        private TextBox ReceiverAddressTextBox;

        private TextBox StudentIdTextBox; // 添加缺失的控件声明
        private TextBox StudentNameTextBox; // 添加缺失的控件声明
        private TextBox MajorTextBox; // 添加缺失的控件声明
        private TextBox GraduationYearTextBox; // 添加缺失的控件声明
        public ArchiveRequest? Request { get; private set; }
        private ObservableCollection<string> _attachments = new();
        private void InitializeComponent()
        {
            // 通常在这里初始化 XAML 组件
        }

        public ArchiveRequestDialog()
        {
            this.InitializeComponent();
            AttachmentsList.ItemsSource = _attachments;
            LoadCurrentUserInfo();

            // 设置按钮点击事件
            this.PrimaryButtonClick += ArchiveRequestDialog_PrimaryButtonClick;
        }

        private async void ArchiveRequestDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 验证输入
            if (!ValidateInput())
            {
                args.Cancel = true;
                return;
            }

            // 创建申请对象，只包含新表结构中的字段
            Request = new ArchiveRequest
            {
                ReceiverName = ReceiverNameTextBox.Text.Trim(),
                ReceiverAddress = ReceiverAddressTextBox.Text.Trim(),
                RequestDate = DateTime.Now,
                Status = "待审核"
            };
        }

        private void LoadCurrentUserInfo()
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser?.Role == "Student")
            {
                var student = ArchiveTransferService.Instance.GetStudentByStudentId(currentUser.Username);
                if (student != null)
                {
                    StudentIdTextBox.Text = student.StudentId;
                    StudentNameTextBox.Text = student.Name;
                    MajorTextBox.Text = student.Major ?? "";
                    GraduationYearTextBox.Text = student.GraduationYear?.ToString() ?? "";
                }
            }
        }

        private async void SelectFilesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var picker = new FileOpenPicker();
                picker.FileTypeFilter.Add(".pdf");
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");
                picker.FileTypeFilter.Add(".doc");
                picker.FileTypeFilter.Add(".docx");

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(((App)Application.Current).MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                var files = await picker.PickMultipleFilesAsync();
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        _attachments.Add(file.Name);
                    }
                    AttachmentsList.Visibility = _attachments.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                await ShowErrorDialog("文件选择失败", ex.Message);
            }
        }

        private bool ValidateInput()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(ReceiverNameTextBox.Text))
                errors.Add("请填写接收单位名称");

            if (string.IsNullOrWhiteSpace(ReceiverAddressTextBox.Text))
                errors.Add("请填写接收单位地址");

            if (errors.Count > 0)
            {
                _ = ShowErrorDialog("信息填写不完整", string.Join("\n", errors));
                return false;
            }

            return true;
        }

        private async System.Threading.Tasks.Task ShowErrorDialog(string title, string message)
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
    }
}
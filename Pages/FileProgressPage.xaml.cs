using GFMS.Models;
using GFMS.Services;
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
    /// 档案转递页面，根据用户角色显示不同内容
    /// </summary>
    public sealed partial class FileProgressPage : Page
    {
        /// <summary>
        /// 学生的申请记录集合
        /// </summary>
        private ObservableCollection<FileTransferApplication> _studentApplications;

        /// <summary>
        /// 管理员当前是否在申请视图模式
        /// </summary>
        private bool _isAdminInApplicationView = false;

        public FileProgressPage()
        {
            InitializeComponent();
            
            _studentApplications = new ObservableCollection<FileTransferApplication>();
            
            // 根据用户角色设置页面布局
            SetupPageForUserRole();
        }

        /// <summary>
        /// 页面导航时的处理
        /// </summary>
        /// <param name="e">导航参数</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            // 每次进入页面时刷新数据
            RefreshData();
        }

        /// <summary>
        /// 根据用户角色设置页面布局
        /// </summary>
        private void SetupPageForUserRole()
        {
            if (!UserManager.Instance.IsAuthed)
            {
                // 未认证用户隐藏所有功能
                AdminControlPanel.Visibility = Visibility.Collapsed;
                StudentApplicationForm.Visibility = Visibility.Collapsed;
                StudentHistoryTable.Visibility = Visibility.Collapsed;
                AdminAllApplicationsTable.Visibility = Visibility.Collapsed;
                return;
            }

            var userRole = UserManager.Instance.AuthedUser?.GrantedType;

            switch (userRole)
            {
                case "Student":
                    // 学生角色：显示申请表单和个人历史记录
                    AdminControlPanel.Visibility = Visibility.Collapsed;
                    StudentApplicationForm.Visibility = Visibility.Visible;
                    StudentHistoryTable.Visibility = Visibility.Visible;
                    AdminAllApplicationsTable.Visibility = Visibility.Collapsed;
                    
                    // 自动填充当前用户的学号
                    if (UserManager.Instance.AuthedUser != null)
                    {
                        StudentIdTextBox.Text = UserManager.Instance.AuthedUser.UserId;
                        StudentIdTextBox.IsReadOnly = true; // 学生不能修改自己的学号
                    }
                    break;
                    
                case "Admin":
                    // 管理员角色：显示控制面板，默认显示管理表格
                    AdminControlPanel.Visibility = Visibility.Visible;
                    SetAdminView(false); // 默认管理视图
                    break;
                    
                default:
                    // 其他角色隐藏所有功能
                    AdminControlPanel.Visibility = Visibility.Collapsed;
                    StudentApplicationForm.Visibility = Visibility.Collapsed;
                    StudentHistoryTable.Visibility = Visibility.Collapsed;
                    AdminAllApplicationsTable.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        /// <summary>
        /// 设置管理员视图模式
        /// </summary>
        /// <param name="isApplicationView">是否为申请视图模式</param>
        private void SetAdminView(bool isApplicationView)
        {
            _isAdminInApplicationView = isApplicationView;
            
            if (isApplicationView)
            {
                // 申请视图：显示申请表单和历史记录
                StudentApplicationForm.Visibility = Visibility.Visible;
                StudentHistoryTable.Visibility = Visibility.Visible;
                AdminAllApplicationsTable.Visibility = Visibility.Collapsed;
                
                // 管理员可以修改学号，不自动填充
                StudentIdTextBox.IsReadOnly = false;
                StudentIdTextBox.Text = string.Empty;
                
                // 更新按钮文本
                SwitchToApplicationViewButton.Content = "转换为管理视图";
            }
            else
            {
                // 管理视图：显示所有申请的管理表格
                StudentApplicationForm.Visibility = Visibility.Collapsed;
                StudentHistoryTable.Visibility = Visibility.Collapsed;
                AdminAllApplicationsTable.Visibility = Visibility.Visible;
                
                // 更新按钮文本
                SwitchToApplicationViewButton.Content = "转换为档案申请页";
            }
        }

        /// <summary>
        /// 管理员切换视图按钮点击事件
        /// </summary>
        private void SwitchToApplicationViewButton_Click(object sender, RoutedEventArgs e)
        {
            SetAdminView(!_isAdminInApplicationView);
            RefreshData();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            if (!UserManager.Instance.IsAuthed) return;

            var userRole = UserManager.Instance.AuthedUser?.GrantedType;

            switch (userRole)
            {
                case "Student":
                    LoadStudentApplications();
                    break;
                    
                case "Admin":
                    if (_isAdminInApplicationView)
                    {
                        // 管理员在申请视图时，不加载特定学生数据，让用户手动输入
                        _studentApplications.Clear();
                        StudentApplicationsListView.ItemsSource = _studentApplications;
                    }
                    else
                    {
                        LoadAllApplications();
                    }
                    break;
            }
        }

        /// <summary>
        /// 加载学生的申请记录
        /// </summary>
        private void LoadStudentApplications()
        {
            if (UserManager.Instance.AuthedUser == null) return;

            var studentId = UserManager.Instance.AuthedUser.UserId;
            var applications = FileTransferApplicationManager.Instance
                .GetApplicationsByStudentId(studentId);

            _studentApplications.Clear();
            foreach (var app in applications)
            {
                _studentApplications.Add(app);
            }

            StudentApplicationsListView.ItemsSource = _studentApplications;
        }

        /// <summary>
        /// 加载所有申请记录（管理员）
        /// </summary>
        private void LoadAllApplications()
        {
            AdminApplicationsListView.ItemsSource = 
                FileTransferApplicationManager.Instance.Applications;
        }

        /// <summary>
        /// 提交申请按钮点击事件
        /// </summary>
        private void SubmitApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            // 验证表单数据
            if (string.IsNullOrWhiteSpace(StudentIdTextBox.Text) ||
                string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(AddressTextBox.Text) ||
                string.IsNullOrWhiteSpace(TelephoneTextBox.Text))
            {
                ShowMessage("请填写所有必填字段");
                return;
            }

            // 创建新的申请
            var application = new FileTransferApplication
            {
                StudentId = StudentIdTextBox.Text.Trim(),
                Name = NameTextBox.Text.Trim(),
                Address = AddressTextBox.Text.Trim(),
                Telephone = TelephoneTextBox.Text.Trim(),
                Detail = DetailTextBox.Text.Trim(),
                State = TransferState.档案预备中,
                CreatedAt = DateTime.Now
            };

            // 添加申请
            FileTransferApplicationManager.Instance.AddApplication(application);

            // 清空表单
            ClearForm();

            // 刷新数据
            if (UserManager.Instance.AuthedUser?.GrantedType == "Student")
            {
                LoadStudentApplications();
            }
            else if (_isAdminInApplicationView)
            {
                // 管理员在申请视图时，添加到显示列表
                _studentApplications.Add(application);
            }

            ShowMessage("申请提交成功！");
        }

        /// <summary>
        /// 状态下拉框选择改变事件（管理员）
        /// </summary>
        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && 
                comboBox.Tag is FileTransferApplication application &&
                comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var newStateString = selectedItem.Content?.ToString();
                if (!string.IsNullOrEmpty(newStateString))
                {
                    var newState = newStateString switch
                    {
                        "档案预备中" => TransferState.档案预备中,
                        "转递中" => TransferState.转递中,
                        "已完成" => TransferState.已完成,
                        _ => TransferState.档案预备中
                    };

                    // 更新申请状态
                    FileTransferApplicationManager.Instance
                        .UpdateApplicationState(application.Id, newState);
                }
            }
        }

        /// <summary>
        /// 清空表单
        /// </summary>
        private void ClearForm()
        {
            // 学生用户不清空学号，管理员用户可以清空
            if (UserManager.Instance.AuthedUser?.GrantedType == "Student")
            {
                // 学生用户保持学号不变
            }
            else
            {
                StudentIdTextBox.Text = string.Empty;
            }
            
            NameTextBox.Text = string.Empty;
            AddressTextBox.Text = string.Empty;
            TelephoneTextBox.Text = string.Empty;
            DetailTextBox.Text = string.Empty;
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

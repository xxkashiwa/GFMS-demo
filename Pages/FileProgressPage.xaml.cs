using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using GFMS.Models;
using GFMS.Services;
using GFMS.Dialogs;

namespace GFMS.Pages
{
    /// <summary>
    /// 档案传递页面
    /// </summary>
    public sealed partial class FileProgressPage : Page
    {
        private readonly ArchiveTransferService _transferService;
        private ObservableCollection<ArchiveRequest> _applicationRequests = new();
        private ObservableCollection<ArchiveRequest> _organizeRequests = new();
        private ObservableCollection<ArchiveRequest> _transferRecords = new();
        private ArchiveRequest? _selectedRequest;

        public FileProgressPage()
        {
            InitializeComponent();
            _transferService = ArchiveTransferService.Instance;
            InitializePagePermissions();
            LoadData();
        }

        private void InitializePagePermissions()
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser == null) return;

            // 根据用户角色控制界面元素的可见性
            if (currentUser.Role == "Student")
            {
                // 学生只能看到自己的申请和转递状态
                // 隐藏管理员功能
                //ApproveApplicationButton.Visibility = Visibility.Collapsed;
                //RejectApplicationButton.Visibility = Visibility.Collapsed;
                //GenerateTransferLetterButton.Visibility = Visibility.Collapsed;
                //RecordShipmentButton.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoadData();
        }

        private void LoadData()
        {
            LoadApplicationRequests();
            LoadOrganizeRequests();
            LoadTransferRecords();
        }

        private void LoadApplicationRequests()
        {
            var requests = _transferService.GetArchiveRequests();
            _applicationRequests.Clear();
            foreach (var request in requests)
            {
                _applicationRequests.Add(request);
            }
            ApplicationList.ItemsSource = _applicationRequests;
        }

        private void LoadOrganizeRequests()
        {
            var requests = _transferService.GetArchiveRequestsByStatus("已通过");
            _organizeRequests.Clear();
            foreach (var request in requests)
            {
                _organizeRequests.Add(request);
            }
            OrganizeTaskList.ItemsSource = _organizeRequests;
        }

        private void LoadTransferRecords()
        {
            var records = _transferService.GetTransferRecords();
            _transferRecords.Clear();
            foreach (var record in records)
            {
                _transferRecords.Add(record);
            }
            TransferRecordList.ItemsSource = _transferRecords;
        }

        #region 调档申请标签页事件处理

        private async void AddApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_transferService.CanCreateRequest())
            {
                await ShowErrorDialog("权限不足", "只有学生可以提交调档申请");
                return;
            }

            var dialog = new ArchiveRequestDialog();
            dialog.XamlRoot = this.XamlRoot;

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary && dialog.Request != null)
            {
                var success = await _transferService.CreateArchiveRequestAsync(dialog.Request);
                if (success)
                {
                    await ShowInfoDialog("提交成功", "调档申请已提交，请等待审核");
                    LoadApplicationRequests();
                }
                else
                {
                    await ShowErrorDialog("提交失败", "提交调档申请时出现错误");
                }
            }
        }

        private async void ExportApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            await ShowInfoDialog("导出申请单", "导出功能开发中...");
        }

        private void ApplicationStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox?.SelectedItem is ComboBoxItem selectedItem)
            {
                string status = selectedItem.Content.ToString();
                FilterApplicationsByStatus(status);
            }
        }

        private void FilterApplicationsByStatus(string status)
        {
            var filteredRequests = _transferService.GetArchiveRequestsByStatus(status);
            _applicationRequests.Clear();
            foreach (var request in filteredRequests)
            {
                _applicationRequests.Add(request);
            }
        }

        private void ApplicationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ArchiveRequest selectedRequest)
            {
                _selectedRequest = selectedRequest;
                LoadApplicationDetails(selectedRequest);
            }
        }

        private void LoadApplicationDetails(ArchiveRequest request)
        {
            // 这里可以更新右侧详情面板
            // 由于XAML中的详情面板是静态的，这里暂时不更新
        }

        private async void ProcessApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ArchiveRequest request)
            {
                _selectedRequest = request;
                await ShowInfoDialog("处理申请", $"正在处理 {request.Student?.Name} 的调档申请");
            }
        }

        private async void ApproveApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_transferService.CanReviewRequest())
            {
                await ShowErrorDialog("权限不足", "只有管理员和老师可以审核申请");
                return;
            }

            if (_selectedRequest == null)
            {
                await ShowErrorDialog("请选择申请", "请先选择要审核的申请");
                return;
            }

            bool confirmed = await ShowConfirmDialog("确认批准", "确定要批准这个调档申请吗？");
            if (confirmed)
            {
                var success = await _transferService.ReviewArchiveRequestAsync(_selectedRequest.Id, "已通过");
                if (success)
                {
                    await ShowInfoDialog("操作成功", "申请已批准");
                    LoadApplicationRequests();
                    LoadOrganizeRequests();
                }
                else
                {
                    await ShowErrorDialog("操作失败", "批准申请时出现错误");
                }
            }
        }

        private async void RejectApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_transferService.CanReviewRequest())
            {
                await ShowErrorDialog("权限不足", "只有管理员和老师可以审核申请");
                return;
            }

            if (_selectedRequest == null)
            {
                await ShowErrorDialog("请选择申请", "请先选择要审核的申请");
                return;
            }

            bool confirmed = await ShowConfirmDialog("确认拒绝", "确定要拒绝这个调档申请吗？");
            if (confirmed)
            {
                var success = await _transferService.ReviewArchiveRequestAsync(_selectedRequest.Id, "已拒绝");
                if (success)
                {
                    await ShowInfoDialog("操作成功", "申请已拒绝");
                    LoadApplicationRequests();
                }
                else
                {
                    await ShowErrorDialog("操作失败", "拒绝申请时出现错误");
                }
            }
        }

        private async void SaveDraftApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            await ShowInfoDialog("保存成功", "审核意见已暂存");
        }

        #endregion

        #region 档案整理标签页事件处理

        private async void GenerateTransferLetterButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_transferService.CanManageTransfer())
            {
                await ShowErrorDialog("权限不足", "只有管理员可以生成调档函");
                return;
            }

            await ShowInfoDialog("生成调档函", "批量生成调档函功能开发中...");
        }

        private async void PrintLabelButton_Click(object sender, RoutedEventArgs e)
        {
            await ShowInfoDialog("打印标签", "打印标签功能开发中...");
        }

        private async void BatchOperationButton_Click(object sender, RoutedEventArgs e)
        {
            await ShowInfoDialog("批量操作", "批量操作功能开发中...");
        }

        private void OrganizeStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox?.SelectedItem is ComboBoxItem selectedItem)
            {
                string status = selectedItem.Content.ToString();
                FilterOrganizeFilesByStatus(status);
            }
        }

        private void FilterOrganizeFilesByStatus(string status)
        {
            // TODO: 根据整理状态筛选档案列表
        }

        private void OrganizeTaskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ArchiveRequest selectedRequest)
            {
                _selectedRequest = selectedRequest;
                LoadOrganizeDetails(selectedRequest);
            }
        }

        private void LoadOrganizeDetails(ArchiveRequest request)
        {
            // 更新右侧整理详情面板
            // 这里可以根据实际需要加载档案整理进度
        }

        private async void GenerateTransferLetterDetailButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_transferService.CanManageTransfer())
            {
                await ShowErrorDialog("权限不足", "只有管理员可以生成调档函");
                return;
            }

            if (_selectedRequest == null)
            {
                await ShowErrorDialog("请选择申请", "请先选择要生成调档函的申请");
                return;
            }

            var dialog = new TransferLetterDialog(_selectedRequest);
            dialog.XamlRoot = this.XamlRoot;

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var letterNumber = await _transferService.GenerateTransferLetterAsync(_selectedRequest.Id, "调档函已生成");
                if (!string.IsNullOrEmpty(letterNumber))
                {
                    await ShowInfoDialog("生成成功", $"调档函已生成，编号：{letterNumber}");
                }
                else
                {
                    await ShowErrorDialog("生成失败", "生成调档函时出现错误");
                }
            }
        }

        private async void PrintArchiveLabelButton_Click(object sender, RoutedEventArgs e)
        {
            await ShowInfoDialog("打印标签", "档案袋标签打印功能开发中...");
        }

        private async void MarkCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool confirmed = await ShowConfirmDialog("确认完成", "确定要标记此档案整理为完成状态吗？");
            if (confirmed)
            {
                await ShowInfoDialog("操作成功", "档案整理已标记为完成");
            }
        }

        #endregion

        #region 转递记录标签页事件处理

        private async void RecordShipmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_transferService.CanManageTransfer())
            {
                await ShowErrorDialog("权限不足", "只有管理员可以记录寄出信息");
                return;
            }

            await ShowShipmentDialog();
        }

        private async Task ShowShipmentDialog()
        {
            var dialog = new ContentDialog()
            {
                Title = "记录寄出信息",
                PrimaryButtonText = "确定",
                CloseButtonText = "取消",
                XamlRoot = this.XamlRoot
            };

            var panel = new StackPanel { Spacing = 10 };

            var studentComboBox = new ComboBox
            {
                Header = "选择学生",
                PlaceholderText = "请选择要记录的学生",
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            var approvedRequests = _transferService.GetArchiveRequestsByStatus("已通过");
            foreach (var request in approvedRequests)
            {
                studentComboBox.Items.Add(new ComboBoxItem
                {
                    Content = $"{request.Student?.Name} ({request.Student?.StudentId})",
                    Tag = request
                });
            }

            var trackingNumberTextBox = new TextBox
            {
                Header = "快递单号",
                PlaceholderText = "请输入快递单号"
            };

            var expressCompanyComboBox = new ComboBox
            {
                Header = "快递公司",
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            expressCompanyComboBox.Items.Add(new ComboBoxItem { Content = "顺丰速运" });
            expressCompanyComboBox.Items.Add(new ComboBoxItem { Content = "中通快递" });
            expressCompanyComboBox.Items.Add(new ComboBoxItem { Content = "圆通速递" });
            expressCompanyComboBox.Items.Add(new ComboBoxItem { Content = "申通快递" });
            expressCompanyComboBox.Items.Add(new ComboBoxItem { Content = "韵达快递" });

            panel.Children.Add(studentComboBox);
            panel.Children.Add(trackingNumberTextBox);
            panel.Children.Add(expressCompanyComboBox);

            dialog.Content = panel;

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                if (studentComboBox.SelectedItem is ComboBoxItem selectedItem &&
                    selectedItem.Tag is ArchiveRequest request &&
                    !string.IsNullOrWhiteSpace(trackingNumberTextBox.Text) &&
                    expressCompanyComboBox.SelectedItem is ComboBoxItem selectedCompany)
                {
                    var success = await _transferService.RecordDispatchAsync(
                        request.Id,
                        trackingNumberTextBox.Text);

                    if (success)
                    {
                        await ShowInfoDialog("记录成功", "寄出信息已记录");
                        LoadTransferRecords();
                    }
                    else
                    {
                        await ShowErrorDialog("记录失败", "记录寄出信息时出现错误");
                    }
                }
                else
                {
                    await ShowErrorDialog("输入不完整", "请填写完整的寄出信息");
                }
            }
        }

        private async void BatchInputTrackingButton_Click(object sender, RoutedEventArgs e)
        {
            await ShowInfoDialog("批量录入", "批量录入快递单号功能开发中...");
        }

        private void TransferStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox?.SelectedItem is ComboBoxItem selectedItem)
            {
                string status = selectedItem.Content.ToString();
                FilterTransferRecordsByStatus(status);
            }
        }

        private void FilterTransferRecordsByStatus(string status)
        {
            var allRecords = _transferService.GetTransferRecords();
            var filteredRecords = status == "全部" ? allRecords : allRecords.Where(r => r.Status == status).ToList();

            _transferRecords.Clear();
            foreach (var record in filteredRecords)
            {
                _transferRecords.Add(record);
            }
        }

        private async void ExportTransferListButton_Click(object sender, RoutedEventArgs e)
        {
            await ShowInfoDialog("导出清单", "导出转递清单功能开发中...");
        }

        private async void TrackPackageButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ArchiveRequest record)
            {
                var message = $"快递单号：{record.TrackingNumber}\n状态：{record.Status}";
                await ShowInfoDialog("快递跟踪", message);
            }
        }

        private async void ViewDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ArchiveRequest record)
            {
                var details = $"学生：{record.Student?.Name} ({record.Student?.StudentId})\n" +
                             $"接收单位：{record.ReceiverName}\n" +
                             $"接收地址：{record.ReceiverAddress}\n" +
                             $"快递单号：{record.TrackingNumber}\n" +
                             $"寄出时间：{record.DispatchDate?.ToString("yyyy-MM-dd")}\n" +
                             $"状态：{record.Status}";

                await ShowInfoDialog("转递详情", details);
            }
        }

        #endregion

        #region 材料清单事件处理

        private void MaterialCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                UpdateMaterialIcon(checkBox, true);
                UpdateOrganizeProgress();
            }
        }

        private void MaterialCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                UpdateMaterialIcon(checkBox, false);
                UpdateOrganizeProgress();
            }
        }

        private void UpdateMaterialIcon(CheckBox checkBox, bool isChecked)
        {
            FontIcon? icon = null;

            switch (checkBox.Name)
            {
                case "GraduateFormCheckBox":
                    icon = GraduateFormIcon;
                    break;
                case "TranscriptCheckBox":
                    icon = TranscriptIcon;
                    break;
                case "MedicalCheckBox":
                    icon = MedicalIcon;
                    break;
                case "RewardPunishmentCheckBox":
                    icon = RewardPunishmentIcon;
                    break;
            }

            if (icon != null)
            {
                if (isChecked)
                {
                    icon.Glyph = "\uE73E"; // 勾选图标
                    icon.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green);
                }
                else
                {
                    icon.Glyph = "\uE711"; // 警告图标
                    icon.Foreground = new SolidColorBrush(Microsoft.UI.Colors.Orange);
                }
            }
        }

        private void UpdateOrganizeProgress()
        {
            int checkedCount = 0;
            int totalCount = 4;

            if (GraduateFormCheckBox?.IsChecked == true) checkedCount++;
            if (TranscriptCheckBox?.IsChecked == true) checkedCount++;
            if (MedicalCheckBox?.IsChecked == true) checkedCount++;
            if (RewardPunishmentCheckBox?.IsChecked == true) checkedCount++;

            double progressValue = (double)checkedCount / totalCount * 100;
            if (OrganizeProgressBar != null)
            {
                OrganizeProgressBar.Value = progressValue;
            }

            if (OrganizeProgressText != null)
            {
                OrganizeProgressText.Text = $"{progressValue:F0}% 完成 ({checkedCount}/{totalCount} 项)";
            }
        }

        #endregion

        #region 通用对话框方法

        private async Task ShowInfoDialog(string title, string message)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = title,
                Content = message,
                CloseButtonText = "确定",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async Task ShowErrorDialog(string title, string message)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = title,
                Content = message,
                CloseButtonText = "确定",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async Task<bool> ShowConfirmDialog(string title, string message)
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = title,
                Content = message,
                PrimaryButtonText = "确定",
                CloseButtonText = "取消",
                XamlRoot = this.XamlRoot
            };
            var result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        #endregion
    }
}

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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GFMS.Pages
{
    /// <summary>
    /// 档案传递页面
    /// </summary>
    public sealed partial class FileProgressPage : Page
    {
        public FileProgressPage()
        {
            InitializeComponent();
            LoadSampleData();
        }

        private void LoadSampleData()
        {
            // TODO: 从数据库加载实际数据
            // 这里暂时使用示例数据
        }

        #region 调档申请标签页事件处理

        private async void NewApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 打开新增申请对话框
            await ShowInfoDialog("新增申请", "新增申请功能开发中...");
        }

        private async void AddApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 新增调档申请
            await ShowInfoDialog("新增申请", "新增调档申请功能开发中...");
        }

        private async void ExportApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 导出申请单到Excel或PDF
            await ShowInfoDialog("导出申请单", "导出申请单功能开发中...");
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
            // TODO: 根据状态筛选申请列表
            // 实现数据筛选逻辑
        }

        private async void ProcessApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 处理选中的申请
            await ShowInfoDialog("处理申请", "处理申请功能开发中...");
        }

        private async void ApproveApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            bool confirmed = await ShowConfirmDialog("确认批准", "确定要批准这个调档申请吗？");
            if (confirmed)
            {
                // TODO: 批准申请的业务逻辑
                await ShowInfoDialog("操作成功", "申请已批准");
            }
        }

        private async void RejectApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            bool confirmed = await ShowConfirmDialog("确认拒绝", "确定要拒绝这个调档申请吗？");
            if (confirmed)
            {
                // TODO: 拒绝申请的业务逻辑
                await ShowInfoDialog("操作成功", "申请已拒绝");
            }
        }

        private async void SaveDraftApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 保存申请为草稿状态
            await ShowInfoDialog("保存成功", "申请已保存为草稿");
        }

        #endregion

        #region 档案整理标签页事件处理

        private async void GenerateTransferLetterButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 生成调档函
            await ShowInfoDialog("生成调档函", "生成调档函功能开发中...");
        }

        private async void PrintLabelButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 打印档案袋标签
            await ShowInfoDialog("打印标签", "打印标签功能开发中...");
        }

        private async void BatchOperationButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 批量操作
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
            // 实现数据筛选逻辑
        }

        private async void GenerateTransferLetterDetailButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 为选中档案生成调档函
            await ShowInfoDialog("生成调档函", "调档函生成功能开发中...");
        }

        private async void PrintArchiveLabelButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 打印档案袋标签
            await ShowInfoDialog("打印标签", "档案袋标签打印功能开发中...");
        }

        private async void MarkCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool confirmed = await ShowConfirmDialog("确认完成", "确定要标记此档案整理为完成状态吗？");
            if (confirmed)
            {
                // TODO: 标记档案整理完成的业务逻辑
                await ShowInfoDialog("操作成功", "档案整理已标记为完成");
            }
        }

        #endregion

        #region 转递记录标签页事件处理

        private async void RecordShipmentButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 记录档案寄出信息
            await ShowInfoDialog("记录寄出", "记录寄出功能开发中...");
        }

        private async void BatchInputTrackingButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 批量录入快递单号
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
            // TODO: 根据转递状态筛选记录列表
            // 实现数据筛选逻辑
        }

        private async void ExportTransferListButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 导出转递清单
            await ShowInfoDialog("导出清单", "导出转递清单功能开发中...");
        }

        private async void TrackPackageButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 跟踪快递包裹
            await ShowInfoDialog("跟踪包裹", "快递跟踪功能开发中...");
        }

        private async void ViewDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 查看转递详情
            await ShowInfoDialog("查看详情", "转递详情查看功能开发中...");
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
            
            // 根据CheckBox的名称找到对应的图标
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
             // 计算已选中的材料数量
             int checkedCount = 0;
             int totalCount = 4;

             if (GraduateFormCheckBox?.IsChecked == true) checkedCount++;
             if (TranscriptCheckBox?.IsChecked == true) checkedCount++;
             if (MedicalCheckBox?.IsChecked == true) checkedCount++;
             if (RewardPunishmentCheckBox?.IsChecked == true) checkedCount++;

             // 更新进度条
             double progressValue = (double)checkedCount / totalCount * 100;
             if (OrganizeProgressBar != null)
             {
                 OrganizeProgressBar.Value = progressValue;
             }

             // 更新进度文本显示
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

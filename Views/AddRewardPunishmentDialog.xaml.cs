using GFMS.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace GFMS.Views
{
    /// <summary>
    /// 添加奖惩记录对话框
    /// </summary>
    public sealed partial class AddRewardPunishmentDialog : ContentDialog
    {
        public RewardAndPunishment NewRewardPunishment { get; private set; }

        public AddRewardPunishmentDialog()
        {
            this.InitializeComponent();
            
            // 设置默认日期为今天
            DatePicker.Date = DateTimeOffset.Now;
            
            // 设置默认类型
            TypeComboBox.SelectedIndex = 0;
            
            // 绑定主按钮点击事件
            this.PrimaryButtonClick += ContentDialog_PrimaryButtonClick;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 验证输入
            if (!ValidateInput())
            {
                args.Cancel = true;
                return;
            }

            // 获取选中的类型
            var selectedItem = (ComboBoxItem)TypeComboBox.SelectedItem;
            var typeTag = selectedItem.Tag.ToString();
            var recordType = typeTag == "Reward" ? RecordType.Reward : RecordType.Punishment;

            // 创建新奖惩记录对象
            NewRewardPunishment = new RewardAndPunishment
            {
                Type = recordType,
                Details = DetailsTextBox.Text.Trim(),
                Date = DatePicker.Date.DateTime
            };
        }

        private bool ValidateInput()
        {
            ErrorTextBlock.Visibility = Visibility.Collapsed;

            // 验证类型
            if (TypeComboBox.SelectedItem == null)
            {
                ShowError("请选择奖惩类型");
                return false;
            }

            // 验证详细信息
            if (string.IsNullOrWhiteSpace(DetailsTextBox.Text))
            {
                ShowError("请输入详细信息");
                return false;
            }

            return true;
        }

        private void ShowError(string message)
        {
            ErrorTextBlock.Text = message;
            ErrorTextBlock.Visibility = Visibility.Visible;
        }
    }
}
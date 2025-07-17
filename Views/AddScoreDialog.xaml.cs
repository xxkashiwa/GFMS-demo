using GFMS.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace GFMS.Views
{
    /// <summary>
    /// 添加成绩对话框
    /// </summary>
    public sealed partial class AddScoreDialog : ContentDialog
    {
        public string Semester { get; private set; }
        public Score NewScore { get; private set; }

        public AddScoreDialog()
        {
            this.InitializeComponent();
            
            // 设置默认学期为当前学年第一学期
            SemesterComboBox.SelectedIndex = 2; // 2024-2025学年第一学期
            
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

            // 获取学期信息
            Semester = GetSemesterText();

            // 创建新成绩对象
            NewScore = new Score
            {
                Subject = GetSubjectText(),
                ScoreValue = ScoreNumberBox.Value
            };
        }

        private bool ValidateInput()
        {
            ErrorTextBlock.Visibility = Visibility.Collapsed;

            // 验证学期
            if (string.IsNullOrWhiteSpace(GetSemesterText()))
            {
                ShowError("请选择或输入学期");
                return false;
            }

            // 验证学科
            if (string.IsNullOrWhiteSpace(GetSubjectText()))
            {
                ShowError("请选择或输入学科");
                return false;
            }

            // 验证分数
            if (double.IsNaN(ScoreNumberBox.Value))
            {
                ShowError("请输入有效的分数");
                return false;
            }

            if (ScoreNumberBox.Value < 0 || ScoreNumberBox.Value > 100)
            {
                ShowError("分数必须在0-100之间");
                return false;
            }

            return true;
        }

        private string GetSemesterText()
        {
            if (SemesterComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                return selectedItem.Content.ToString();
            }
            else
            {
                return SemesterComboBox.Text?.Trim() ?? string.Empty;
            }
        }

        private string GetSubjectText()
        {
            if (SubjectComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                return selectedItem.Content.ToString();
            }
            else
            {
                return SubjectComboBox.Text?.Trim() ?? string.Empty;
            }
        }

        private void ShowError(string message)
        {
            ErrorTextBlock.Text = message;
            ErrorTextBlock.Visibility = Visibility.Visible;
        }
    }
}
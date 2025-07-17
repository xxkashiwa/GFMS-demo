using GFMS.Models;
using GFMS.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Linq;

namespace GFMS.Views
{
    /// <summary>
    /// 添加学生信息对话框
    /// </summary>
    public sealed partial class AddStudentDialog : ContentDialog
    {
        public Student NewStudent { get; private set; }

        public AddStudentDialog()
        {
            this.InitializeComponent();
            
            // 设置默认毕业时间为当前年份的6月
            GraduationDatePicker.Date = new DateTimeOffset(DateTime.Now.Year, 6, 30, 0, 0, 0, TimeSpan.Zero);
            
            // 设置默认性别
            GenderComboBox.SelectedIndex = 0;
            
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

            // 创建新学生对象
            NewStudent = new Student
            {
                StudentId = StudentIdTextBox.Text.Trim(),
                Name = NameTextBox.Text.Trim(),
                Gender = ((ComboBoxItem)GenderComboBox.SelectedItem).Content.ToString(),
                GraduationDate = GraduationDatePicker.Date.DateTime
            };

            // 添加到学生管理器
            StudentManager.Instance.AddStudent(NewStudent);
        }

        private bool ValidateInput()
        {
            ErrorTextBlock.Visibility = Visibility.Collapsed;

            // 验证学号
            if (string.IsNullOrWhiteSpace(StudentIdTextBox.Text))
            {
                ShowError("请输入学号");
                return false;
            }

            // 检查学号是否已存在
            if (StudentManager.Instance.Students.Any(s => s.StudentId == StudentIdTextBox.Text.Trim()))
            {
                ShowError("该学号已存在，请输入其他学号");
                return false;
            }

            // 验证姓名
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                ShowError("请输入姓名");
                return false;
            }

            // 验证性别
            if (GenderComboBox.SelectedItem == null)
            {
                ShowError("请选择性别");
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
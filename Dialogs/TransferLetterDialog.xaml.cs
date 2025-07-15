using GFMS.Models;
using GFMS.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Markup;

namespace GFMS.Dialogs
{
    [ContentProperty(Name = "Content")]
    public sealed partial class TransferLetterDialog : ContentDialog
    {
        public string TransferLetterNumber { get; private set; } = "";
        private ArchiveRequest _request;

        // 创建临时控件以避免编译错误
        private TextBox LetterNumberTextBox = new TextBox();
        private DatePicker TransferDatePicker = new DatePicker();
        private TextBox StudentIdTextBox = new TextBox();
        private TextBox StudentNameTextBox = new TextBox();
        private TextBox MajorTextBox = new TextBox();
        private TextBox GraduationYearTextBox = new TextBox();
        private TextBox ReceiverNameTextBox = new TextBox();
        private TextBox ReceiverAddressTextBox = new TextBox();
        private TextBox ReceiverContactTextBox = new TextBox();
        private TextBox ReceiverPhoneTextBox = new TextBox();
        private TextBox RemarksTextBox = new TextBox();
        private TextBox OtherMaterialsTextBox = new TextBox();
        private CheckBox GraduateFormCheckBox = new CheckBox();
        private CheckBox TranscriptCheckBox = new CheckBox();
        private CheckBox MedicalCheckBox = new CheckBox();
        private CheckBox RewardPunishmentCheckBox = new CheckBox();
        private CheckBox ThesisDefenseCheckBox = new CheckBox();
        private CheckBox InternshipReportCheckBox = new CheckBox();
        private TextBlock PreviewContent = new TextBlock();

        public TransferLetterDialog(ArchiveRequest request)
        {
            // 注意：如果没有对应的XAML文件，不能调用InitializeComponent()
            // this.InitializeComponent();

            _request = request;
            InitializeDialog();
            LoadRequestInfo();
            GenerateLetterNumber();
            UpdatePreview();

            // 设置按钮点击事件
            this.PrimaryButtonClick += TransferLetterDialog_PrimaryButtonClick;
        }

        private void InitializeDialog()
        {
            // 手动初始化对话框
            this.Title = "生成调档函";
            this.PrimaryButtonText = "生成";
            this.CloseButtonText = "取消";

            // 创建内容区域
            var stackPanel = new StackPanel();

            // 添加基本信息区域
            var infoGrid = CreateInfoGrid();
            stackPanel.Children.Add(infoGrid);

            // 添加材料清单区域
            var materialsPanel = CreateMaterialsPanel();
            stackPanel.Children.Add(materialsPanel);

            // 添加预览区域
            var previewPanel = CreatePreviewPanel();
            stackPanel.Children.Add(previewPanel);

            this.Content = stackPanel;
        }

        private Grid CreateInfoGrid()
        {
            var grid = new Grid();
            grid.Margin = new Thickness(10);

            // 定义行和列
            for (int i = 0; i < 6; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // 调档函编号
            var letterNumberLabel = new TextBlock { Text = "调档函编号:", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(letterNumberLabel, 0);
            Grid.SetColumn(letterNumberLabel, 0);
            grid.Children.Add(letterNumberLabel);

            Grid.SetRow(LetterNumberTextBox, 0);
            Grid.SetColumn(LetterNumberTextBox, 1);
            grid.Children.Add(LetterNumberTextBox);

            // 学生信息
            var studentIdLabel = new TextBlock { Text = "学号:", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(studentIdLabel, 1);
            Grid.SetColumn(studentIdLabel, 0);
            grid.Children.Add(studentIdLabel);

            Grid.SetRow(StudentIdTextBox, 1);
            Grid.SetColumn(StudentIdTextBox, 1);
            StudentIdTextBox.IsReadOnly = true;
            grid.Children.Add(StudentIdTextBox);

            var studentNameLabel = new TextBlock { Text = "姓名:", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(studentNameLabel, 2);
            Grid.SetColumn(studentNameLabel, 0);
            grid.Children.Add(studentNameLabel);

            Grid.SetRow(StudentNameTextBox, 2);
            Grid.SetColumn(StudentNameTextBox, 1);
            StudentNameTextBox.IsReadOnly = true;
            grid.Children.Add(StudentNameTextBox);

            // 接收单位信息
            var receiverLabel = new TextBlock { Text = "接收单位:", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(receiverLabel, 3);
            Grid.SetColumn(receiverLabel, 0);
            grid.Children.Add(receiverLabel);

            Grid.SetRow(ReceiverNameTextBox, 3);
            Grid.SetColumn(ReceiverNameTextBox, 1);
            ReceiverNameTextBox.IsReadOnly = true;
            grid.Children.Add(ReceiverNameTextBox);

            // 寄送时间
            var transferDateLabel = new TextBlock { Text = "寄送时间:", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(transferDateLabel, 4);
            Grid.SetColumn(transferDateLabel, 0);
            grid.Children.Add(transferDateLabel);

            Grid.SetRow(TransferDatePicker, 4);
            Grid.SetColumn(TransferDatePicker, 1);
            grid.Children.Add(TransferDatePicker);

            // 备注
            var remarksLabel = new TextBlock { Text = "备注:", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(remarksLabel, 5);
            Grid.SetColumn(remarksLabel, 0);
            grid.Children.Add(remarksLabel);

            Grid.SetRow(RemarksTextBox, 5);
            Grid.SetColumn(RemarksTextBox, 1);
            RemarksTextBox.AcceptsReturn = true;
            RemarksTextBox.Height = 60;
            grid.Children.Add(RemarksTextBox);

            return grid;
        }

        private StackPanel CreateMaterialsPanel()
        {
            var panel = new StackPanel();
            panel.Margin = new Thickness(10);

            var header = new TextBlock
            {
                Text = "材料清单:",
                FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                Margin = new Thickness(0, 10, 0, 5)
            };
            panel.Children.Add(header);

            panel.Children.Add(GraduateFormCheckBox);
            GraduateFormCheckBox.Content = "毕业生登记表";

            panel.Children.Add(TranscriptCheckBox);
            TranscriptCheckBox.Content = "学习成绩单";

            panel.Children.Add(MedicalCheckBox);
            MedicalCheckBox.Content = "体检表";

            panel.Children.Add(RewardPunishmentCheckBox);
            RewardPunishmentCheckBox.Content = "奖惩材料";

            panel.Children.Add(ThesisDefenseCheckBox);
            ThesisDefenseCheckBox.Content = "论文答辩记录";

            panel.Children.Add(InternshipReportCheckBox);
            InternshipReportCheckBox.Content = "实习报告";

            var otherLabel = new TextBlock { Text = "其他材料:", Margin = new Thickness(0, 10, 0, 5) };
            panel.Children.Add(otherLabel);

            panel.Children.Add(OtherMaterialsTextBox);
            OtherMaterialsTextBox.PlaceholderText = "请输入其他材料名称";

            return panel;
        }

        private StackPanel CreatePreviewPanel()
        {
            var panel = new StackPanel();
            panel.Margin = new Thickness(10);

            var header = new TextBlock
            {
                Text = "调档函预览:",
                FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                Margin = new Thickness(0, 10, 0, 5)
            };
            panel.Children.Add(header);

            var scrollViewer = new ScrollViewer();
            scrollViewer.Height = 200;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            PreviewContent.TextWrapping = TextWrapping.Wrap;
            PreviewContent.Margin = new Thickness(5);
            scrollViewer.Content = PreviewContent;

            panel.Children.Add(scrollViewer);

            return panel;
        }

        private async void TransferLetterDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // 验证输入数据
            if (TransferDatePicker.Date == null)
            {
                args.Cancel = true;
                await ShowErrorDialog("请选择寄送时间");
                return;
            }

            var materials = GetSelectedMaterials();
            if (materials.Count == 0)
            {
                args.Cancel = true;
                await ShowErrorDialog("请选择至少一种材料");
                return;
            }

            TransferLetterNumber = LetterNumberTextBox.Text;
        }

        private void LoadRequestInfo()
        {
            if (_request?.Student != null)
            {
                StudentIdTextBox.Text = _request.Student.StudentId;
                StudentNameTextBox.Text = _request.Student.Name;
                MajorTextBox.Text = _request.Student.Major ?? "";
                GraduationYearTextBox.Text = _request.Student.GraduationYear?.ToString() ?? "";
            }

            if (_request != null)
            {
                ReceiverNameTextBox.Text = _request.ReceiverName ?? "";
                ReceiverAddressTextBox.Text = _request.ReceiverAddress ?? "";
            }

            // 移除了联系人和电话字段，因为新表结构中没有这些字段
            ReceiverContactTextBox.Text = "";
            ReceiverPhoneTextBox.Text = "";

            // 设置默认的寄送时间为明天
            TransferDatePicker.Date = DateTimeOffset.Now.AddDays(1);

            // 绑定事件实时更新预览
            TransferDatePicker.DateChanged += (s, e) => UpdatePreview();
            RemarksTextBox.TextChanged += (s, e) => UpdatePreview();
            OtherMaterialsTextBox.TextChanged += (s, e) => UpdatePreview();

            // 为所有复选框绑定事件
            var checkBoxes = new[] {
                GraduateFormCheckBox, TranscriptCheckBox, MedicalCheckBox,
                RewardPunishmentCheckBox, ThesisDefenseCheckBox, InternshipReportCheckBox
            };

            foreach (var checkBox in checkBoxes)
            {
                checkBox.Checked += (s, e) => UpdatePreview();
                checkBox.Unchecked += (s, e) => UpdatePreview();
            }
        }

        private void GenerateLetterNumber()
        {
            var year = DateTime.Now.Year;
            var sequence = new Random().Next(1, 9999); // 简单的序列号生成
            LetterNumberTextBox.Text = $"DF{year}{sequence:D4}";
        }

        private void UpdatePreview()
        {
            var materials = GetSelectedMaterials();
            var materialsText = string.Join("、", materials);

            var transferDate = TransferDatePicker.Date.ToString("yyyy年MM月dd日") ?? "____年__月__日";
            var remarks = string.IsNullOrWhiteSpace(RemarksTextBox.Text) ? "" : $"\n\n备注：{RemarksTextBox.Text}";

            // 由于新表结构中没有联系人和电话字段，我们从界面获取这些信息
            var contactInfo = "";
            if (!string.IsNullOrWhiteSpace(ReceiverContactTextBox.Text) || !string.IsNullOrWhiteSpace(ReceiverPhoneTextBox.Text))
            {
                contactInfo = $"\n联系人：{ReceiverContactTextBox.Text}\n联系电话：{ReceiverPhoneTextBox.Text}";
            }

            var content = $@"{ReceiverNameTextBox.Text}：

兹有{StudentNameTextBox.Text}（学号：{StudentIdTextBox.Text}）的档案申请，现将其学籍档案转递贵单位。

学生信息：
姓名：{StudentNameTextBox.Text}
学号：{StudentIdTextBox.Text}
专业：{MajorTextBox.Text}
毕业年份：{GraduationYearTextBox.Text}

接收单位：{ReceiverNameTextBox.Text}
接收地址：{ReceiverAddressTextBox.Text}{contactInfo}

随函转递材料清单：
{materialsText}

寄送时间：{transferDate}

请贵单位查收该学生学籍档案。{remarks}

此致
敬礼

档案管理中心
{DateTime.Now:yyyy年MM月dd日}";

            PreviewContent.Text = content;
        }

        private List<string> GetSelectedMaterials()
        {
            var materials = new List<string>();

            if (GraduateFormCheckBox.IsChecked == true) materials.Add("毕业生登记表");
            if (TranscriptCheckBox.IsChecked == true) materials.Add("学习成绩单");
            if (MedicalCheckBox.IsChecked == true) materials.Add("体检表");
            if (RewardPunishmentCheckBox.IsChecked == true) materials.Add("奖惩材料");
            if (ThesisDefenseCheckBox.IsChecked == true) materials.Add("论文答辩记录");
            if (InternshipReportCheckBox.IsChecked == true) materials.Add("实习报告");

            if (!string.IsNullOrWhiteSpace(OtherMaterialsTextBox.Text))
            {
                materials.Add(OtherMaterialsTextBox.Text.Trim());
            }

            return materials;
        }

        private async System.Threading.Tasks.Task ShowErrorDialog(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "输入错误",
                Content = message,
                CloseButtonText = "确定",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}
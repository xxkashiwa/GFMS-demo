using GFMS.Models;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Linq;

namespace GFMS.Dialogs
{
    public sealed partial class TransferStatusDialog : ContentDialog
    {
        private readonly ArchiveRequest _request;

        // 创建所有需要的UI控件
        private TextBlock StudentIdText = new TextBlock();
        private TextBlock StudentNameText = new TextBlock();
        private TextBlock MajorText = new TextBlock();
        private TextBlock GraduationYearText = new TextBlock();
        private TextBlock ReceiverNameText = new TextBlock();
        private TextBlock ReceiverAddressText = new TextBlock();
        private TextBlock RequestDateText = new TextBlock();
        private TextBlock ReasonText = new TextBlock();

        private StackPanel ExpressInfoPanel = new StackPanel();
        private TextBlock TrackingNumberText = new TextBlock();
        private TextBlock ExpressCompanyText = new TextBlock();
        private TextBlock DispatchDateText = new TextBlock();
        private TextBlock ReceiveDateText = new TextBlock();
        private Button TrackExpressButton = new Button();

        private StackPanel ReviewInfoPanel = new StackPanel();
        private TextBlock ReviewedByText = new TextBlock();
        private TextBlock ReviewDateText = new TextBlock();
        private TextBlock ReviewCommentText = new TextBlock();

        private StackPanel StatusTimeline = new StackPanel();

        public TransferStatusDialog(ArchiveRequest request)
        {
            // 不调用 InitializeComponent()，手动创建界面
            _request = request;
            InitializeDialog();
            LoadRequestInfo();
            BuildStatusTimeline();
        }

        private void InitializeDialog()
        {
            this.Title = "档案转递状态";
            this.CloseButtonText = "关闭";
            this.DefaultButton = ContentDialogButton.Close;

            // 创建主界面
            var mainPanel = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
            };

            var contentPanel = new StackPanel
            {
                Spacing = 20,
                Margin = new Thickness(20)
            };

            // 学生信息部分
            contentPanel.Children.Add(CreateStudentInfoPanel());

            // 申请信息部分
            contentPanel.Children.Add(CreateRequestInfoPanel());

            // 快递信息部分
            ExpressInfoPanel.Visibility = Visibility.Collapsed;
            contentPanel.Children.Add(CreateExpressInfoPanel());

            // 审核信息部分
            ReviewInfoPanel.Visibility = Visibility.Collapsed;
            contentPanel.Children.Add(CreateReviewInfoPanel());

            // 状态时间线
            contentPanel.Children.Add(CreateStatusTimelinePanel());

            mainPanel.Content = contentPanel;
            this.Content = mainPanel;
        }

        private StackPanel CreateStudentInfoPanel()
        {
            var panel = new StackPanel { Spacing = 10 };

            var header = new TextBlock
            {
                Text = "学生信息",
                FontSize = 16,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.DodgerBlue)
            };
            panel.Children.Add(header);

            var infoGrid = new Grid();
            infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            infoGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            infoGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // 学号
            var studentIdPanel = CreateInfoItem("学号:", StudentIdText);
            Grid.SetRow(studentIdPanel, 0);
            Grid.SetColumn(studentIdPanel, 0);
            infoGrid.Children.Add(studentIdPanel);

            // 姓名
            var namePanel = CreateInfoItem("姓名:", StudentNameText);
            Grid.SetRow(namePanel, 0);
            Grid.SetColumn(namePanel, 1);
            infoGrid.Children.Add(namePanel);

            // 专业
            var majorPanel = CreateInfoItem("专业:", MajorText);
            Grid.SetRow(majorPanel, 1);
            Grid.SetColumn(majorPanel, 0);
            infoGrid.Children.Add(majorPanel);

            // 毕业年份
            var yearPanel = CreateInfoItem("毕业年份:", GraduationYearText);
            Grid.SetRow(yearPanel, 1);
            Grid.SetColumn(yearPanel, 1);
            infoGrid.Children.Add(yearPanel);

            panel.Children.Add(infoGrid);
            return panel;
        }

        private StackPanel CreateRequestInfoPanel()
        {
            var panel = new StackPanel { Spacing = 10 };

            var header = new TextBlock
            {
                Text = "申请信息",
                FontSize = 16,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green)
            };
            panel.Children.Add(header);

            panel.Children.Add(CreateInfoItem("接收单位:", ReceiverNameText));
            panel.Children.Add(CreateInfoItem("接收地址:", ReceiverAddressText));
            panel.Children.Add(CreateInfoItem("申请时间:", RequestDateText));
            panel.Children.Add(CreateInfoItem("申请原因:", ReasonText));

            return panel;
        }

        private StackPanel CreateExpressInfoPanel()
        {
            ExpressInfoPanel.Spacing = 10;

            var header = new TextBlock
            {
                Text = "快递信息",
                FontSize = 16,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.Orange)
            };
            ExpressInfoPanel.Children.Add(header);

            ExpressInfoPanel.Children.Add(CreateInfoItem("快递单号:", TrackingNumberText));
            ExpressInfoPanel.Children.Add(CreateInfoItem("快递公司:", ExpressCompanyText));
            ExpressInfoPanel.Children.Add(CreateInfoItem("寄出时间:", DispatchDateText));
            ExpressInfoPanel.Children.Add(CreateInfoItem("签收时间:", ReceiveDateText));

            TrackExpressButton.Content = "查看快递跟踪";
            TrackExpressButton.Click += TrackExpressButton_Click;
            TrackExpressButton.Margin = new Thickness(0, 10, 0, 0);
            TrackExpressButton.Visibility = Visibility.Collapsed;
            ExpressInfoPanel.Children.Add(TrackExpressButton);

            return ExpressInfoPanel;
        }

        private StackPanel CreateReviewInfoPanel()
        {
            ReviewInfoPanel.Spacing = 10;

            var header = new TextBlock
            {
                Text = "审核信息",
                FontSize = 16,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.Purple)
            };
            ReviewInfoPanel.Children.Add(header);

            ReviewInfoPanel.Children.Add(CreateInfoItem("审核人:", ReviewedByText));
            ReviewInfoPanel.Children.Add(CreateInfoItem("审核时间:", ReviewDateText));
            ReviewInfoPanel.Children.Add(CreateInfoItem("审核意见:", ReviewCommentText));

            return ReviewInfoPanel;
        }

        private StackPanel CreateStatusTimelinePanel()
        {
            var panel = new StackPanel { Spacing = 10 };

            var header = new TextBlock
            {
                Text = "状态时间线",
                FontSize = 16,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.Red)
            };
            panel.Children.Add(header);

            StatusTimeline.Spacing = 5;
            panel.Children.Add(StatusTimeline);

            return panel;
        }

        private StackPanel CreateInfoItem(string label, TextBlock valueText)
        {
            var panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 10,
                Margin = new Thickness(0, 5, 0, 5)
            };

            var labelText = new TextBlock
            {
                Text = label,
                FontWeight = Microsoft.UI.Text.FontWeights.SemiBold,
                MinWidth = 80,
                VerticalAlignment = VerticalAlignment.Center
            };

            valueText.VerticalAlignment = VerticalAlignment.Center;
            valueText.TextWrapping = TextWrapping.Wrap;

            panel.Children.Add(labelText);
            panel.Children.Add(valueText);

            return panel;
        }

        private void LoadRequestInfo()
        {
            // 加载学生基本信息
            if (_request.Student != null)
            {
                StudentIdText.Text = _request.Student.StudentId;
                StudentNameText.Text = _request.Student.Name;
                MajorText.Text = _request.Student.Major ?? "";
                GraduationYearText.Text = _request.Student.GraduationYear?.ToString() ?? "";
            }

            // 加载申请信息
            ReceiverNameText.Text = _request.ReceiverName ?? "";
            ReceiverAddressText.Text = _request.ReceiverAddress ?? "";
            RequestDateText.Text = _request.RequestDate.ToString("yyyy年MM月dd日");
            ReasonText.Text = "档案转递申请"; // 使用默认值，因为模型中没有Reason属性

            // 加载快递信息（如果已寄出）
            if (!string.IsNullOrEmpty(_request.TrackingNumber))
            {
                ExpressInfoPanel.Visibility = Visibility.Visible;
                TrackingNumberText.Text = _request.TrackingNumber;
                ExpressCompanyText.Text = "未指定"; // 使用默认值，因为模型中没有ExpressCompany属性
                DispatchDateText.Text = _request.DispatchDate?.ToString("yyyy年MM月dd日") ?? "";
                ReceiveDateText.Text = _request.ReceiveDate?.ToString("yyyy年MM月dd日") ?? "未签收";

                TrackExpressButton.Visibility = Visibility.Visible;
            }

            // 加载审核信息 - 使用默认值，因为模型中没有这些属性
            if (_request.Status != "待审核")
            {
                ReviewInfoPanel.Visibility = Visibility.Visible;
                ReviewedByText.Text = "系统管理员"; // 默认值
                ReviewDateText.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm"); // 默认值
                ReviewCommentText.Text = _request.Status == "已通过" ? "申请已通过审核" : "申请未通过审核";
            }
        }

        private void BuildStatusTimeline()
        {
            StatusTimeline.Children.Clear();

            // 定义状态步骤，使用现有的属性
            var steps = new (string Status, DateTime? Date, bool IsCompleted, string Description)[]
            {
                ("申请提交", _request.RequestDate, true, "学生提交调档申请"),
                ("申请审核", (DateTime?)DateTime.Now, _request.Status != "待审核", GetReviewDescription()),
                ("档案整理", (DateTime?)null, _request.Status == "已寄出" || _request.Status == "已签收", "档案材料整理和调档函生成"),
                ("档案寄出", _request.DispatchDate, _request.Status == "已寄出" || _request.Status == "已签收", "档案通过快递寄出"),
                ("档案签收", _request.ReceiveDate, _request.Status == "已签收", "接收单位签收档案")
            };

            for (int i = 0; i < steps.Length; i++)
            {
                var step = steps[i];
                var stepPanel = CreateTimelineStep(step.Status, step.Date, step.IsCompleted, step.Description, i == steps.Length - 1);
                StatusTimeline.Children.Add(stepPanel);
            }
        }

        private string GetReviewDescription()
        {
            return _request.Status switch
            {
                "待审核" => "等待管理员审核",
                "已通过" => "申请审核通过",
                "已拒绝" => "申请被拒绝",
                _ => "审核中"
            };
        }

        private UIElement CreateTimelineStep(string title, DateTime? date, bool isCompleted, string description, bool isLast)
        {
            var panel = new StackPanel { Spacing = 5, Margin = new Thickness(0, 5, 0, 5) };

            // 状态行
            var statusPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };

            // 状态图标
            var icon = new FontIcon
            {
                Glyph = isCompleted ? "\uE73E" : "\uE711", // 勾选或等待图标
                Foreground = new SolidColorBrush(isCompleted ? Colors.Green : Colors.Orange),
                FontSize = 16
            };

            // 状态标题和时间
            var titlePanel = new StackPanel { Spacing = 2 };
            var titleText = new TextBlock
            {
                Text = title,
                FontSize = 14,
                FontWeight = Microsoft.UI.Text.FontWeights.SemiBold
            };

            var dateText = new TextBlock
            {
                Text = date?.ToString("yyyy年MM月dd日 HH:mm") ?? "待处理",
                FontSize = 12,
                Foreground = new SolidColorBrush(Colors.Gray)
            };

            titlePanel.Children.Add(titleText);
            titlePanel.Children.Add(dateText);

            statusPanel.Children.Add(icon);
            statusPanel.Children.Add(titlePanel);

            panel.Children.Add(statusPanel);

            // 描述文本
            var descriptionText = new TextBlock
            {
                Text = description,
                FontSize = 12,
                Foreground = new SolidColorBrush(Colors.Gray),
                Margin = new Thickness(26, 0, 0, 0),
                TextWrapping = TextWrapping.Wrap
            };
            panel.Children.Add(descriptionText);

            // 连接线（除了最后一个步骤）
            if (!isLast)
            {
                var line = new Border
                {
                    Width = 2,
                    Height = 20,
                    Background = new SolidColorBrush(isCompleted ? Colors.Green : Colors.LightGray),
                    Margin = new Thickness(8, 5, 0, 5),
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                panel.Children.Add(line);
            }

            return panel;
        }

        private void TrackExpressButton_Click(object sender, RoutedEventArgs e)
        {
            // 这里可以集成实际的快递跟踪API
            var message = $"快递单号：{_request.TrackingNumber}\n" +
                         $"快递公司：未指定\n" +
                         $"当前状态：{_request.Status}\n\n" +
                         "请到快递公司官网或客服电话查询详细物流信息。";

            _ = ShowInfoDialog("快递跟踪", message);
        }

        private void PrintStatusButton_Click(object sender, RoutedEventArgs e)
        {
            _ = ShowInfoDialog("打印状态", "打印功能开发中...");
        }

        private async System.Threading.Tasks.Task ShowInfoDialog(string title, string message)
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
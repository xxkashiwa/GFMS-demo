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

        // ����������Ҫ��UI�ؼ�
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
            // ������ InitializeComponent()���ֶ���������
            _request = request;
            InitializeDialog();
            LoadRequestInfo();
            BuildStatusTimeline();
        }

        private void InitializeDialog()
        {
            this.Title = "����ת��״̬";
            this.CloseButtonText = "�ر�";
            this.DefaultButton = ContentDialogButton.Close;

            // ����������
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

            // ѧ����Ϣ����
            contentPanel.Children.Add(CreateStudentInfoPanel());

            // ������Ϣ����
            contentPanel.Children.Add(CreateRequestInfoPanel());

            // �����Ϣ����
            ExpressInfoPanel.Visibility = Visibility.Collapsed;
            contentPanel.Children.Add(CreateExpressInfoPanel());

            // �����Ϣ����
            ReviewInfoPanel.Visibility = Visibility.Collapsed;
            contentPanel.Children.Add(CreateReviewInfoPanel());

            // ״̬ʱ����
            contentPanel.Children.Add(CreateStatusTimelinePanel());

            mainPanel.Content = contentPanel;
            this.Content = mainPanel;
        }

        private StackPanel CreateStudentInfoPanel()
        {
            var panel = new StackPanel { Spacing = 10 };

            var header = new TextBlock
            {
                Text = "ѧ����Ϣ",
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

            // ѧ��
            var studentIdPanel = CreateInfoItem("ѧ��:", StudentIdText);
            Grid.SetRow(studentIdPanel, 0);
            Grid.SetColumn(studentIdPanel, 0);
            infoGrid.Children.Add(studentIdPanel);

            // ����
            var namePanel = CreateInfoItem("����:", StudentNameText);
            Grid.SetRow(namePanel, 0);
            Grid.SetColumn(namePanel, 1);
            infoGrid.Children.Add(namePanel);

            // רҵ
            var majorPanel = CreateInfoItem("רҵ:", MajorText);
            Grid.SetRow(majorPanel, 1);
            Grid.SetColumn(majorPanel, 0);
            infoGrid.Children.Add(majorPanel);

            // ��ҵ���
            var yearPanel = CreateInfoItem("��ҵ���:", GraduationYearText);
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
                Text = "������Ϣ",
                FontSize = 16,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.Green)
            };
            panel.Children.Add(header);

            panel.Children.Add(CreateInfoItem("���յ�λ:", ReceiverNameText));
            panel.Children.Add(CreateInfoItem("���յ�ַ:", ReceiverAddressText));
            panel.Children.Add(CreateInfoItem("����ʱ��:", RequestDateText));
            panel.Children.Add(CreateInfoItem("����ԭ��:", ReasonText));

            return panel;
        }

        private StackPanel CreateExpressInfoPanel()
        {
            ExpressInfoPanel.Spacing = 10;

            var header = new TextBlock
            {
                Text = "�����Ϣ",
                FontSize = 16,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.Orange)
            };
            ExpressInfoPanel.Children.Add(header);

            ExpressInfoPanel.Children.Add(CreateInfoItem("��ݵ���:", TrackingNumberText));
            ExpressInfoPanel.Children.Add(CreateInfoItem("��ݹ�˾:", ExpressCompanyText));
            ExpressInfoPanel.Children.Add(CreateInfoItem("�ĳ�ʱ��:", DispatchDateText));
            ExpressInfoPanel.Children.Add(CreateInfoItem("ǩ��ʱ��:", ReceiveDateText));

            TrackExpressButton.Content = "�鿴��ݸ���";
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
                Text = "�����Ϣ",
                FontSize = 16,
                FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                Foreground = new SolidColorBrush(Microsoft.UI.Colors.Purple)
            };
            ReviewInfoPanel.Children.Add(header);

            ReviewInfoPanel.Children.Add(CreateInfoItem("�����:", ReviewedByText));
            ReviewInfoPanel.Children.Add(CreateInfoItem("���ʱ��:", ReviewDateText));
            ReviewInfoPanel.Children.Add(CreateInfoItem("������:", ReviewCommentText));

            return ReviewInfoPanel;
        }

        private StackPanel CreateStatusTimelinePanel()
        {
            var panel = new StackPanel { Spacing = 10 };

            var header = new TextBlock
            {
                Text = "״̬ʱ����",
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
            // ����ѧ��������Ϣ
            if (_request.Student != null)
            {
                StudentIdText.Text = _request.Student.StudentId;
                StudentNameText.Text = _request.Student.Name;
                MajorText.Text = _request.Student.Major ?? "";
                GraduationYearText.Text = _request.Student.GraduationYear?.ToString() ?? "";
            }

            // ����������Ϣ
            ReceiverNameText.Text = _request.ReceiverName ?? "";
            ReceiverAddressText.Text = _request.ReceiverAddress ?? "";
            RequestDateText.Text = _request.RequestDate.ToString("yyyy��MM��dd��");
            ReasonText.Text = "����ת������"; // ʹ��Ĭ��ֵ����Ϊģ����û��Reason����

            // ���ؿ����Ϣ������Ѽĳ���
            if (!string.IsNullOrEmpty(_request.TrackingNumber))
            {
                ExpressInfoPanel.Visibility = Visibility.Visible;
                TrackingNumberText.Text = _request.TrackingNumber;
                ExpressCompanyText.Text = "δָ��"; // ʹ��Ĭ��ֵ����Ϊģ����û��ExpressCompany����
                DispatchDateText.Text = _request.DispatchDate?.ToString("yyyy��MM��dd��") ?? "";
                ReceiveDateText.Text = _request.ReceiveDate?.ToString("yyyy��MM��dd��") ?? "δǩ��";

                TrackExpressButton.Visibility = Visibility.Visible;
            }

            // ���������Ϣ - ʹ��Ĭ��ֵ����Ϊģ����û����Щ����
            if (_request.Status != "�����")
            {
                ReviewInfoPanel.Visibility = Visibility.Visible;
                ReviewedByText.Text = "ϵͳ����Ա"; // Ĭ��ֵ
                ReviewDateText.Text = DateTime.Now.ToString("yyyy��MM��dd�� HH:mm"); // Ĭ��ֵ
                ReviewCommentText.Text = _request.Status == "��ͨ��" ? "������ͨ�����" : "����δͨ�����";
            }
        }

        private void BuildStatusTimeline()
        {
            StatusTimeline.Children.Clear();

            // ����״̬���裬ʹ�����е�����
            var steps = new (string Status, DateTime? Date, bool IsCompleted, string Description)[]
            {
                ("�����ύ", _request.RequestDate, true, "ѧ���ύ��������"),
                ("�������", (DateTime?)DateTime.Now, _request.Status != "�����", GetReviewDescription()),
                ("��������", (DateTime?)null, _request.Status == "�Ѽĳ�" || _request.Status == "��ǩ��", "������������͵���������"),
                ("�����ĳ�", _request.DispatchDate, _request.Status == "�Ѽĳ�" || _request.Status == "��ǩ��", "����ͨ����ݼĳ�"),
                ("����ǩ��", _request.ReceiveDate, _request.Status == "��ǩ��", "���յ�λǩ�յ���")
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
                "�����" => "�ȴ�����Ա���",
                "��ͨ��" => "�������ͨ��",
                "�Ѿܾ�" => "���뱻�ܾ�",
                _ => "�����"
            };
        }

        private UIElement CreateTimelineStep(string title, DateTime? date, bool isCompleted, string description, bool isLast)
        {
            var panel = new StackPanel { Spacing = 5, Margin = new Thickness(0, 5, 0, 5) };

            // ״̬��
            var statusPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };

            // ״̬ͼ��
            var icon = new FontIcon
            {
                Glyph = isCompleted ? "\uE73E" : "\uE711", // ��ѡ��ȴ�ͼ��
                Foreground = new SolidColorBrush(isCompleted ? Colors.Green : Colors.Orange),
                FontSize = 16
            };

            // ״̬�����ʱ��
            var titlePanel = new StackPanel { Spacing = 2 };
            var titleText = new TextBlock
            {
                Text = title,
                FontSize = 14,
                FontWeight = Microsoft.UI.Text.FontWeights.SemiBold
            };

            var dateText = new TextBlock
            {
                Text = date?.ToString("yyyy��MM��dd�� HH:mm") ?? "������",
                FontSize = 12,
                Foreground = new SolidColorBrush(Colors.Gray)
            };

            titlePanel.Children.Add(titleText);
            titlePanel.Children.Add(dateText);

            statusPanel.Children.Add(icon);
            statusPanel.Children.Add(titlePanel);

            panel.Children.Add(statusPanel);

            // �����ı�
            var descriptionText = new TextBlock
            {
                Text = description,
                FontSize = 12,
                Foreground = new SolidColorBrush(Colors.Gray),
                Margin = new Thickness(26, 0, 0, 0),
                TextWrapping = TextWrapping.Wrap
            };
            panel.Children.Add(descriptionText);

            // �����ߣ��������һ�����裩
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
            // ������Լ���ʵ�ʵĿ�ݸ���API
            var message = $"��ݵ��ţ�{_request.TrackingNumber}\n" +
                         $"��ݹ�˾��δָ��\n" +
                         $"��ǰ״̬��{_request.Status}\n\n" +
                         "�뵽��ݹ�˾������ͷ��绰��ѯ��ϸ������Ϣ��";

            _ = ShowInfoDialog("��ݸ���", message);
        }

        private void PrintStatusButton_Click(object sender, RoutedEventArgs e)
        {
            _ = ShowInfoDialog("��ӡ״̬", "��ӡ���ܿ�����...");
        }

        private async System.Threading.Tasks.Task ShowInfoDialog(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "ȷ��",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}
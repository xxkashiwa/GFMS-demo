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

        // ������ʱ�ؼ��Ա���������
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
            // ע�⣺���û�ж�Ӧ��XAML�ļ������ܵ���InitializeComponent()
            // this.InitializeComponent();

            _request = request;
            InitializeDialog();
            LoadRequestInfo();
            GenerateLetterNumber();
            UpdatePreview();

            // ���ð�ť����¼�
            this.PrimaryButtonClick += TransferLetterDialog_PrimaryButtonClick;
        }

        private void InitializeDialog()
        {
            // �ֶ���ʼ���Ի���
            this.Title = "���ɵ�����";
            this.PrimaryButtonText = "����";
            this.CloseButtonText = "ȡ��";

            // ������������
            var stackPanel = new StackPanel();

            // ��ӻ�����Ϣ����
            var infoGrid = CreateInfoGrid();
            stackPanel.Children.Add(infoGrid);

            // ��Ӳ����嵥����
            var materialsPanel = CreateMaterialsPanel();
            stackPanel.Children.Add(materialsPanel);

            // ���Ԥ������
            var previewPanel = CreatePreviewPanel();
            stackPanel.Children.Add(previewPanel);

            this.Content = stackPanel;
        }

        private Grid CreateInfoGrid()
        {
            var grid = new Grid();
            grid.Margin = new Thickness(10);

            // �����к���
            for (int i = 0; i < 6; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // ���������
            var letterNumberLabel = new TextBlock { Text = "���������:", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(letterNumberLabel, 0);
            Grid.SetColumn(letterNumberLabel, 0);
            grid.Children.Add(letterNumberLabel);

            Grid.SetRow(LetterNumberTextBox, 0);
            Grid.SetColumn(LetterNumberTextBox, 1);
            grid.Children.Add(LetterNumberTextBox);

            // ѧ����Ϣ
            var studentIdLabel = new TextBlock { Text = "ѧ��:", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(studentIdLabel, 1);
            Grid.SetColumn(studentIdLabel, 0);
            grid.Children.Add(studentIdLabel);

            Grid.SetRow(StudentIdTextBox, 1);
            Grid.SetColumn(StudentIdTextBox, 1);
            StudentIdTextBox.IsReadOnly = true;
            grid.Children.Add(StudentIdTextBox);

            var studentNameLabel = new TextBlock { Text = "����:", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(studentNameLabel, 2);
            Grid.SetColumn(studentNameLabel, 0);
            grid.Children.Add(studentNameLabel);

            Grid.SetRow(StudentNameTextBox, 2);
            Grid.SetColumn(StudentNameTextBox, 1);
            StudentNameTextBox.IsReadOnly = true;
            grid.Children.Add(StudentNameTextBox);

            // ���յ�λ��Ϣ
            var receiverLabel = new TextBlock { Text = "���յ�λ:", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(receiverLabel, 3);
            Grid.SetColumn(receiverLabel, 0);
            grid.Children.Add(receiverLabel);

            Grid.SetRow(ReceiverNameTextBox, 3);
            Grid.SetColumn(ReceiverNameTextBox, 1);
            ReceiverNameTextBox.IsReadOnly = true;
            grid.Children.Add(ReceiverNameTextBox);

            // ����ʱ��
            var transferDateLabel = new TextBlock { Text = "����ʱ��:", VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(transferDateLabel, 4);
            Grid.SetColumn(transferDateLabel, 0);
            grid.Children.Add(transferDateLabel);

            Grid.SetRow(TransferDatePicker, 4);
            Grid.SetColumn(TransferDatePicker, 1);
            grid.Children.Add(TransferDatePicker);

            // ��ע
            var remarksLabel = new TextBlock { Text = "��ע:", VerticalAlignment = VerticalAlignment.Center };
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
                Text = "�����嵥:",
                FontWeight = Microsoft.UI.Text.FontWeights.Bold,
                Margin = new Thickness(0, 10, 0, 5)
            };
            panel.Children.Add(header);

            panel.Children.Add(GraduateFormCheckBox);
            GraduateFormCheckBox.Content = "��ҵ���ǼǱ�";

            panel.Children.Add(TranscriptCheckBox);
            TranscriptCheckBox.Content = "ѧϰ�ɼ���";

            panel.Children.Add(MedicalCheckBox);
            MedicalCheckBox.Content = "����";

            panel.Children.Add(RewardPunishmentCheckBox);
            RewardPunishmentCheckBox.Content = "���Ͳ���";

            panel.Children.Add(ThesisDefenseCheckBox);
            ThesisDefenseCheckBox.Content = "���Ĵ���¼";

            panel.Children.Add(InternshipReportCheckBox);
            InternshipReportCheckBox.Content = "ʵϰ����";

            var otherLabel = new TextBlock { Text = "��������:", Margin = new Thickness(0, 10, 0, 5) };
            panel.Children.Add(otherLabel);

            panel.Children.Add(OtherMaterialsTextBox);
            OtherMaterialsTextBox.PlaceholderText = "������������������";

            return panel;
        }

        private StackPanel CreatePreviewPanel()
        {
            var panel = new StackPanel();
            panel.Margin = new Thickness(10);

            var header = new TextBlock
            {
                Text = "������Ԥ��:",
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
            // ��֤��������
            if (TransferDatePicker.Date == null)
            {
                args.Cancel = true;
                await ShowErrorDialog("��ѡ�����ʱ��");
                return;
            }

            var materials = GetSelectedMaterials();
            if (materials.Count == 0)
            {
                args.Cancel = true;
                await ShowErrorDialog("��ѡ������һ�ֲ���");
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

            // �Ƴ�����ϵ�˺͵绰�ֶΣ���Ϊ�±�ṹ��û����Щ�ֶ�
            ReceiverContactTextBox.Text = "";
            ReceiverPhoneTextBox.Text = "";

            // ����Ĭ�ϵļ���ʱ��Ϊ����
            TransferDatePicker.Date = DateTimeOffset.Now.AddDays(1);

            // ���¼�ʵʱ����Ԥ��
            TransferDatePicker.DateChanged += (s, e) => UpdatePreview();
            RemarksTextBox.TextChanged += (s, e) => UpdatePreview();
            OtherMaterialsTextBox.TextChanged += (s, e) => UpdatePreview();

            // Ϊ���и�ѡ����¼�
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
            var sequence = new Random().Next(1, 9999); // �򵥵����к�����
            LetterNumberTextBox.Text = $"DF{year}{sequence:D4}";
        }

        private void UpdatePreview()
        {
            var materials = GetSelectedMaterials();
            var materialsText = string.Join("��", materials);

            var transferDate = TransferDatePicker.Date.ToString("yyyy��MM��dd��") ?? "____��__��__��";
            var remarks = string.IsNullOrWhiteSpace(RemarksTextBox.Text) ? "" : $"\n\n��ע��{RemarksTextBox.Text}";

            // �����±�ṹ��û����ϵ�˺͵绰�ֶΣ����Ǵӽ����ȡ��Щ��Ϣ
            var contactInfo = "";
            if (!string.IsNullOrWhiteSpace(ReceiverContactTextBox.Text) || !string.IsNullOrWhiteSpace(ReceiverPhoneTextBox.Text))
            {
                contactInfo = $"\n��ϵ�ˣ�{ReceiverContactTextBox.Text}\n��ϵ�绰��{ReceiverPhoneTextBox.Text}";
            }

            var content = $@"{ReceiverNameTextBox.Text}��

����{StudentNameTextBox.Text}��ѧ�ţ�{StudentIdTextBox.Text}���ĵ������룬�ֽ���ѧ������ת�ݹ�λ��

ѧ����Ϣ��
������{StudentNameTextBox.Text}
ѧ�ţ�{StudentIdTextBox.Text}
רҵ��{MajorTextBox.Text}
��ҵ��ݣ�{GraduationYearTextBox.Text}

���յ�λ��{ReceiverNameTextBox.Text}
���յ�ַ��{ReceiverAddressTextBox.Text}{contactInfo}

�溯ת�ݲ����嵥��
{materialsText}

����ʱ�䣺{transferDate}

���λ���ո�ѧ��ѧ��������{remarks}

����
����

������������
{DateTime.Now:yyyy��MM��dd��}";

            PreviewContent.Text = content;
        }

        private List<string> GetSelectedMaterials()
        {
            var materials = new List<string>();

            if (GraduateFormCheckBox.IsChecked == true) materials.Add("��ҵ���ǼǱ�");
            if (TranscriptCheckBox.IsChecked == true) materials.Add("ѧϰ�ɼ���");
            if (MedicalCheckBox.IsChecked == true) materials.Add("����");
            if (RewardPunishmentCheckBox.IsChecked == true) materials.Add("���Ͳ���");
            if (ThesisDefenseCheckBox.IsChecked == true) materials.Add("���Ĵ���¼");
            if (InternshipReportCheckBox.IsChecked == true) materials.Add("ʵϰ����");

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
                Title = "�������",
                Content = message,
                CloseButtonText = "ȷ��",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}
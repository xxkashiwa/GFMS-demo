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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage;
using Microsoft.UI;
using GFMS.Models;
using GFMS.Services;

namespace GFMS.Pages
{
    // ѧ����Ϣ����ģ��
    public class StudentInfo
    {
        public string StudentId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTimeOffset? BirthDate { get; set; }
        public string IdCard { get; set; } = string.Empty;
        public string Major { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ParentName { get; set; } = string.Empty;
        public string ParentPhone { get; set; } = string.Empty;
        public string Status { get; set; } = "δ���";
    }

    // �ɼ���Ϣ����ģ��
    public class GradeInfo
    {
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public double Credits { get; set; }
        public double Score { get; set; }
        public double GPA { get; set; }
        public string GradeType { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public string Semester { get; set; } = string.Empty;
    }

    // ���ͼ�¼����ģ��
    public class RewardPunishmentRecord
    {
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "����" �� "����"
        public string Name { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public DateTimeOffset? Date { get; set; }
        public string Organization { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CertificateFile { get; set; } = string.Empty;
    }

    public sealed partial class DataCollectionPage : Page
    {
        // ���ݼ���
        public ObservableCollection<StudentInfo> Students { get; set; }
        public ObservableCollection<GradeInfo> Grades { get; set; }
        public ObservableCollection<RewardPunishmentRecord> Records { get; set; }

        // ��ǰѡ�е�ѧ��
        private StudentInfo? _selectedStudent;
        private RewardPunishmentRecord? _selectedRecord;

        public DataCollectionPage()
        {
            InitializeComponent();

            // ��ʼ�����ݼ���
            Students = new ObservableCollection<StudentInfo>();
            Grades = new ObservableCollection<GradeInfo>();
            Records = new ObservableCollection<RewardPunishmentRecord>();

            // ������Դ
            StudentListView.ItemsSource = Students;
            GradeListView.ItemsSource = Grades;
            RewardPunishmentListView.ItemsSource = Records;

            // ע���¼�������
            RegisterEventHandlers();

            // ���س�ʼ����
            LoadInitialData();
        }

        private void RegisterEventHandlers()
        {
            // ������Ϣ��ǩҳ�¼�
            StudentListView.SelectionChanged += StudentListView_SelectionChanged;

            // ���ͼ�¼�б�ѡ���¼�
            RewardPunishmentListView.SelectionChanged += RewardPunishmentListView_SelectionChanged;

            // ���Ҳ�ע�ᰴť�¼�
            var importButton = FindName("ImportStudentInfoButton") as Button;
            if (importButton?.Flyout is MenuFlyout flyout)
            {
                foreach (var item in flyout.Items)
                {
                    if (item is MenuFlyoutItem menuItem)
                    {
                        menuItem.Click += ImportMenuItem_Click;
                    }
                }
            }

            // ע��������ť�¼�
            RegisterButtonEvents();

            // ע��ɸѡ�ؼ��¼�
            RegisterFilterEvents();

            // ע���������¼�
            RegisterSearchEvents();
        }

        private void RegisterButtonEvents()
        {
            // ������Ϣҳ�水ť
            var buttons = GetButtonsFromGrid();
            foreach (var button in buttons)
            {
                var content = GetButtonContent(button);
                switch (content)
                {
                    case "����ѧ��":
                        button.Click += AddStudentButton_Click;
                        break;
                    case "��������":
                        button.Click += ExportDataButton_Click;
                        break;
                    case "������Ϣ":
                        button.Click += SaveStudentButton_Click;
                        break;
                    case "ɾ��ѧ��":
                        button.Click += DeleteStudentButton_Click;
                        break;
                    case "����":
                        button.Click += ResetStudentButton_Click;
                        break;
                    case "��������ɼ�":
                        button.Click += ImportGradesButton_Click;
                        break;
                    case "��Ӽ�¼":
                        button.Click += AddRecordButton_Click;
                        break;
                    case "��������":
                        button.Click += ImportRecordsButton_Click;
                        break;
                    case "�����¼":
                        button.Click += SaveRecordButton_Click;
                        break;
                    case "ɾ��":
                        button.Click += DeleteRecordButton_Click;
                        break;
                    case "���":
                        button.Click += ClearRecordButton_Click;
                        break;
                    case "ѡ���ļ�":
                        button.Click += SelectFileButton_Click;
                        break;
                }
            }
        }

        private List<Button> GetButtonsFromGrid()
        {
            var buttons = new List<Button>();
            // �ݹ��������Button�ؼ�
            FindButtons(this, buttons);
            return buttons;
        }

        private void FindButtons(DependencyObject parent, List<Button> buttons)
        {
            var childCount = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is Button button)
                {
                    buttons.Add(button);
                }
                FindButtons(child, buttons);
            }
        }

        private string GetButtonContent(Button button)
        {
            if (button.Content is string text)
                return text;
            if (button.Content is StackPanel panel)
            {
                foreach (var child in panel.Children)
                {
                    if (child is TextBlock textBlock)
                        return textBlock.Text;
                }
            }
            return string.Empty;
        }

        private void RegisterFilterEvents()
        {
            // ���Ҳ�ע��ɸѡ�ؼ��¼�
            var comboBoxes = GetComboBoxesFromGrid();
            foreach (var comboBox in comboBoxes)
            {
                var placeholder = comboBox.PlaceholderText;
                switch (placeholder)
                {
                    case "ѡ��ѧ��":
                        comboBox.SelectionChanged += (s, e) => FilterGradesBySemester((comboBox.SelectedItem as ComboBoxItem)?.Content?.ToString());
                        break;
                    case "ѡ��רҵ":
                        comboBox.SelectionChanged += (s, e) => FilterGradesByMajor((comboBox.SelectedItem as ComboBoxItem)?.Content?.ToString());
                        break;
                    case "��¼����":
                        comboBox.SelectionChanged += (s, e) => FilterRecordsByType((comboBox.SelectedItem as ComboBoxItem)?.Content?.ToString());
                        break;
                    case "��¼����":
                        comboBox.SelectionChanged += (s, e) => FilterRecordsByLevel((comboBox.SelectedItem as ComboBoxItem)?.Content?.ToString());
                        break;
                }
            }
        }

        private List<ComboBox> GetComboBoxesFromGrid()
        {
            var comboBoxes = new List<ComboBox>();
            FindComboBoxes(this, comboBoxes);
            return comboBoxes;
        }

        private void FindComboBoxes(DependencyObject parent, List<ComboBox> comboBoxes)
        {
            var childCount = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is ComboBox comboBox)
                {
                    comboBoxes.Add(comboBox);
                }
                FindComboBoxes(child, comboBoxes);
            }
        }

        private void RegisterSearchEvents()
        {
            // ����������ע���¼�
            var textBoxes = GetTextBoxesFromGrid();
            foreach (var textBox in textBoxes)
            {
                if (textBox.PlaceholderText?.Contains("����") == true)
                {
                    textBox.TextChanged += SearchTextBox_TextChanged;
                }
            }
        }

        private List<TextBox> GetTextBoxesFromGrid()
        {
            var textBoxes = new List<TextBox>();
            FindTextBoxes(this, textBoxes);
            return textBoxes;
        }

        private void FindTextBoxes(DependencyObject parent, List<TextBox> textBoxes)
        {
            var childCount = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is TextBox textBox)
                {
                    textBoxes.Add(textBox);
                }
                FindTextBoxes(child, textBoxes);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox searchBox)
            {
                SearchStudents(searchBox.Text);
            }
        }

        private void LoadInitialData()
        {
            // TODO: �����ݿ����ѧ����Ϣ
            // var students = await StudentService.GetAllStudentsAsync();
            // foreach (var student in students)
            // {
            //     Students.Add(student);
            // }

            // ��ʱ���ʾ������
            Students.Add(new StudentInfo
            {
                StudentId = "2021001",
                Name = "����",
                Gender = "��",
                Major = "�������ѧ�뼼��",
                Class = "�ƿ�2101",
                Status = "�����"
            });

            Students.Add(new StudentInfo
            {
                StudentId = "2021002",
                Name = "����",
                Gender = "Ů",
                Major = "�������",
                Class = "��2101",
                Status = "δ���"
            });

            // TODO: ���سɼ�����
            // var grades = await GradeService.GetAllGradesAsync();
            // foreach (var grade in grades)
            // {
            //     Grades.Add(grade);
            // }

            // ��ʱ���ʾ���ɼ�����
            Grades.Add(new GradeInfo
            {
                StudentId = "2021001",
                StudentName = "����",
                CourseName = "�ߵ���ѧ",
                Credits = 4,
                Score = 85,
                GPA = 3.2,
                GradeType = "��ĩ�ɼ�",
                Semester = "2023-2024����"
            });

            // TODO: ���ؽ��ͼ�¼
            // var records = await RecordService.GetAllRecordsAsync();
            // foreach (var record in records)
            // {
            //     Records.Add(record);
            // }

            // ��ʱ���ʾ�����ͼ�¼
            Records.Add(new RewardPunishmentRecord
            {
                StudentId = "2021001",
                StudentName = "����",
                Type = "����",
                Name = "����ѧ��",
                Level = "У��",
                Date = DateTimeOffset.Parse("2023-12-01")
            });
        }

        #region ������Ϣ��ǩҳ�¼�����

        private void StudentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StudentListView.SelectedItem is StudentInfo selectedStudent)
            {
                _selectedStudent = selectedStudent;
                LoadStudentDetails(selectedStudent);
            }
        }

        private void LoadStudentDetails(StudentInfo student)
        {
            // ������������еĿؼ����������
            var detailsPanel = FindDetailsPanel();
            if (detailsPanel != null)
            {
                FillStudentDetailsForm(detailsPanel, student);
            }
        }

        private StackPanel? FindDetailsPanel()
        {
            // �ݹ�����������
            return FindChildOfType<StackPanel>(this, "ѧ����ϸ��Ϣ");
        }

        private T? FindChildOfType<T>(DependencyObject parent, string identifier = "") where T : DependencyObject
        {
            var childCount = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is T target)
                {
                    if (string.IsNullOrEmpty(identifier) ||
                        (target is TextBlock tb && tb.Text == identifier))
                        return target;
                }
                var result = FindChildOfType<T>(child, identifier);
                if (result != null) return result;
            }
            return null;
        }

        private void FillStudentDetailsForm(StackPanel panel, StudentInfo student)
        {
            // ���Ҳ������ؼ�
            var textBoxes = new List<TextBox>();
            var comboBoxes = new List<ComboBox>();
            var datePickers = new List<DatePicker>();

            FindFormControls(panel, textBoxes, comboBoxes, datePickers);

            // ����Header�����������
            foreach (var textBox in textBoxes)
            {
                switch (textBox.Header?.ToString())
                {
                    case "ѧ��":
                        textBox.Text = student.StudentId;
                        break;
                    case "����":
                        textBox.Text = student.Name;
                        break;
                    case "���֤��":
                        textBox.Text = student.IdCard;
                        break;
                    case "רҵ":
                        textBox.Text = student.Major;
                        break;
                    case "�༶":
                        textBox.Text = student.Class;
                        break;
                    case "��ϵ�绰":
                        textBox.Text = student.Phone;
                        break;
                    case "�����ַ":
                        textBox.Text = student.Email;
                        break;
                    case "��ͥסַ":
                        textBox.Text = student.Address;
                        break;
                    case "�ҳ�����":
                        textBox.Text = student.ParentName;
                        break;
                    case "�ҳ���ϵ��ʽ":
                        textBox.Text = student.ParentPhone;
                        break;
                }
            }

            // ����Ա�������
            foreach (var comboBox in comboBoxes)
            {
                if (comboBox.Header?.ToString() == "�Ա�")
                {
                    comboBox.SelectedIndex = student.Gender == "��" ? 0 : 1;
                }
            }

            // ����������
            foreach (var datePicker in datePickers)
            {
                if (datePicker.Header?.ToString() == "��������")
                {
                    datePicker.Date = student.BirthDate ?? DateTimeOffset.Now;
                }
            }
        }

        private void FindFormControls(DependencyObject parent, List<TextBox> textBoxes, List<ComboBox> comboBoxes, List<DatePicker> datePickers)
        {
            var childCount = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = Microsoft.UI.Xaml.Media.VisualTreeHelper.GetChild(parent, i);

                if (child is TextBox textBox)
                    textBoxes.Add(textBox);
                else if (child is ComboBox comboBox)
                    comboBoxes.Add(comboBox);
                else if (child is DatePicker datePicker)
                    datePickers.Add(datePicker);

                FindFormControls(child, textBoxes, comboBoxes, datePickers);
            }
        }

        private async void ImportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem menuItem)
            {
                switch (menuItem.Text)
                {
                    case "��Excel����":
                        await ImportFromExcel();
                        break;
                    case "�ӽ���ϵͳ����":
                        await ImportFromEducationSystem();
                        break;
                    case "����ģ��":
                        await DownloadTemplate();
                        break;
                }
            }
        }

        private async Task ImportFromExcel()
        {
            try
            {
                var picker = new FileOpenPicker();
                picker.FileTypeFilter.Add(".xlsx");
                picker.FileTypeFilter.Add(".xls");

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(((App)Application.Current).MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                var file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    // ʹ�� ExcelImportService ����ѧ����Ϣ
                    var students = await ExcelImportService.ImportStudentsFromExcelAsync(file);
                    foreach (var student in students)
                    {
                        var studentInfo = new StudentInfo
                        {
                            StudentId = student.StudentId,
                            Name = student.Name,
                            Gender = student.Gender == "M" ? "��" : "Ů",
                            Major = student.Major ?? "",
                            Status = "δ���"
                        };
                        Students.Add(studentInfo);
                    }

                    ShowInfoDialog("����ɹ�", $"Excel�ļ�������ɣ������� {students.Count} ��ѧ����¼��");
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog("����ʧ��", $"Excel����ʱ��������{ex.Message}");
            }
        }

        private async Task ImportFromEducationSystem()
        {
            try
            {
                // TODO: ʵ�ִӽ���ϵͳ������߼�
                // var students = await EducationSystemService.ImportStudentsAsync();
                // foreach (var student in students)
                // {
                //     Students.Add(student);
                // }

                ShowInfoDialog("����ɹ�", "�ӽ���ϵͳ������ɣ�");
            }
            catch (Exception ex)
            {
                ShowErrorDialog("����ʧ��", $"�ӽ���ϵͳ����ʱ��������{ex.Message}");
            }
        }

        private async Task DownloadTemplate()
        {
            try
            {
                var picker = new FileSavePicker();
                picker.FileTypeChoices.Add("Excel�ļ�", new List<string> { ".xlsx" });
                picker.SuggestedFileName = "ѧ����Ϣ����ģ��";

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(((App)Application.Current).MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                var file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    // ʹ�� ExcelTemplateService ����ѧ����Ϣģ��
                    await ExcelTemplateService.GenerateStudentTemplate(file);

                    ShowInfoDialog("���سɹ�", "ģ���ļ��ѱ��棡");
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog("����ʧ��", $"ģ������ʱ��������{ex.Message}");
            }
        }

        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            // ��ձ���׼�������ѧ��
            _selectedStudent = null;
            ClearStudentForm();

            // �����µ�ѧ��
            var newStudentId = GenerateNewStudentId();
            var textBoxes = new List<TextBox>();
            FindFormControls(this, textBoxes, new List<ComboBox>(), new List<DatePicker>());

            foreach (var textBox in textBoxes)
            {
                if (textBox.Header?.ToString() == "ѧ��")
                {
                    textBox.Text = newStudentId;
                    break;
                }
            }
        }

        private string GenerateNewStudentId()
        {
            // TODO: ʵ��ѧ�������߼�
            // return StudentService.GenerateNewStudentId();

            // ��ʱʵ�֣����ڵ�ǰ��ݺ����
            var year = DateTime.Now.Year;
            var maxId = Students.Where(s => s.StudentId.StartsWith(year.ToString()))
                              .Select(s => s.StudentId)
                              .DefaultIfEmpty($"{year}000")
                              .Max();

            if (int.TryParse(maxId.Substring(4), out int lastNumber))
            {
                return $"{year}{(lastNumber + 1):D3}";
            }
            return $"{year}001";
        }

        private void ClearStudentForm()
        {
            var textBoxes = new List<TextBox>();
            var comboBoxes = new List<ComboBox>();
            var datePickers = new List<DatePicker>();

            FindFormControls(this, textBoxes, comboBoxes, datePickers);

            foreach (var textBox in textBoxes)
            {
                textBox.Text = string.Empty;
            }

            foreach (var comboBox in comboBoxes)
            {
                comboBox.SelectedIndex = -1;
            }

            foreach (var datePicker in datePickers)
            {
                datePicker.Date = DateTimeOffset.Now;
            }
        }

        private async void ExportDataButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var picker = new FileSavePicker();
                picker.FileTypeChoices.Add("Excel�ļ�", new List<string> { ".xlsx" });
                picker.SuggestedFileName = $"ѧ����Ϣ_{DateTime.Now:yyyyMMdd}";

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(((App)Application.Current).MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                var file = await picker.PickSaveFileAsync();
                if (file != null)
                {
                    // ת�� StudentInfo �� Student ģ��
                    var studentsToExport = Students.Select(s => new Student
                    {
                        StudentId = s.StudentId,
                        Name = s.Name,
                        Gender = s.Gender == "��" ? "M" : "F",
                        Major = s.Major,
                        GraduationYear = DateTime.Now.Year // ���Ը�����Ҫ����
                    }).ToList();

                    // ʹ�� ExcelExportService ����ѧ������
                    await ExcelExportService.ExportStudentsToExcelAsync(studentsToExport, file);

                    ShowInfoDialog("�����ɹ�", $"ѧ�������ѵ�����Excel�ļ��������� {studentsToExport.Count} ����¼��");
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog("����ʧ��", $"���ݵ���ʱ��������{ex.Message}");
            }
        }

        private async void SaveStudentButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var student = GetStudentFromForm();
                if (student == null) return;

                if (_selectedStudent == null)
                {
                    // ����ѧ��
                    // TODO: ���浽���ݿ�
                    // await StudentService.AddStudentAsync(student);

                    Students.Add(student);
                    ShowInfoDialog("����ɹ�", "ѧ����Ϣ����ӣ�");
                }
                else
                {
                    // ����ѧ����Ϣ
                    // TODO: �������ݿ�
                    // await StudentService.UpdateStudentAsync(student);

                    var index = Students.IndexOf(_selectedStudent);
                    if (index >= 0)
                    {
                        Students[index] = student;
                    }
                    ShowInfoDialog("����ɹ�", "ѧ����Ϣ�Ѹ��£�");
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog("����ʧ��", $"����ѧ����Ϣʱ��������{ex.Message}");
            }
        }

        private StudentInfo? GetStudentFromForm()
        {
            var textBoxes = new List<TextBox>();
            var comboBoxes = new List<ComboBox>();
            var datePickers = new List<DatePicker>();

            FindFormControls(this, textBoxes, comboBoxes, datePickers);

            var student = new StudentInfo();

            foreach (var textBox in textBoxes)
            {
                switch (textBox.Header?.ToString())
                {
                    case "ѧ��":
                        if (string.IsNullOrWhiteSpace(textBox.Text))
                        {
                            ShowErrorDialog("��֤ʧ��", "ѧ�Ų���Ϊ�գ�");
                            return null;
                        }
                        student.StudentId = textBox.Text.Trim();
                        break;
                    case "����":
                        if (string.IsNullOrWhiteSpace(textBox.Text))
                        {
                            ShowErrorDialog("��֤ʧ��", "��������Ϊ�գ�");
                            return null;
                        }
                        student.Name = textBox.Text.Trim();
                        break;
                    case "���֤��":
                        student.IdCard = textBox.Text.Trim();
                        break;
                    case "רҵ":
                        student.Major = textBox.Text.Trim();
                        break;
                    case "�༶":
                        student.Class = textBox.Text.Trim();
                        break;
                    case "��ϵ�绰":
                        student.Phone = textBox.Text.Trim();
                        break;
                    case "�����ַ":
                        student.Email = textBox.Text.Trim();
                        break;
                    case "��ͥסַ":
                        student.Address = textBox.Text.Trim();
                        break;
                    case "�ҳ�����":
                        student.ParentName = textBox.Text.Trim();
                        break;
                    case "�ҳ���ϵ��ʽ":
                        student.ParentPhone = textBox.Text.Trim();
                        break;
                }
            }

            foreach (var comboBox in comboBoxes)
            {
                if (comboBox.Header?.ToString() == "�Ա�" && comboBox.SelectedItem is ComboBoxItem item)
                {
                    student.Gender = item.Content?.ToString() ?? "";
                }
            }

            foreach (var datePicker in datePickers)
            {
                if (datePicker.Header?.ToString() == "��������")
                {
                    student.BirthDate = datePicker.Date;
                }
            }

            return student;
        }

        private async void DeleteStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedStudent == null)
            {
                ShowErrorDialog("����ʧ��", "����ѡ��Ҫɾ����ѧ����");
                return;
            }

            var result = await ShowConfirmDialog("ȷ��ɾ��", $"ȷ��Ҫɾ��ѧ�� {_selectedStudent.Name} ����Ϣ�𣿴˲������ɳ�����");
            if (result)
            {
                try
                {
                    // TODO: �����ݿ�ɾ��
                    // await StudentService.DeleteStudentAsync(_selectedStudent.StudentId);

                    Students.Remove(_selectedStudent);
                    _selectedStudent = null;
                    ClearStudentForm();

                    ShowInfoDialog("ɾ���ɹ�", "ѧ����Ϣ��ɾ����");
                }
                catch (Exception ex)
                {
                    ShowErrorDialog("ɾ��ʧ��", $"ɾ��ѧ����Ϣʱ��������{ex.Message}");
                }
            }
        }

        private void ResetStudentButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedStudent != null)
            {
                LoadStudentDetails(_selectedStudent);
            }
            else
            {
                ClearStudentForm();
            }
        }

        private void RewardPunishmentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RewardPunishmentListView.SelectedItem is RewardPunishmentRecord selectedRecord)
            {
                _selectedRecord = selectedRecord;
                LoadRecordDetails(selectedRecord);
            }
        }

        private void LoadRecordDetails(RewardPunishmentRecord record)
        {
            // ������������еĿؼ����������
            var detailsPanel = FindRecordDetailsPanel();
            if (detailsPanel != null)
            {
                FillRecordDetailsForm(detailsPanel, record);
            }
        }

        private void FillRecordDetailsForm(StackPanel panel, RewardPunishmentRecord record)
        {
            // ���Ҳ������ؼ�
            var textBoxes = new List<TextBox>();
            var comboBoxes = new List<ComboBox>();
            var datePickers = new List<DatePicker>();

            FindFormControls(panel, textBoxes, comboBoxes, datePickers);

            // �����������ݹ���ұ��ؼ�
            void FindFormControls(DependencyObject parent, List<TextBox> textBoxList, List<ComboBox> comboBoxList, List<DatePicker> datePickerList)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);

                    if (child is TextBox textBox)
                        textBoxList.Add(textBox);
                    else if (child is ComboBox comboBox)
                        comboBoxList.Add(comboBox);
                    else if (child is DatePicker datePicker)
                        datePickerList.Add(datePicker);

                    FindFormControls(child, textBoxList, comboBoxList, datePickerList);
                }
            }

            // ����Header�����������
            foreach (var textBox in textBoxes)
            {
                switch (textBox.Header?.ToString())
                {
                    case "ѧ��":
                        textBox.Text = record.StudentId;
                        break;
                    case "ѧ������":
                        textBox.Text = record.StudentName;
                        break;
                    case "��������":
                        textBox.Text = record.Name;
                        break;
                    case "�䷢����":
                        textBox.Text = record.Organization;
                        break;
                    case "��ϸ����":
                        textBox.Text = record.Description;
                        break;
                }
            }

            // ���������
            foreach (var comboBox in comboBoxes)
            {
                if (comboBox.Header?.ToString() == "��¼����")
                {
                    for (int i = 0; i < comboBox.Items.Count; i++)
                    {
                        if (comboBox.Items[i] is ComboBoxItem item && item.Content?.ToString() == record.Type)
                        {
                            comboBox.SelectedIndex = i;
                            break;
                        }
                    }
                }
                else if (comboBox.Header?.ToString() == "��¼����")
                {
                    for (int i = 0; i < comboBox.Items.Count; i++)
                    {
                        if (comboBox.Items[i] is ComboBoxItem item && item.Content?.ToString() == record.Level)
                        {
                            comboBox.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }

            // �������
            foreach (var datePicker in datePickers)
            {
                if (datePicker.Header?.ToString() == "���ʱ��")
                {
                    datePicker.Date = record.Date ?? DateTimeOffset.Now;
                }
            }
        }

        #endregion

        #region �ɼ�¼���ǩҳ�¼�����

        private async void ImportGradesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var picker = new FileOpenPicker();
                picker.FileTypeFilter.Add(".xlsx");
                picker.FileTypeFilter.Add(".xls");

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(((App)Application.Current).MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                var file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    // ʹ�� ExcelImportService ����ɼ�����
                    var scores = await ExcelImportService.ImportGradesFromExcelAsync(file);

                    // �� StudentScore ת��Ϊ GradeInfo
                    foreach (var score in scores)
                    {
                        try
                        {
                            var gradeDict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(score.Score);
                            if (gradeDict != null)
                            {
                                foreach (var grade in gradeDict)
                                {
                                    if (double.TryParse(grade.Value, out double scoreValue))
                                    {
                                        var gradeInfo = new GradeInfo
                                        {
                                            StudentId = score.StudentUuid,
                                            StudentName = GetStudentNameById(score.StudentUuid),
                                            CourseName = grade.Key,
                                            Score = scoreValue,
                                            Semester = score.Term,
                                            GradeType = "����ɼ�",
                                            Credits = 3.0, // Ĭ��ѧ�֣��ɸ�����Ҫ����
                                            GPA = CalculateGPA(scoreValue),
                                            Remarks = ""
                                        };
                                        Grades.Add(gradeInfo);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // ��¼�������󵫼���������������
                            System.Diagnostics.Debug.WriteLine($"�����ɼ�����ʱ����: {ex.Message}");
                        }
                    }

                    ShowInfoDialog("����ɹ�", $"�ɼ����ݵ�����ɣ������� {scores.Count} ����¼��");
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog("����ʧ��", $"�ɼ�����ʱ��������{ex.Message}");
            }
        }

        private string GetStudentNameById(string studentId)
        {
            var student = Students.FirstOrDefault(s => s.StudentId == studentId);
            return student?.Name ?? "δ֪ѧ��";
        }

        private double CalculateGPA(double score)
        {
            // �򵥵�GPA�����߼�
            if (score >= 90) return 4.0;
            if (score >= 80) return 3.0;
            if (score >= 70) return 2.0;
            if (score >= 60) return 1.0;
            return 0.0;
        }

        private void FilterGradesBySemester(string semester)
        {
            // TODO: ʵ�ְ�ѧ��ɸѡ�ɼ����߼�
            // var filteredGrades = await GradeService.GetGradesBySemesterAsync(semester);
            // Grades.Clear();
            // foreach (var grade in filteredGrades)
            // {
            //     Grades.Add(grade);
            // }
        }

        private void FilterGradesByMajor(string major)
        {
            // TODO: ʵ�ְ�רҵɸѡ�ɼ����߼�
            // var filteredGrades = await GradeService.GetGradesByMajorAsync(major);
            // Grades.Clear();
            // foreach (var grade in filteredGrades)
            // {
            //     Grades.Add(grade);
            // }
        }

        #endregion

        #region ���ͼ�¼��ǩҳ�¼�����

        private void AddRecordButton_Click(object sender, RoutedEventArgs e)
        {
            // ��ձ���׼������¼�¼
            _selectedRecord = null;
            ClearRecordForm();
        }

        private async void ImportRecordsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var picker = new FileOpenPicker();
                picker.FileTypeFilter.Add(".xlsx");
                picker.FileTypeFilter.Add(".xls");

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(((App)Application.Current).MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                var file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    // ʹ�� ExcelImportService ���뽱�ͼ�¼
                    var records = await ExcelImportService.ImportRecordsFromExcelAsync(file);

                    foreach (var record in records)
                    {
                        var rewardPunishmentRecord = new RewardPunishmentRecord
                        {
                            StudentId = GetStudentIdFromRecord(record),
                            StudentName = GetStudentNameById(GetStudentIdFromRecord(record)),
                            Type = MapRecordTypeToDisplay(record.RecordType),
                            Description = record.Description ?? "",
                            Name = ExtractRecordName(record.Description ?? ""),
                            Level = "У��", // Ĭ�ϼ���
                            Date = DateTimeOffset.Now, // Ĭ�ϵ�ǰʱ��
                            Organization = "ѧУ"
                        };
                        Records.Add(rewardPunishmentRecord);
                    }

                    ShowInfoDialog("����ɹ�", $"���ͼ�¼������ɣ������� {records.Count} ����¼��");
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog("����ʧ��", $"���ͼ�¼����ʱ��������{ex.Message}");
            }
        }

        private string GetStudentIdFromRecord(StudentRecord record)
        {
            // ���� StudentRecord ʹ�� int StudentId ��������Ҫ string��������Ҫת���߼�
            // ������Ҫͨ�����ݿ��ѯ��ȡʵ�ʵ�ѧ��
            return record.StudentId.ToString(); // ��ʱʵ��
        }

        private string MapRecordTypeToDisplay(string recordType)
        {
            return recordType switch
            {
                "Award" => "����",
                "Punishment" => "����",
                "Grade" => "�ɼ�",
                _ => recordType
            };
        }

        private string ExtractRecordName(string description)
        {
            // ����������ȡ�������Ƶļ��߼�
            if (description.Length > 20)
                return description.Substring(0, 20) + "...";
            return description;
        }

        private async void SaveRecordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var record = GetRecordFromForm();
                if (record == null) return;

                if (_selectedRecord == null)
                {
                    // ������¼
                    // TODO: ���浽���ݿ�
                    // await RecordService.AddRecordAsync(record);

                    Records.Add(record);
                    ShowInfoDialog("����ɹ�", "���ͼ�¼����ӣ�");
                }
                else
                {
                    // ���¼�¼
                    // TODO: �������ݿ�
                    // await RecordService.UpdateRecordAsync(record);

                    var index = Records.IndexOf(_selectedRecord);
                    if (index >= 0)
                    {
                        Records[index] = record;
                    }
                    ShowInfoDialog("����ɹ�", "���ͼ�¼�Ѹ��£�");
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog("����ʧ��", $"���潱�ͼ�¼ʱ��������{ex.Message}");
            }
        }

        private RewardPunishmentRecord? GetRecordFromForm()
        {
            var textBoxes = new List<TextBox>();
            var comboBoxes = new List<ComboBox>();
            var datePickers = new List<DatePicker>();

            // ���ҽ��ͼ�¼���ؼ�
            var recordPanel = FindRecordDetailsPanel();
            if (recordPanel != null)
            {
                FindFormControls(recordPanel, textBoxes, comboBoxes, datePickers);
            }

            var record = new RewardPunishmentRecord();

            foreach (var textBox in textBoxes)
            {
                switch (textBox.Header?.ToString())
                {
                    case "ѧ��":
                        if (string.IsNullOrWhiteSpace(textBox.Text))
                        {
                            ShowErrorDialog("��֤ʧ��", "ѧ�Ų���Ϊ�գ�");
                            return null;
                        }
                        record.StudentId = textBox.Text.Trim();
                        break;
                    case "ѧ������":
                        if (string.IsNullOrWhiteSpace(textBox.Text))
                        {
                            ShowErrorDialog("��֤ʧ��", "ѧ����������Ϊ�գ�");
                            return null;
                        }
                        record.StudentName = textBox.Text.Trim();
                        break;
                    case "��������":
                        if (string.IsNullOrWhiteSpace(textBox.Text))
                        {
                            ShowErrorDialog("��֤ʧ��", "�������Ʋ���Ϊ�գ�");
                            return null;
                        }
                        record.Name = textBox.Text.Trim();
                        break;
                    case "�䷢����":
                        record.Organization = textBox.Text.Trim();
                        break;
                    case "��ϸ����":
                        record.Description = textBox.Text.Trim();
                        break;
                }
            }

            foreach (var comboBox in comboBoxes)
            {
                if (comboBox.Header?.ToString() == "��¼����" && comboBox.SelectedItem is ComboBoxItem typeItem)
                {
                    record.Type = typeItem.Content?.ToString() ?? "";
                }
                else if (comboBox.Header?.ToString() == "��¼����" && comboBox.SelectedItem is ComboBoxItem levelItem)
                {
                    record.Level = levelItem.Content?.ToString() ?? "";
                }
            }

            foreach (var datePicker in datePickers)
            {
                if (datePicker.Header?.ToString() == "���ʱ��")
                {
                    record.Date = datePicker.Date;
                }
            }

            return record;
        }

        private StackPanel? FindRecordDetailsPanel()
        {
            // �ݹ���ҽ��ͼ�¼�������
            return FindChildOfType<StackPanel>(this, "���ͼ�¼����");
        }

        private void ClearRecordForm()
        {
            var textBoxes = new List<TextBox>();
            var comboBoxes = new List<ComboBox>();
            var datePickers = new List<DatePicker>();

            var recordPanel = FindRecordDetailsPanel();
            if (recordPanel != null)
            {
                FindFormControls(recordPanel, textBoxes, comboBoxes, datePickers);

                foreach (var textBox in textBoxes)
                {
                    textBox.Text = string.Empty;
                }

                foreach (var comboBox in comboBoxes)
                {
                    comboBox.SelectedIndex = -1;
                }

                foreach (var datePicker in datePickers)
                {
                    datePicker.Date = DateTimeOffset.Now;
                }
            }
        }

        private async void DeleteRecordButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRecord == null)
            {
                ShowErrorDialog("����ʧ��", "����ѡ��Ҫɾ���ļ�¼��");
                return;
            }

            var result = await ShowConfirmDialog("ȷ��ɾ��", $"ȷ��Ҫɾ�� {_selectedRecord.StudentName} �� {_selectedRecord.Name} ��¼�𣿴˲������ɳ�����");
            if (result)
            {
                try
                {
                    // TODO: �����ݿ�ɾ��
                    // await RecordService.DeleteRecordAsync(_selectedRecord.Id);

                    Records.Remove(_selectedRecord);
                    _selectedRecord = null;
                    ClearRecordForm();

                    ShowInfoDialog("ɾ���ɹ�", "���ͼ�¼��ɾ����");
                }
                catch (Exception ex)
                {
                    ShowErrorDialog("ɾ��ʧ��", $"ɾ�����ͼ�¼ʱ��������{ex.Message}");
                }
            }
        }

        private void ClearRecordButton_Click(object sender, RoutedEventArgs e)
        {
            ClearRecordForm();
        }

        private async void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var picker = new FileOpenPicker();
                picker.FileTypeFilter.Add(".pdf");
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");
                picker.FileTypeFilter.Add(".doc");
                picker.FileTypeFilter.Add(".docx");

                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(((App)Application.Current).MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);

                var file = await picker.PickSingleFileAsync();
                if (file != null)
                {
                    // TODO: ����֤���ļ��ϴ�
                    // var uploadedPath = await FileUploadService.UploadCertificateAsync(file);
                    // if (_selectedRecord != null)
                    // {
                    //     _selectedRecord.CertificateFile = uploadedPath;
                    // }

                    ShowInfoDialog("�ϴ��ɹ�", $"֤���ļ� {file.Name} ��ѡ��");
                }
            }
            catch (Exception ex)
            {
                ShowErrorDialog("�ļ�ѡ��ʧ��", $"ѡ��֤���ļ�ʱ��������{ex.Message}");
            }
        }

        private void FilterRecordsByType(string type)
        {
            // TODO: ʵ�ְ���¼����ɸѡ���߼�
            // var filteredRecords = await RecordService.GetRecordsByTypeAsync(type);
            // Records.Clear();
            // foreach (var record in filteredRecords)
            // {
            //     Records.Add(record);
            // }
        }

        private void FilterRecordsByLevel(string level)
        {
            // TODO: ʵ�ְ���¼����ɸѡ���߼�
            // var filteredRecords = await RecordService.GetRecordsByLevelAsync(level);
            // Records.Clear();
            // foreach (var record in filteredRecords)
            // {
            //     Records.Add(record);
            // }
        }

        #endregion

        #region ComboBox�¼�����

        private void SemesterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var semester = selectedItem.Content?.ToString();
                if (!string.IsNullOrEmpty(semester))
                {
                    FilterGradesBySemester(semester);
                }
            }
        }

        private void MajorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var major = selectedItem.Content?.ToString();
                if (!string.IsNullOrEmpty(major))
                {
                    FilterGradesByMajor(major);
                }
            }
        }

        private void RecordTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var recordType = selectedItem.Content?.ToString();
                if (!string.IsNullOrEmpty(recordType))
                {
                    FilterRecordsByType(recordType);
                }
            }
        }

        private void RecordLevelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var recordLevel = selectedItem.Content?.ToString();
                if (!string.IsNullOrEmpty(recordLevel))
                {
                    FilterRecordsByLevel(recordLevel);
                }
            }
        }

        #endregion

        #region ͨ�öԻ���͸�������

        private async void ShowInfoDialog(string title, string message)
        {
            var dialog = new ContentDialog()
            {
                Title = title,
                Content = message,
                CloseButtonText = "ȷ��",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async void ShowErrorDialog(string title, string message)
        {
            var dialog = new ContentDialog()
            {
                Title = title,
                Content = message,
                CloseButtonText = "ȷ��",
                XamlRoot = this.XamlRoot
            };
            await dialog.ShowAsync();
        }

        private async Task<bool> ShowConfirmDialog(string title, string message)
        {
            var dialog = new ContentDialog()
            {
                Title = title,
                Content = message,
                PrimaryButtonText = "ȷ��",
                CloseButtonText = "ȡ��",
                XamlRoot = this.XamlRoot
            };

            var result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        private void SearchStudents(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // TODO: ��ʾ����ѧ��
                // LoadInitialData();
                return;
            }

            // TODO: ʵ��ѧ�������߼�
            // var filteredStudents = await StudentService.SearchStudentsAsync(searchText);
            // Students.Clear();
            // foreach (var student in filteredStudents)
            // {
            //     Students.Add(student);
            // }
        }

        #endregion
    }

    // ת������
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string status)
            {
                return status switch
                {
                    "��У" => new SolidColorBrush(Colors.Green),
                    "��ѧ" => new SolidColorBrush(Colors.Orange),
                    "��ѧ" => new SolidColorBrush(Colors.Red),
                    "��ҵ" => new SolidColorBrush(Colors.Blue),
                    _ => new SolidColorBrush(Colors.Gray)
                };
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class RecordTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string type)
            {
                return type switch
                {
                    "����" => new SolidColorBrush(Colors.Green),
                    "�ͷ�" => new SolidColorBrush(Colors.Red),
                    _ => new SolidColorBrush(Colors.Gray)
                };
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public partial class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTimeOffset dateTimeOffset)
            {
                return dateTimeOffset.ToString("yyyy-MM-dd");
            }
            if (value is DateTime date)
            {
                return date.ToString("yyyy-MM-dd");
            }
            if (value is DateOnly dateOnly)
            {
                return dateOnly.ToString("yyyy-MM-dd");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string dateString && DateTimeOffset.TryParse(dateString, out DateTimeOffset result))
            {
                return result;
            }
            return DateTimeOffset.Now;
        }
    }
}
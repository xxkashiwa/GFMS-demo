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

namespace GFMS.Pages
{
    // 学生信息数据模型
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
        public string Status { get; set; } = "未完成";
    }

    // 成绩信息数据模型
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

    // 奖惩记录数据模型
    public class RewardPunishmentRecord
    {
        public string StudentId { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "奖励" 或 "处分"
        public string Name { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public DateTimeOffset? Date { get; set; }
        public string Organization { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CertificateFile { get; set; } = string.Empty;
    }

    public sealed partial class DataCollectionPage : Page
    {
        // 数据集合
        public ObservableCollection<StudentInfo> Students { get; set; }
        public ObservableCollection<GradeInfo> Grades { get; set; }
        public ObservableCollection<RewardPunishmentRecord> Records { get; set; }

        // 当前选中的学生
        private StudentInfo? _selectedStudent;
        private RewardPunishmentRecord? _selectedRecord;

        public DataCollectionPage()
        {
            InitializeComponent();
            
            // 初始化数据集合
            Students = new ObservableCollection<StudentInfo>();
            Grades = new ObservableCollection<GradeInfo>();
            Records = new ObservableCollection<RewardPunishmentRecord>();

            // 绑定数据源
            StudentListView.ItemsSource = Students;
            GradeListView.ItemsSource = Grades;
            RewardPunishmentListView.ItemsSource = Records;

            // 注册事件处理器
            RegisterEventHandlers();

            // 加载初始数据
            LoadInitialData();
        }

        private void RegisterEventHandlers()
        {
            // 基础信息标签页事件
            StudentListView.SelectionChanged += StudentListView_SelectionChanged;
            
            // 奖惩记录列表选择事件
            RewardPunishmentListView.SelectionChanged += RewardPunishmentListView_SelectionChanged;
            
            // 查找并注册按钮事件
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

            // 注册其他按钮事件
            RegisterButtonEvents();
            
            // 注册筛选控件事件
            RegisterFilterEvents();
            
            // 注册搜索框事件
            RegisterSearchEvents();
        }

        private void RegisterButtonEvents()
        {
            // 基础信息页面按钮
            var buttons = GetButtonsFromGrid();
            foreach (var button in buttons)
            {
                var content = GetButtonContent(button);
                switch (content)
                {
                    case "新增学生":
                        button.Click += AddStudentButton_Click;
                        break;
                    case "导出数据":
                        button.Click += ExportDataButton_Click;
                        break;
                    case "保存信息":
                        button.Click += SaveStudentButton_Click;
                        break;
                    case "删除学生":
                        button.Click += DeleteStudentButton_Click;
                        break;
                    case "重置":
                        button.Click += ResetStudentButton_Click;
                        break;
                    case "批量导入成绩":
                        button.Click += ImportGradesButton_Click;
                        break;
                    case "添加记录":
                        button.Click += AddRecordButton_Click;
                        break;
                    case "批量导入":
                        button.Click += ImportRecordsButton_Click;
                        break;
                    case "保存记录":
                        button.Click += SaveRecordButton_Click;
                        break;
                    case "删除":
                        button.Click += DeleteRecordButton_Click;
                        break;
                    case "清空":
                        button.Click += ClearRecordButton_Click;
                        break;
                    case "选择文件":
                        button.Click += SelectFileButton_Click;
                        break;
                }
            }
        }

        private List<Button> GetButtonsFromGrid()
        {
            var buttons = new List<Button>();
            // 递归查找所有Button控件
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
            // 查找并注册筛选控件事件
            var comboBoxes = GetComboBoxesFromGrid();
            foreach (var comboBox in comboBoxes)
            {
                var placeholder = comboBox.PlaceholderText;
                switch (placeholder)
                {
                    case "选择学期":
                        comboBox.SelectionChanged += (s, e) => FilterGradesBySemester((comboBox.SelectedItem as ComboBoxItem)?.Content?.ToString());
                        break;
                    case "选择专业":
                        comboBox.SelectionChanged += (s, e) => FilterGradesByMajor((comboBox.SelectedItem as ComboBoxItem)?.Content?.ToString());
                        break;
                    case "记录类型":
                        comboBox.SelectionChanged += (s, e) => FilterRecordsByType((comboBox.SelectedItem as ComboBoxItem)?.Content?.ToString());
                        break;
                    case "记录级别":
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
             // 查找搜索框并注册事件
             var textBoxes = GetTextBoxesFromGrid();
             foreach (var textBox in textBoxes)
             {
                 if (textBox.PlaceholderText?.Contains("搜索") == true)
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
            // TODO: 从数据库加载学生信息
            // var students = await StudentService.GetAllStudentsAsync();
            // foreach (var student in students)
            // {
            //     Students.Add(student);
            // }

            // 临时添加示例数据
            Students.Add(new StudentInfo
            {
                StudentId = "2021001",
                Name = "张三",
                Gender = "男",
                Major = "计算机科学与技术",
                Class = "计科2101",
                Status = "已完成"
            });

            Students.Add(new StudentInfo
            {
                StudentId = "2021002",
                Name = "李四",
                Gender = "女",
                Major = "软件工程",
                Class = "软工2101",
                Status = "未完成"
            });

            // TODO: 加载成绩数据
            // var grades = await GradeService.GetAllGradesAsync();
            // foreach (var grade in grades)
            // {
            //     Grades.Add(grade);
            // }

            // 临时添加示例成绩数据
            Grades.Add(new GradeInfo
            {
                StudentId = "2021001",
                StudentName = "张三",
                CourseName = "高等数学",
                Credits = 4,
                Score = 85,
                GPA = 3.2,
                GradeType = "期末成绩",
                Semester = "2023-2024春季"
            });

            // TODO: 加载奖惩记录
            // var records = await RecordService.GetAllRecordsAsync();
            // foreach (var record in records)
            // {
            //     Records.Add(record);
            // }

            // 临时添加示例奖惩记录
             Records.Add(new RewardPunishmentRecord
             {
                 StudentId = "2021001",
                 StudentName = "张三",
                 Type = "奖励",
                 Name = "三好学生",
                 Level = "校级",
                 Date = DateTimeOffset.Parse("2023-12-01")
             });
         }

         #region 基础信息标签页事件处理

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
             // 查找详情面板中的控件并填充数据
             var detailsPanel = FindDetailsPanel();
             if (detailsPanel != null)
             {
                 FillStudentDetailsForm(detailsPanel, student);
             }
         }

         private StackPanel? FindDetailsPanel()
         {
             // 递归查找详情面板
             return FindChildOfType<StackPanel>(this, "学生详细信息");
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
             // 查找并填充表单控件
             var textBoxes = new List<TextBox>();
             var comboBoxes = new List<ComboBox>();
             var datePickers = new List<DatePicker>();
             
             FindFormControls(panel, textBoxes, comboBoxes, datePickers);

             // 根据Header属性填充数据
             foreach (var textBox in textBoxes)
             {
                 switch (textBox.Header?.ToString())
                 {
                     case "学号":
                         textBox.Text = student.StudentId;
                         break;
                     case "姓名":
                         textBox.Text = student.Name;
                         break;
                     case "身份证号":
                         textBox.Text = student.IdCard;
                         break;
                     case "专业":
                         textBox.Text = student.Major;
                         break;
                     case "班级":
                         textBox.Text = student.Class;
                         break;
                     case "联系电话":
                         textBox.Text = student.Phone;
                         break;
                     case "邮箱地址":
                         textBox.Text = student.Email;
                         break;
                     case "家庭住址":
                         textBox.Text = student.Address;
                         break;
                     case "家长姓名":
                         textBox.Text = student.ParentName;
                         break;
                     case "家长联系方式":
                         textBox.Text = student.ParentPhone;
                         break;
                 }
             }

             // 填充性别下拉框
             foreach (var comboBox in comboBoxes)
             {
                 if (comboBox.Header?.ToString() == "性别")
                 {
                     comboBox.SelectedIndex = student.Gender == "男" ? 0 : 1;
                 }
             }

             // 填充出生日期
             foreach (var datePicker in datePickers)
             {
                 if (datePicker.Header?.ToString() == "出生日期")
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
                     case "从Excel导入":
                         await ImportFromExcel();
                         break;
                     case "从教务系统导入":
                         await ImportFromEducationSystem();
                         break;
                     case "下载模板":
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
                     // TODO: 实现Excel文件解析逻辑
                     // var students = await ExcelImportService.ImportStudentsFromExcel(file);
                     // foreach (var student in students)
                     // {
                     //     Students.Add(student);
                     // }
                     
                     ShowInfoDialog("导入成功", "Excel文件导入完成！");
                 }
             }
             catch (Exception ex)
             {
                 ShowErrorDialog("导入失败", $"Excel导入时发生错误：{ex.Message}");
             }
         }

         private async Task ImportFromEducationSystem()
         {
             try
             {
                 // TODO: 实现从教务系统导入的逻辑
                 // var students = await EducationSystemService.ImportStudentsAsync();
                 // foreach (var student in students)
                 // {
                 //     Students.Add(student);
                 // }
                 
                 ShowInfoDialog("导入成功", "从教务系统导入完成！");
             }
             catch (Exception ex)
             {
                 ShowErrorDialog("导入失败", $"从教务系统导入时发生错误：{ex.Message}");
             }
         }

         private async Task DownloadTemplate()
         {
             try
             {
                 var picker = new FileSavePicker();
                 picker.FileTypeChoices.Add("Excel文件", new List<string> { ".xlsx" });
                 picker.SuggestedFileName = "学生信息导入模板";
                 
                 var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(((App)Application.Current).MainWindow);
                 WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
                 
                 var file = await picker.PickSaveFileAsync();
                 if (file != null)
                 {
                     // TODO: 生成Excel模板文件
                     // await ExcelTemplateService.GenerateStudentTemplate(file);
                     
                     ShowInfoDialog("下载成功", "模板文件已保存！");
                 }
             }
             catch (Exception ex)
             {
                 ShowErrorDialog("下载失败", $"模板下载时发生错误：{ex.Message}");
             }
         }

         private void AddStudentButton_Click(object sender, RoutedEventArgs e)
         {
             // 清空表单并准备添加新学生
             _selectedStudent = null;
             ClearStudentForm();
             
             // 生成新的学号
             var newStudentId = GenerateNewStudentId();
             var textBoxes = new List<TextBox>();
             FindFormControls(this, textBoxes, new List<ComboBox>(), new List<DatePicker>());
             
             foreach (var textBox in textBoxes)
             {
                 if (textBox.Header?.ToString() == "学号")
                 {
                     textBox.Text = newStudentId;
                     break;
                 }
             }
         }

         private string GenerateNewStudentId()
         {
             // TODO: 实现学号生成逻辑
             // return StudentService.GenerateNewStudentId();
             
             // 临时实现：基于当前年份和序号
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
                 picker.FileTypeChoices.Add("Excel文件", new List<string> { ".xlsx" });
                 picker.SuggestedFileName = $"学生信息_{DateTime.Now:yyyyMMdd}";
                 
                 var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(((App)Application.Current).MainWindow);
                 WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
                 
                 var file = await picker.PickSaveFileAsync();
                 if (file != null)
                 {
                     // TODO: 实现数据导出逻辑
                     // await ExcelExportService.ExportStudentsToExcel(Students.ToList(), file);
                     
                     ShowInfoDialog("导出成功", "学生数据已导出到Excel文件！");
                 }
             }
             catch (Exception ex)
             {
                 ShowErrorDialog("导出失败", $"数据导出时发生错误：{ex.Message}");
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
                     // 新增学生
                     // TODO: 保存到数据库
                     // await StudentService.AddStudentAsync(student);
                     
                     Students.Add(student);
                     ShowInfoDialog("保存成功", "学生信息已添加！");
                 }
                 else
                 {
                     // 更新学生信息
                     // TODO: 更新数据库
                     // await StudentService.UpdateStudentAsync(student);
                     
                     var index = Students.IndexOf(_selectedStudent);
                     if (index >= 0)
                     {
                         Students[index] = student;
                     }
                     ShowInfoDialog("保存成功", "学生信息已更新！");
                 }
             }
             catch (Exception ex)
             {
                 ShowErrorDialog("保存失败", $"保存学生信息时发生错误：{ex.Message}");
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
                     case "学号":
                         if (string.IsNullOrWhiteSpace(textBox.Text))
                         {
                             ShowErrorDialog("验证失败", "学号不能为空！");
                             return null;
                         }
                         student.StudentId = textBox.Text.Trim();
                         break;
                     case "姓名":
                         if (string.IsNullOrWhiteSpace(textBox.Text))
                         {
                             ShowErrorDialog("验证失败", "姓名不能为空！");
                             return null;
                         }
                         student.Name = textBox.Text.Trim();
                         break;
                     case "身份证号":
                         student.IdCard = textBox.Text.Trim();
                         break;
                     case "专业":
                         student.Major = textBox.Text.Trim();
                         break;
                     case "班级":
                         student.Class = textBox.Text.Trim();
                         break;
                     case "联系电话":
                         student.Phone = textBox.Text.Trim();
                         break;
                     case "邮箱地址":
                         student.Email = textBox.Text.Trim();
                         break;
                     case "家庭住址":
                         student.Address = textBox.Text.Trim();
                         break;
                     case "家长姓名":
                         student.ParentName = textBox.Text.Trim();
                         break;
                     case "家长联系方式":
                         student.ParentPhone = textBox.Text.Trim();
                         break;
                 }
             }

             foreach (var comboBox in comboBoxes)
             {
                 if (comboBox.Header?.ToString() == "性别" && comboBox.SelectedItem is ComboBoxItem item)
                 {
                     student.Gender = item.Content?.ToString() ?? "";
                 }
             }

             foreach (var datePicker in datePickers)
             {
                 if (datePicker.Header?.ToString() == "出生日期")
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
                 ShowErrorDialog("操作失败", "请先选择要删除的学生！");
                 return;
             }

             var result = await ShowConfirmDialog("确认删除", $"确定要删除学生 {_selectedStudent.Name} 的信息吗？此操作不可撤销。");
             if (result)
             {
                 try
                 {
                     // TODO: 从数据库删除
                     // await StudentService.DeleteStudentAsync(_selectedStudent.StudentId);
                     
                     Students.Remove(_selectedStudent);
                     _selectedStudent = null;
                     ClearStudentForm();
                     
                     ShowInfoDialog("删除成功", "学生信息已删除！");
                 }
                 catch (Exception ex)
                 {
                     ShowErrorDialog("删除失败", $"删除学生信息时发生错误：{ex.Message}");
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
             // 查找详情面板中的控件并填充数据
             var detailsPanel = FindRecordDetailsPanel();
             if (detailsPanel != null)
             {
                 FillRecordDetailsForm(detailsPanel, record);
             }
         }

         private void FillRecordDetailsForm(StackPanel panel, RewardPunishmentRecord record)
          {
              // 查找并填充表单控件
              var textBoxes = new List<TextBox>();
              var comboBoxes = new List<ComboBox>();
              var datePickers = new List<DatePicker>();
              
              FindFormControls(panel, textBoxes, comboBoxes, datePickers);

              // 辅助方法：递归查找表单控件
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

             // 根据Header属性填充数据
             foreach (var textBox in textBoxes)
             {
                 switch (textBox.Header?.ToString())
                 {
                     case "学号":
                         textBox.Text = record.StudentId;
                         break;
                     case "学生姓名":
                         textBox.Text = record.StudentName;
                         break;
                     case "奖惩名称":
                         textBox.Text = record.Name;
                         break;
                     case "颁发机构":
                         textBox.Text = record.Organization;
                         break;
                     case "详细描述":
                         textBox.Text = record.Description;
                         break;
                 }
             }

             // 填充下拉框
             foreach (var comboBox in comboBoxes)
             {
                 if (comboBox.Header?.ToString() == "记录类型")
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
                 else if (comboBox.Header?.ToString() == "记录级别")
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

             // 填充日期
             foreach (var datePicker in datePickers)
             {
                 if (datePicker.Header?.ToString() == "获得时间")
                 {
                     datePicker.Date = record.Date ?? DateTimeOffset.Now;
                 }
             }
         }

         #endregion

         #region 成绩录入标签页事件处理

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
                     // TODO: 实现成绩Excel文件解析逻辑
                     // var grades = await ExcelImportService.ImportGradesFromExcel(file);
                     // foreach (var grade in grades)
                     // {
                     //     Grades.Add(grade);
                     // }
                     
                     ShowInfoDialog("导入成功", "成绩数据导入完成！");
                 }
             }
             catch (Exception ex)
             {
                 ShowErrorDialog("导入失败", $"成绩导入时发生错误：{ex.Message}");
             }
         }

         private void FilterGradesBySemester(string semester)
         {
             // TODO: 实现按学期筛选成绩的逻辑
             // var filteredGrades = await GradeService.GetGradesBySemesterAsync(semester);
             // Grades.Clear();
             // foreach (var grade in filteredGrades)
             // {
             //     Grades.Add(grade);
             // }
         }

         private void FilterGradesByMajor(string major)
         {
             // TODO: 实现按专业筛选成绩的逻辑
             // var filteredGrades = await GradeService.GetGradesByMajorAsync(major);
             // Grades.Clear();
             // foreach (var grade in filteredGrades)
             // {
             //     Grades.Add(grade);
             // }
         }

         #endregion

         #region 奖惩记录标签页事件处理

         private void AddRecordButton_Click(object sender, RoutedEventArgs e)
         {
             // 清空表单并准备添加新记录
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
                     // TODO: 实现奖惩记录Excel文件解析逻辑
                     // var records = await ExcelImportService.ImportRecordsFromExcel(file);
                     // foreach (var record in records)
                     // {
                     //     Records.Add(record);
                     // }
                     
                     ShowInfoDialog("导入成功", "奖惩记录导入完成！");
                 }
             }
             catch (Exception ex)
             {
                 ShowErrorDialog("导入失败", $"奖惩记录导入时发生错误：{ex.Message}");
             }
         }

         private async void SaveRecordButton_Click(object sender, RoutedEventArgs e)
         {
             try
             {
                 var record = GetRecordFromForm();
                 if (record == null) return;

                 if (_selectedRecord == null)
                 {
                     // 新增记录
                     // TODO: 保存到数据库
                     // await RecordService.AddRecordAsync(record);
                     
                     Records.Add(record);
                     ShowInfoDialog("保存成功", "奖惩记录已添加！");
                 }
                 else
                 {
                     // 更新记录
                     // TODO: 更新数据库
                     // await RecordService.UpdateRecordAsync(record);
                     
                     var index = Records.IndexOf(_selectedRecord);
                     if (index >= 0)
                     {
                         Records[index] = record;
                     }
                     ShowInfoDialog("保存成功", "奖惩记录已更新！");
                 }
             }
             catch (Exception ex)
             {
                 ShowErrorDialog("保存失败", $"保存奖惩记录时发生错误：{ex.Message}");
             }
         }

         private RewardPunishmentRecord? GetRecordFromForm()
         {
             var textBoxes = new List<TextBox>();
             var comboBoxes = new List<ComboBox>();
             var datePickers = new List<DatePicker>();
             
             // 查找奖惩记录表单控件
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
                     case "学号":
                         if (string.IsNullOrWhiteSpace(textBox.Text))
                         {
                             ShowErrorDialog("验证失败", "学号不能为空！");
                             return null;
                         }
                         record.StudentId = textBox.Text.Trim();
                         break;
                     case "学生姓名":
                         if (string.IsNullOrWhiteSpace(textBox.Text))
                         {
                             ShowErrorDialog("验证失败", "学生姓名不能为空！");
                             return null;
                         }
                         record.StudentName = textBox.Text.Trim();
                         break;
                     case "奖惩名称":
                         if (string.IsNullOrWhiteSpace(textBox.Text))
                         {
                             ShowErrorDialog("验证失败", "奖惩名称不能为空！");
                             return null;
                         }
                         record.Name = textBox.Text.Trim();
                         break;
                     case "颁发机构":
                         record.Organization = textBox.Text.Trim();
                         break;
                     case "详细描述":
                         record.Description = textBox.Text.Trim();
                         break;
                 }
             }

             foreach (var comboBox in comboBoxes)
             {
                 if (comboBox.Header?.ToString() == "记录类型" && comboBox.SelectedItem is ComboBoxItem typeItem)
                 {
                     record.Type = typeItem.Content?.ToString() ?? "";
                 }
                 else if (comboBox.Header?.ToString() == "记录级别" && comboBox.SelectedItem is ComboBoxItem levelItem)
                 {
                     record.Level = levelItem.Content?.ToString() ?? "";
                 }
             }

             foreach (var datePicker in datePickers)
             {
                 if (datePicker.Header?.ToString() == "获得时间")
                 {
                     record.Date = datePicker.Date;
                 }
             }

             return record;
         }

         private StackPanel? FindRecordDetailsPanel()
         {
             // 递归查找奖惩记录详情面板
             return FindChildOfType<StackPanel>(this, "奖惩记录详情");
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
                 ShowErrorDialog("操作失败", "请先选择要删除的记录！");
                 return;
             }

             var result = await ShowConfirmDialog("确认删除", $"确定要删除 {_selectedRecord.StudentName} 的 {_selectedRecord.Name} 记录吗？此操作不可撤销。");
             if (result)
             {
                 try
                 {
                     // TODO: 从数据库删除
                     // await RecordService.DeleteRecordAsync(_selectedRecord.Id);
                     
                     Records.Remove(_selectedRecord);
                     _selectedRecord = null;
                     ClearRecordForm();
                     
                     ShowInfoDialog("删除成功", "奖惩记录已删除！");
                 }
                 catch (Exception ex)
                 {
                     ShowErrorDialog("删除失败", $"删除奖惩记录时发生错误：{ex.Message}");
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
                     // TODO: 处理证书文件上传
                     // var uploadedPath = await FileUploadService.UploadCertificateAsync(file);
                     // if (_selectedRecord != null)
                     // {
                     //     _selectedRecord.CertificateFile = uploadedPath;
                     // }
                     
                     ShowInfoDialog("上传成功", $"证书文件 {file.Name} 已选择！");
                 }
             }
             catch (Exception ex)
             {
                 ShowErrorDialog("文件选择失败", $"选择证书文件时发生错误：{ex.Message}");
             }
         }

         private void FilterRecordsByType(string type)
         {
             // TODO: 实现按记录类型筛选的逻辑
             // var filteredRecords = await RecordService.GetRecordsByTypeAsync(type);
             // Records.Clear();
             // foreach (var record in filteredRecords)
             // {
             //     Records.Add(record);
             // }
         }

         private void FilterRecordsByLevel(string level)
         {
             // TODO: 实现按记录级别筛选的逻辑
             // var filteredRecords = await RecordService.GetRecordsByLevelAsync(level);
             // Records.Clear();
             // foreach (var record in filteredRecords)
             // {
             //     Records.Add(record);
             // }
         }

         #endregion

         #region ComboBox事件处理

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

         #region 通用对话框和辅助方法

         private async void ShowInfoDialog(string title, string message)
         {
             var dialog = new ContentDialog()
             {
                 Title = title,
                 Content = message,
                 CloseButtonText = "确定",
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
                 CloseButtonText = "确定",
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
                 PrimaryButtonText = "确定",
                 CloseButtonText = "取消",
                 XamlRoot = this.XamlRoot
             };
             
             var result = await dialog.ShowAsync();
             return result == ContentDialogResult.Primary;
         }

         private void SearchStudents(string searchText)
         {
             if (string.IsNullOrWhiteSpace(searchText))
             {
                 // TODO: 显示所有学生
                 // LoadInitialData();
                 return;
             }

             // TODO: 实现学生搜索逻辑
             // var filteredStudents = await StudentService.SearchStudentsAsync(searchText);
             // Students.Clear();
             // foreach (var student in filteredStudents)
             // {
             //     Students.Add(student);
             // }
         }

         #endregion
     }

    // 转换器类
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string status)
            {
                return status switch
                {
                    "在校" => new SolidColorBrush(Colors.Green),
                    "休学" => new SolidColorBrush(Colors.Orange),
                    "退学" => new SolidColorBrush(Colors.Red),
                    "毕业" => new SolidColorBrush(Colors.Blue),
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
                    "奖励" => new SolidColorBrush(Colors.Green),
                    "惩罚" => new SolidColorBrush(Colors.Red),
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

    public class DateToStringConverter : IValueConverter
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

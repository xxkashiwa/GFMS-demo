using GFMS.Models;
using GFMS.Services;
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
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GFMS.Pages
{
    /// <summary>
    /// 档案管理页面，用于管理学生档案文件
    /// </summary>
    public sealed partial class FileManagementPage : Page
    {
        /// <summary>
        /// 用于绑定到ListView的学生档案管理项集合
        /// </summary>
        private ObservableCollection<StudentFileManagementItem> _fileManagementItems;

        public FileManagementPage()
        {
            InitializeComponent();
            
            // 初始化档案管理项集合
            _fileManagementItems = new ObservableCollection<StudentFileManagementItem>();
            
            // 设置ListView的数据源
            FileManagementListView.ItemsSource = _fileManagementItems;
            
            // 加载学生数据
            LoadStudentData();
        }

        /// <summary>
        /// 页面导航时的处理
        /// </summary>
        /// <param name="e">导航参数</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            // 每次进入页面时同步学生数据
            SyncStudentData();
        }

        /// <summary>
        /// 加载学生数据
        /// </summary>
        private void LoadStudentData()
        {
            SyncStudentData();
        }

        /// <summary>
        /// 同步学生数据，检测StudentManager中的新学生并追加到列表中
        /// </summary>
        private void SyncStudentData()
        {
            // 遍历StudentManager中的所有学生
            foreach (var student in StudentManager.Instance.Students)
            {
                // 检查当前集合中是否已存在该学生
                var existingItem = _fileManagementItems.FirstOrDefault(item => item.StudentId == student.StudentId);
                
                if (existingItem == null)
                {
                    // 如果不存在，则创建新的档案管理项并添加到集合中
                    var newItem = new StudentFileManagementItem
                    {
                        Student = student
                    };
                    newItem.UpdateFileStatuses();
                    _fileManagementItems.Add(newItem);
                }
                else
                {
                    // 如果存在，则更新学生信息和档案状态
                    existingItem.Student = student;
                    existingItem.UpdateFileStatuses();
                }
            }

            // 移除在StudentManager中不存在的学生
            var itemsToRemove = _fileManagementItems.Where(item => 
                !StudentManager.Instance.Students.Any(student => student.StudentId == item.StudentId)).ToList();
            
            foreach (var item in itemsToRemove)
            {
                _fileManagementItems.Remove(item);
            }
        }

        /// <summary>
        /// 毕业登记表按钮点击事件
        /// </summary>
        private void GraduationFormButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is StudentFileManagementItem item)
            {
                // TODO: 实现毕业登记表上传功能
                // 这里可以实现文件上传逻辑
                
                // 示例：添加一个毕业登记表文件记录
                // var file = new StudentFile
                // {
                //     StudentId = item.Student.StudentId,
                //     FileType = "毕业登记表",
                //     FilePath = "path/to/graduation/form.pdf",
                //     State = FileState.已上传,
                //     UpdatedAt = DateTime.Now
                // };
                // item.Student.Files.Add(file);
                // item.UpdateFileStatuses();
            }
        }

        /// <summary>
        /// 体检表按钮点击事件
        /// </summary>
        private void MedicalExamButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is StudentFileManagementItem item)
            {
                // TODO: 实现体检表上传功能
                // 这里可以实现文件上传逻辑
            }
        }

        /// <summary>
        /// 实习报告按钮点击事件
        /// </summary>
        private void InternshipReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is StudentFileManagementItem item)
            {
                // TODO: 实现实习报告上传功能
                // 这里可以实现文件上传逻辑
            }
        }

        /// <summary>
        /// 档案详细按钮点击事件
        /// </summary>
        private void FileDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is StudentFileManagementItem item)
            {
                // TODO: 实现档案详细查看功能
                // 这里可以显示该学生的所有档案文件详情
            }
        }
    }
}

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
    /// ��������ҳ�棬���ڹ���ѧ�������ļ�
    /// </summary>
    public sealed partial class FileManagementPage : Page
    {
        /// <summary>
        /// ���ڰ󶨵�ListView��ѧ�������������
        /// </summary>
        private ObservableCollection<StudentFileManagementItem> _fileManagementItems;

        public FileManagementPage()
        {
            InitializeComponent();
            
            // ��ʼ�������������
            _fileManagementItems = new ObservableCollection<StudentFileManagementItem>();
            
            // ����ListView������Դ
            FileManagementListView.ItemsSource = _fileManagementItems;
            
            // ����ѧ������
            LoadStudentData();
        }

        /// <summary>
        /// ҳ�浼��ʱ�Ĵ���
        /// </summary>
        /// <param name="e">��������</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            // ÿ�ν���ҳ��ʱͬ��ѧ������
            SyncStudentData();
        }

        /// <summary>
        /// ����ѧ������
        /// </summary>
        private void LoadStudentData()
        {
            SyncStudentData();
        }

        /// <summary>
        /// ͬ��ѧ�����ݣ����StudentManager�е���ѧ����׷�ӵ��б���
        /// </summary>
        private void SyncStudentData()
        {
            // ����StudentManager�е�����ѧ��
            foreach (var student in StudentManager.Instance.Students)
            {
                // ��鵱ǰ�������Ƿ��Ѵ��ڸ�ѧ��
                var existingItem = _fileManagementItems.FirstOrDefault(item => item.StudentId == student.StudentId);
                
                if (existingItem == null)
                {
                    // ��������ڣ��򴴽��µĵ����������ӵ�������
                    var newItem = new StudentFileManagementItem
                    {
                        Student = student
                    };
                    newItem.UpdateFileStatuses();
                    _fileManagementItems.Add(newItem);
                }
                else
                {
                    // ������ڣ������ѧ����Ϣ�͵���״̬
                    existingItem.Student = student;
                    existingItem.UpdateFileStatuses();
                }
            }

            // �Ƴ���StudentManager�в����ڵ�ѧ��
            var itemsToRemove = _fileManagementItems.Where(item => 
                !StudentManager.Instance.Students.Any(student => student.StudentId == item.StudentId)).ToList();
            
            foreach (var item in itemsToRemove)
            {
                _fileManagementItems.Remove(item);
            }
        }

        /// <summary>
        /// ��ҵ�ǼǱ�ť����¼�
        /// </summary>
        private void GraduationFormButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is StudentFileManagementItem item)
            {
                // TODO: ʵ�ֱ�ҵ�ǼǱ��ϴ�����
                // �������ʵ���ļ��ϴ��߼�
                
                // ʾ�������һ����ҵ�ǼǱ��ļ���¼
                // var file = new StudentFile
                // {
                //     StudentId = item.Student.StudentId,
                //     FileType = "��ҵ�ǼǱ�",
                //     FilePath = "path/to/graduation/form.pdf",
                //     State = FileState.���ϴ�,
                //     UpdatedAt = DateTime.Now
                // };
                // item.Student.Files.Add(file);
                // item.UpdateFileStatuses();
            }
        }

        /// <summary>
        /// ����ť����¼�
        /// </summary>
        private void MedicalExamButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is StudentFileManagementItem item)
            {
                // TODO: ʵ�������ϴ�����
                // �������ʵ���ļ��ϴ��߼�
            }
        }

        /// <summary>
        /// ʵϰ���水ť����¼�
        /// </summary>
        private void InternshipReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is StudentFileManagementItem item)
            {
                // TODO: ʵ��ʵϰ�����ϴ�����
                // �������ʵ���ļ��ϴ��߼�
            }
        }

        /// <summary>
        /// ������ϸ��ť����¼�
        /// </summary>
        private void FileDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is StudentFileManagementItem item)
            {
                // TODO: ʵ�ֵ�����ϸ�鿴����
                // ���������ʾ��ѧ�������е����ļ�����
            }
        }
    }
}

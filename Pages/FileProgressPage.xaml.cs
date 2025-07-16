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
    /// ����ת��ҳ�棬�����û���ɫ��ʾ��ͬ����
    /// </summary>
    public sealed partial class FileProgressPage : Page
    {
        /// <summary>
        /// ѧ���������¼����
        /// </summary>
        private ObservableCollection<FileTransferApplication> _studentApplications;

        /// <summary>
        /// ����Ա��ǰ�Ƿ���������ͼģʽ
        /// </summary>
        private bool _isAdminInApplicationView = false;

        public FileProgressPage()
        {
            InitializeComponent();
            
            _studentApplications = new ObservableCollection<FileTransferApplication>();
            
            // �����û���ɫ����ҳ�沼��
            SetupPageForUserRole();
        }

        /// <summary>
        /// ҳ�浼��ʱ�Ĵ���
        /// </summary>
        /// <param name="e">��������</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            // ÿ�ν���ҳ��ʱˢ������
            RefreshData();
        }

        /// <summary>
        /// �����û���ɫ����ҳ�沼��
        /// </summary>
        private void SetupPageForUserRole()
        {
            if (!UserManager.Instance.IsAuthed)
            {
                // δ��֤�û��������й���
                AdminControlPanel.Visibility = Visibility.Collapsed;
                StudentApplicationForm.Visibility = Visibility.Collapsed;
                StudentHistoryTable.Visibility = Visibility.Collapsed;
                AdminAllApplicationsTable.Visibility = Visibility.Collapsed;
                return;
            }

            var userRole = UserManager.Instance.AuthedUser?.GrantedType;

            switch (userRole)
            {
                case "Student":
                    // ѧ����ɫ����ʾ������͸�����ʷ��¼
                    AdminControlPanel.Visibility = Visibility.Collapsed;
                    StudentApplicationForm.Visibility = Visibility.Visible;
                    StudentHistoryTable.Visibility = Visibility.Visible;
                    AdminAllApplicationsTable.Visibility = Visibility.Collapsed;
                    
                    // �Զ���䵱ǰ�û���ѧ��
                    if (UserManager.Instance.AuthedUser != null)
                    {
                        StudentIdTextBox.Text = UserManager.Instance.AuthedUser.UserId;
                        StudentIdTextBox.IsReadOnly = true; // ѧ�������޸��Լ���ѧ��
                    }
                    break;
                    
                case "Admin":
                    // ����Ա��ɫ����ʾ������壬Ĭ����ʾ������
                    AdminControlPanel.Visibility = Visibility.Visible;
                    SetAdminView(false); // Ĭ�Ϲ�����ͼ
                    break;
                    
                default:
                    // ������ɫ�������й���
                    AdminControlPanel.Visibility = Visibility.Collapsed;
                    StudentApplicationForm.Visibility = Visibility.Collapsed;
                    StudentHistoryTable.Visibility = Visibility.Collapsed;
                    AdminAllApplicationsTable.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        /// <summary>
        /// ���ù���Ա��ͼģʽ
        /// </summary>
        /// <param name="isApplicationView">�Ƿ�Ϊ������ͼģʽ</param>
        private void SetAdminView(bool isApplicationView)
        {
            _isAdminInApplicationView = isApplicationView;
            
            if (isApplicationView)
            {
                // ������ͼ����ʾ���������ʷ��¼
                StudentApplicationForm.Visibility = Visibility.Visible;
                StudentHistoryTable.Visibility = Visibility.Visible;
                AdminAllApplicationsTable.Visibility = Visibility.Collapsed;
                
                // ����Ա�����޸�ѧ�ţ����Զ����
                StudentIdTextBox.IsReadOnly = false;
                StudentIdTextBox.Text = string.Empty;
                
                // ���°�ť�ı�
                SwitchToApplicationViewButton.Content = "ת��Ϊ������ͼ";
            }
            else
            {
                // ������ͼ����ʾ��������Ĺ�����
                StudentApplicationForm.Visibility = Visibility.Collapsed;
                StudentHistoryTable.Visibility = Visibility.Collapsed;
                AdminAllApplicationsTable.Visibility = Visibility.Visible;
                
                // ���°�ť�ı�
                SwitchToApplicationViewButton.Content = "ת��Ϊ��������ҳ";
            }
        }

        /// <summary>
        /// ����Ա�л���ͼ��ť����¼�
        /// </summary>
        private void SwitchToApplicationViewButton_Click(object sender, RoutedEventArgs e)
        {
            SetAdminView(!_isAdminInApplicationView);
            RefreshData();
        }

        /// <summary>
        /// ˢ������
        /// </summary>
        private void RefreshData()
        {
            if (!UserManager.Instance.IsAuthed) return;

            var userRole = UserManager.Instance.AuthedUser?.GrantedType;

            switch (userRole)
            {
                case "Student":
                    LoadStudentApplications();
                    break;
                    
                case "Admin":
                    if (_isAdminInApplicationView)
                    {
                        // ����Ա��������ͼʱ���������ض�ѧ�����ݣ����û��ֶ�����
                        _studentApplications.Clear();
                        StudentApplicationsListView.ItemsSource = _studentApplications;
                    }
                    else
                    {
                        LoadAllApplications();
                    }
                    break;
            }
        }

        /// <summary>
        /// ����ѧ���������¼
        /// </summary>
        private void LoadStudentApplications()
        {
            if (UserManager.Instance.AuthedUser == null) return;

            var studentId = UserManager.Instance.AuthedUser.UserId;
            var applications = FileTransferApplicationManager.Instance
                .GetApplicationsByStudentId(studentId);

            _studentApplications.Clear();
            foreach (var app in applications)
            {
                _studentApplications.Add(app);
            }

            StudentApplicationsListView.ItemsSource = _studentApplications;
        }

        /// <summary>
        /// �������������¼������Ա��
        /// </summary>
        private void LoadAllApplications()
        {
            AdminApplicationsListView.ItemsSource = 
                FileTransferApplicationManager.Instance.Applications;
        }

        /// <summary>
        /// �ύ���밴ť����¼�
        /// </summary>
        private void SubmitApplicationButton_Click(object sender, RoutedEventArgs e)
        {
            // ��֤������
            if (string.IsNullOrWhiteSpace(StudentIdTextBox.Text) ||
                string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                string.IsNullOrWhiteSpace(AddressTextBox.Text) ||
                string.IsNullOrWhiteSpace(TelephoneTextBox.Text))
            {
                ShowMessage("����д���б����ֶ�");
                return;
            }

            // �����µ�����
            var application = new FileTransferApplication
            {
                StudentId = StudentIdTextBox.Text.Trim(),
                Name = NameTextBox.Text.Trim(),
                Address = AddressTextBox.Text.Trim(),
                Telephone = TelephoneTextBox.Text.Trim(),
                Detail = DetailTextBox.Text.Trim(),
                State = TransferState.����Ԥ����,
                CreatedAt = DateTime.Now
            };

            // �������
            FileTransferApplicationManager.Instance.AddApplication(application);

            // ��ձ�
            ClearForm();

            // ˢ������
            if (UserManager.Instance.AuthedUser?.GrantedType == "Student")
            {
                LoadStudentApplications();
            }
            else if (_isAdminInApplicationView)
            {
                // ����Ա��������ͼʱ����ӵ���ʾ�б�
                _studentApplications.Add(application);
            }

            ShowMessage("�����ύ�ɹ���");
        }

        /// <summary>
        /// ״̬������ѡ��ı��¼�������Ա��
        /// </summary>
        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && 
                comboBox.Tag is FileTransferApplication application &&
                comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                var newStateString = selectedItem.Content?.ToString();
                if (!string.IsNullOrEmpty(newStateString))
                {
                    var newState = newStateString switch
                    {
                        "����Ԥ����" => TransferState.����Ԥ����,
                        "ת����" => TransferState.ת����,
                        "�����" => TransferState.�����,
                        _ => TransferState.����Ԥ����
                    };

                    // ��������״̬
                    FileTransferApplicationManager.Instance
                        .UpdateApplicationState(application.Id, newState);
                }
            }
        }

        /// <summary>
        /// ��ձ�
        /// </summary>
        private void ClearForm()
        {
            // ѧ���û������ѧ�ţ�����Ա�û��������
            if (UserManager.Instance.AuthedUser?.GrantedType == "Student")
            {
                // ѧ���û�����ѧ�Ų���
            }
            else
            {
                StudentIdTextBox.Text = string.Empty;
            }
            
            NameTextBox.Text = string.Empty;
            AddressTextBox.Text = string.Empty;
            TelephoneTextBox.Text = string.Empty;
            DetailTextBox.Text = string.Empty;
        }

        /// <summary>
        /// ��ʾ��Ϣ
        /// </summary>
        /// <param name="message">��Ϣ����</param>
        private async void ShowMessage(string message)
        {
            var dialog = new ContentDialog
            {
                Title = "��ʾ",
                Content = message,
                CloseButtonText = "ȷ��",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}

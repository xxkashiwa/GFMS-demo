using GFMS.Models;
using GFMS.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GFMS.Pages
{
    public sealed partial class HomePage : Page
    {
        private DispatcherTimer _timer;

        public HomePage()
        {
            InitializeComponent();
            
            // ��ʼ����ʱ�������ڸ���ϵͳʱ��
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
            
            // ���³�ʼϵͳʱ��
            UpdateSystemTime();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            // �����û���Ϣ
            UpdateUserInfo();
            
            // �����Ǳ������
            UpdateDashboardData();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            
            // ֹͣ��ʱ��
            _timer.Stop();
        }

        private void Timer_Tick(object sender, object e)
        {
            UpdateSystemTime();
        }

        private void UpdateSystemTime()
        {
            SystemTimeText.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void UpdateUserInfo()
        {
            if (UserManager.Instance.IsAuthed && UserManager.Instance.AuthedUser != null)
            {
                UserNameText.Text = UserManager.Instance.AuthedUser.UserName ?? UserManager.Instance.AuthedUser.UserId;
                
                // ��ʾ�û���ɫ
                string role = UserManager.Instance.AuthedUser.GrantedType switch
                {
                    "Admin" => "����Ա",
                    "Manager" => "����",
                    "Staff" => "��ͨԱ��",
                    "Student" => "ѧ��",
                    _ => "δ֪��ɫ"
                };
                
                UserRoleText.Text = role;
            }
        }

        /// <summary>
        /// �����Ǳ������
        /// </summary>
        private void UpdateDashboardData()
        {
            UpdateStudentStatistics();
            UpdateFileTransferStatistics();
        }

        /// <summary>
        /// ����ѧ������ͳ��
        /// </summary>
        private void UpdateStudentStatistics()
        {
            var students = StudentManager.Instance.Students;
            
            // ��ѧ����
            TotalStudentsText.Text = students.Count.ToString();
            
            // �ѱ�ҵѧ��������ҵ����С�ڵ��ڽ����ѧ����
            var graduatedStudents = students.Where(s => s.GraduationDate <= DateTime.Now).Count();
            GraduatedStudentsText.Text = graduatedStudents.ToString();
            
            // �гɼ���¼��ѧ����
            var studentsWithScores = students.Where(s => s.Scores.Any()).Count();
            StudentsWithScoresText.Text = studentsWithScores.ToString();
            
            // �н��ͼ�¼��ѧ����
            var studentsWithRewards = students.Where(s => s.RewardsAndPunishments.Any()).Count();
            StudentsWithRewardsText.Text = studentsWithRewards.ToString();
        }

        /// <summary>
        /// ���µ���ת������ͳ��
        /// </summary>
        private void UpdateFileTransferStatistics()
        {
            var applications = FileTransferApplicationManager.Instance.Applications;
            
            // ��������
            TotalApplicationsText.Text = applications.Count.ToString();
            
            // ��״̬ͳ��
            var preparingCount = applications.Where(a => a.State == TransferState.����Ԥ����).Count();
            PreparingApplicationsText.Text = preparingCount.ToString();
            
            var transferringCount = applications.Where(a => a.State == TransferState.ת����).Count();
            TransferringApplicationsText.Text = transferringCount.ToString();
            
            var completedCount = applications.Where(a => a.State == TransferState.�����).Count();
            CompletedApplicationsText.Text = completedCount.ToString();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserManager.Instance.Logout();
        }
    }
}

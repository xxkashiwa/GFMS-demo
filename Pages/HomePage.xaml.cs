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
            
            // 初始化定时器，用于更新系统时间
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
            _timer.Start();
            
            // 更新初始系统时间
            UpdateSystemTime();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
            // 更新用户信息
            UpdateUserInfo();
            
            // 更新仪表板数据
            UpdateDashboardData();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            
            // 停止定时器
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
                
                // 显示用户角色
                string role = UserManager.Instance.AuthedUser.GrantedType switch
                {
                    "Admin" => "管理员",
                    "Manager" => "经理",
                    "Staff" => "普通员工",
                    "Student" => "学生",
                    _ => "未知角色"
                };
                
                UserRoleText.Text = role;
            }
        }

        /// <summary>
        /// 更新仪表板数据
        /// </summary>
        private void UpdateDashboardData()
        {
            UpdateStudentStatistics();
            UpdateFileTransferStatistics();
        }

        /// <summary>
        /// 更新学生数据统计
        /// </summary>
        private void UpdateStudentStatistics()
        {
            var students = StudentManager.Instance.Students;
            
            // 总学生数
            TotalStudentsText.Text = students.Count.ToString();
            
            // 已毕业学生数（毕业日期小于等于今天的学生）
            var graduatedStudents = students.Where(s => s.GraduationDate <= DateTime.Now).Count();
            GraduatedStudentsText.Text = graduatedStudents.ToString();
            
            // 有成绩记录的学生数
            var studentsWithScores = students.Where(s => s.Scores.Any()).Count();
            StudentsWithScoresText.Text = studentsWithScores.ToString();
            
            // 有奖惩记录的学生数
            var studentsWithRewards = students.Where(s => s.RewardsAndPunishments.Any()).Count();
            StudentsWithRewardsText.Text = studentsWithRewards.ToString();
        }

        /// <summary>
        /// 更新档案转递申请统计
        /// </summary>
        private void UpdateFileTransferStatistics()
        {
            var applications = FileTransferApplicationManager.Instance.Applications;
            
            // 总申请数
            TotalApplicationsText.Text = applications.Count.ToString();
            
            // 按状态统计
            var preparingCount = applications.Where(a => a.State == TransferState.档案预备中).Count();
            PreparingApplicationsText.Text = preparingCount.ToString();
            
            var transferringCount = applications.Where(a => a.State == TransferState.转递中).Count();
            TransferringApplicationsText.Text = transferringCount.ToString();
            
            var completedCount = applications.Where(a => a.State == TransferState.已完成).Count();
            CompletedApplicationsText.Text = completedCount.ToString();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserManager.Instance.Logout();
        }
    }
}

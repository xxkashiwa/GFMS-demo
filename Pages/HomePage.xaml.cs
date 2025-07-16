using GFMS.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
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
            
            // 加载用户信息
            UpdateUserInfo();
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
                    _ => "未知角色"
                };
                
                UserRoleText.Text = role;
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserManager.Instance.Logout();
        }
    }
}

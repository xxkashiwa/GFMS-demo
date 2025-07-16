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
                    _ => "δ֪��ɫ"
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

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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();

            // 在页面加载时预先填充已保存的用户名（如果有）
            LoadSavedCredentials();
        }

        private void LoadSavedCredentials()
        {
            // 从本地设置中读取已保存的用户名
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.TryGetValue("RememberedUsername", out var username))
            {
                UsernameTextBox.Text = username.ToString();
                RememberMeCheckBox.IsChecked = true;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs args)
        {
            // 隐藏之前可能显示的错误消息
            ErrorMessageBorder.Visibility = Visibility.Collapsed;

            // 1. 验证表单
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                ShowError("请输入用户名");
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                ShowError("请输入密码");
                return;
            }

            if (RoleComboBox.SelectedItem == null)
            {
                ShowError("请选择登录角色");
                return;
            }

            // 2. 这里应该进行真实的身份验证
            // 目前简单示例，在实际应用中应该调用API或服务进行验证
            var roleTag = (RoleComboBox.SelectedItem as ComboBoxItem)?.Tag as string;

            // 创建用户对象
            var user = new User
            {
                UserId = UsernameTextBox.Text, // 实际应用中这应该是从服务器获取的ID
                UserName = UsernameTextBox.Text,
                GrantedType = roleTag
            };

            // 3. 处理记住登录选项
            if (RememberMeCheckBox.IsChecked == true)
            {
                UserManager.Instance.SetAuthedState(user);
                // 保存用户名到本地设置
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["RememberedUsername"] = UsernameTextBox.Text;
            }
            else
            {
                UserManager.Instance.ClearAuthedState();
                // 清除保存的用户名
                Windows.Storage.ApplicationData.Current.LocalSettings.Values.Remove("RememberedUsername");
            }

            // 4. 登录用户
            UserManager.Instance.AuthenticateUser(user);
        }

        private void ShowError(string message)
        {
            ErrorMessageText.Text = message;
            ErrorMessageBorder.Visibility = Visibility.Visible;
        }
    }
}

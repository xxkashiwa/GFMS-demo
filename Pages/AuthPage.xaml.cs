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

            // ��ҳ�����ʱԤ������ѱ�����û���������У�
            LoadSavedCredentials();
        }

        private void LoadSavedCredentials()
        {
            // �ӱ��������ж�ȡ�ѱ�����û���
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.TryGetValue("RememberedUsername", out var username))
            {
                UsernameTextBox.Text = username.ToString();
                RememberMeCheckBox.IsChecked = true;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs args)
        {
            // ����֮ǰ������ʾ�Ĵ�����Ϣ
            ErrorMessageBorder.Visibility = Visibility.Collapsed;

            // 1. ��֤��
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                ShowError("�������û���");
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                ShowError("����������");
                return;
            }

            if (RoleComboBox.SelectedItem == null)
            {
                ShowError("��ѡ���¼��ɫ");
                return;
            }

            // 2. ����Ӧ�ý�����ʵ�������֤
            // Ŀǰ��ʾ������ʵ��Ӧ����Ӧ�õ���API����������֤
            var roleTag = (RoleComboBox.SelectedItem as ComboBoxItem)?.Tag as string;

            // �����û�����
            var user = new User
            {
                UserId = UsernameTextBox.Text, // ʵ��Ӧ������Ӧ���Ǵӷ�������ȡ��ID
                UserName = UsernameTextBox.Text,
                GrantedType = roleTag
            };

            // 3. �����ס��¼ѡ��
            if (RememberMeCheckBox.IsChecked == true)
            {
                UserManager.Instance.SetAuthedState(user);
                // �����û�������������
                Windows.Storage.ApplicationData.Current.LocalSettings.Values["RememberedUsername"] = UsernameTextBox.Text;
            }
            else
            {
                UserManager.Instance.ClearAuthedState();
                // ���������û���
                Windows.Storage.ApplicationData.Current.LocalSettings.Values.Remove("RememberedUsername");
            }

            // 4. ��¼�û�
            UserManager.Instance.AuthenticateUser(user);
        }

        private void ShowError(string message)
        {
            ErrorMessageText.Text = message;
            ErrorMessageBorder.Visibility = Visibility.Visible;
        }
    }
}

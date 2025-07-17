using GFMS.Models;
using GFMS.Services;
using GFMS.Views;
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
    /// �����ռ�ҳ�棬����ѧ�������Ϣ¼��͹���
    /// </summary>
    public sealed partial class DataCollectionPage : Page
    {
        public DataCollectionPage()
        {
            InitializeComponent();

            // ����ListView������ԴΪStudentManager������Students����
            StudentsListView.ItemsSource = StudentManager.Instance.Students;

        }

        // ������Ϣ¼�밴ť����¼�
        private async void BasicInfoEntryButton_Click(object sender, RoutedEventArgs e)
        {
            // ��������ʾ���ѧ����Ϣ�Ի���
            var addStudentDialog = new Views.AddStudentDialog
            {
                XamlRoot = this.XamlRoot
            };

            var result = await addStudentDialog.ShowAsync();
            
            // ����û������ȷ����ť��ѧ����Ϣ�Ѿ��ڶԻ�������ӵ�StudentManager
            // ����ListView�󶨵�ObservableCollection��������Զ�����
        }

        // �ɼ���ϸ��ť����¼�
        private void GradesButton_Click(object sender, RoutedEventArgs e)
        {
            // ��ȡ������ѧ������
            if (sender is Button button && button.Tag is Student student)
            {
                // ��������ʾ�ɼ���ϸ��Ϣ�Ӵ���
                var scoreDetailWindow = new ScoresDialogWindow(student);
                scoreDetailWindow.Activate();
            }
        }

        // ������ϸ��ť����¼�
        private void AwardPunishmentButton_Click(object sender, RoutedEventArgs e)
        {
            // ��ȡ������ѧ������
            if (sender is Button button && button.Tag is Student student)
            {
                // ��������ʾ������ϸ��Ϣ�Ӵ���
                var rewardPunishmentWindow = new RewardAndPunishimentDialogWindow(student);
                rewardPunishmentWindow.Activate();
            }
        }

    }
}

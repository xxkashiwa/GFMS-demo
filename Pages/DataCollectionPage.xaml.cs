using GFMS.Models;
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
        // ѧ�����ݼ���
        private ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();

        public DataCollectionPage()
        {
            InitializeComponent();

            // ����ListView������Դ
            StudentsListView.ItemsSource = Students;

            // ��������������ʾ����ʽ���ݻ�ӷ��������أ�
            LoadStudentData();
        }

        // ������Ϣ¼�밴ť����¼�
        private void BasicInfoEntryButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: ��ʾѧ��������Ϣ¼��Ի���
            // �˴�����Ϊ��ʾ��������ʵ�־����߼�
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

        // ����ѧ�����ݵķ�����ʾ�����ݣ�
        private void LoadStudentData()
        {
            // ���һЩʾ��ѧ���������ڲ���
            var student1 = new Student
            {
                StudentId = "2023001",
                Name = "����",
                Gender = "��",
                GraduationDate = new DateTime(2023, 6, 15)
            };

            // ���ʾ���ɼ�����
            student1.Scores.Add("2023�괺��", new List<Score>
            {
                new Score { Subject = "��ѧ", ScoreValue = 88.5 },
                new Score { Subject = "����", ScoreValue = 92.0 }
            });
            student1.Scores.Add("2023���＾", new List<Score>
            {
                new Score { Subject = "Ӣ��", ScoreValue = 89.0 },
                new Score { Subject = "��ѧ", ScoreValue = 91.5 }
            });
            student1.Scores.Add("2022�괺��", new List<Score>
            {
                new Score { Subject = "�����", ScoreValue = 95.0 },
                new Score { Subject = "���ݽṹ", ScoreValue = 87.5 }
            });

            // ���ʾ����������
            student1.RewardsAndPunishments.Add(new RewardAndPunishment
            {
                Type = RecordType.Reward,
                Details = "��ù��ҽ�ѧ��",
                Date = new DateTime(2023, 3, 15)
            });
            student1.RewardsAndPunishments.Add(new RewardAndPunishment
            {
                Type = RecordType.Reward,
                Details = "����ѧ���ɲ�",
                Date = new DateTime(2023, 5, 20)
            });
            student1.RewardsAndPunishments.Add(new RewardAndPunishment
            {
                Type = RecordType.Punishment,
                Details = "�ٵ�����",
                Date = new DateTime(2023, 2, 10)
            });

            var student2 = new Student
            {
                StudentId = "2023002",
                Name = "����",
                Gender = "Ů",
                GraduationDate = new DateTime(2023, 6, 15)
            };

            // ���ʾ���ɼ�����
            student2.Scores.Add("2023�괺��", new List<Score>
            {
                new Score { Subject = "�������ѧ", ScoreValue = 95.0 },
                new Score { Subject = "�������", ScoreValue = 89.5 }
            });
            student2.Scores.Add("2022���＾", new List<Score>
            {
                new Score { Subject = "���ݽṹ", ScoreValue = 92.0 },
                new Score { Subject = "�㷨", ScoreValue = 88.0 }
            });

            // ���ʾ����������
            student2.RewardsAndPunishments.Add(new RewardAndPunishment
            {
                Type = RecordType.Reward,
                Details = "����ѧ��",
                Date = new DateTime(2023, 1, 15)
            });
            student2.RewardsAndPunishments.Add(new RewardAndPunishment
            {
                Type = RecordType.Reward,
                Details = "ѧ������һ�Ƚ�",
                Date = new DateTime(2023, 4, 10)
            });

            Students.Add(student1);
            Students.Add(student2);
        }
    }
}

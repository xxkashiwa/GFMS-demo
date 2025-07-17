using GFMS.Models;
using System;
using System.Collections.Generic;

namespace GFMS.Services
{
    /// <summary>
    /// Ĭ�����ݷ��񣬸���Ϊ���е�������������UserManager�⣩����Ĭ������
    /// </summary>
    public static class DefaultDataService
    {
        /// <summary>
        /// ��ʼ�����й�������Ĭ������
        /// </summary>
        public static void InitializeAllDefaultData()
        {
            InitializeStudentManagerData();
            InitializeFileTransferApplicationManagerData();
        }

        /// <summary>
        /// ��ʼ��ѧ����������Ĭ������
        /// </summary>
        public static void InitializeStudentManagerData()
        {
            var studentManager = StudentManager.Instance;
            
            // �����������
            studentManager.Clear();
            
            // ���Ĭ��ѧ������
            var student1 = new Student
            {
                StudentId = "2023001",
                Name = "����",
                Gender = "��",
                GraduationDate = new DateTime(2027, 6, 30),
                Scores = new Dictionary<string, List<Score>>
                {
                    ["2023����ѧ��"] = new List<Score>
                    {
                        new Score { Subject = "�ߵ���ѧ", ScoreValue = 85.5 },
                        new Score { Subject = "������ƻ���", ScoreValue = 92.0 },
                        new Score { Subject = "Ӣ��", ScoreValue = 78.0 }
                    }
                },
                RewardsAndPunishments = new List<RewardAndPunishment>
                {
                    new RewardAndPunishment
                    {
                        Type = RecordType.Reward,
                        Details = "����ѧ���ɲ�",
                        Date = new DateTime(2023, 12, 15)
                    }
                }
            };

            var student2 = new Student
            {
                StudentId = "2023002",
                Name = "����",
                Gender = "Ů",
                GraduationDate = new DateTime(2027, 6, 30),
                Scores = new Dictionary<string, List<Score>>
                {
                    ["2023����ѧ��"] = new List<Score>
                    {
                        new Score { Subject = "�ߵ���ѧ", ScoreValue = 88.0 },
                        new Score { Subject = "������ƻ���", ScoreValue = 95.5 },
                        new Score { Subject = "Ӣ��", ScoreValue = 82.5 }
                    }
                },
                RewardsAndPunishments = new List<RewardAndPunishment>
                {
                    new RewardAndPunishment
                    {
                        Type = RecordType.Reward,
                        Details = "ѧϰ���㽱",
                        Date = new DateTime(2023, 11, 20)
                    }
                }
            };

            var student3 = new Student
            {
                StudentId = "2023003",
                Name = "����",
                Gender = "��",
                GraduationDate = new DateTime(2027, 6, 30),
                Scores = new Dictionary<string, List<Score>>
                {
                    ["2023����ѧ��"] = new List<Score>
                    {
                        new Score { Subject = "�ߵ���ѧ", ScoreValue = 76.0 },
                        new Score { Subject = "������ƻ���", ScoreValue = 84.5 },
                        new Score { Subject = "Ӣ��", ScoreValue = 73.0 }
                    }
                }
            };

            var student4 = new Student
            {
                StudentId = "2022001",
                Name = "����",
                Gender = "��",
                GraduationDate = new DateTime(2026, 6, 30),
                Scores = new Dictionary<string, List<Score>>
                {
                    ["2022����ѧ��"] = new List<Score>
                    {
                        new Score { Subject = "���ݽṹ", ScoreValue = 91.0 },
                        new Score { Subject = "���������", ScoreValue = 87.5 },
                        new Score { Subject = "����ϵͳ", ScoreValue = 89.0 }
                    }
                },
                Files = new List<StudentFile>
                {
                    new StudentFile
                    {
                        FileType = "��ҵ�ǼǱ�",
                        FilePath = "/files/2022001/graduation_form.pdf",
                        StudentId = "2022001",
                        State = FileState.���ϴ�,
                        UpdatedAt = new DateTime(2024, 1, 15)
                    },
                    new StudentFile
                    {
                        FileType = "����",
                        FilePath = "/files/2022001/medical_exam.pdf",
                        StudentId = "2022001",
                        State = FileState.�����,
                        UpdatedAt = new DateTime(2024, 1, 20)
                    },
                    new StudentFile
                    {
                        FileType = "ʵϰ����",
                        FilePath = "/files/2022001/internship_report.pdf",
                        StudentId = "2022001",
                        State = FileState.����,
                        UpdatedAt = new DateTime(2024, 1, 25)
                    }
                }
            };

            var student5 = new Student
            {
                StudentId = "2022002",
                Name = "Ǯ��",
                Gender = "Ů",
                GraduationDate = new DateTime(2026, 6, 30),
                Scores = new Dictionary<string, List<Score>>
                {
                    ["2022����ѧ��"] = new List<Score>
                    {
                        new Score { Subject = "�˹�����", ScoreValue = 93.5 },
                        new Score { Subject = "����ѧϰ", ScoreValue = 90.0 },
                        new Score { Subject = "ͳ��ѧ", ScoreValue = 86.5 }
                    }
                },
                RewardsAndPunishments = new List<RewardAndPunishment>
                {
                    new RewardAndPunishment
                    {
                        Type = RecordType.Reward,
                        Details = "���ҽ�ѧ��",
                        Date = new DateTime(2023, 10, 1)
                    }
                },
                Files = new List<StudentFile>
                {
                    new StudentFile
                    {
                        FileType = "��ҵ�ǼǱ�",
                        FilePath = "/files/2022002/graduation_form.pdf",
                        StudentId = "2022002",
                        State = FileState.���ϴ�,
                        UpdatedAt = new DateTime(2024, 2, 1)
                    },
                    new StudentFile
                    {
                        FileType = "����",
                        FilePath = "/files/2022002/medical_exam.pdf",
                        StudentId = "2022002",
                        State = FileState.���ϴ�,
                        UpdatedAt = new DateTime(2024, 2, 5)
                    }
                }
            };

            var student6 = new Student
            {
                StudentId = "2022003",
                Name = "���",
                Gender = "��",
                GraduationDate = new DateTime(2026, 6, 30),
                Scores = new Dictionary<string, List<Score>>
                {
                    ["2022����ѧ��"] = new List<Score>
                    {
                        new Score { Subject = "�������", ScoreValue = 88.0 },
                        new Score { Subject = "���ݿ�ϵͳ", ScoreValue = 91.5 },
                        new Score { Subject = "����ϵͳ", ScoreValue = 85.0 }
                    }
                },
                Files = new List<StudentFile>
                {
                    new StudentFile
                    {
                        FileType = "��ҵ�ǼǱ�",
                        FilePath = "/files/2022003/graduation_form.pdf",
                        StudentId = "2022003",
                        State = FileState.���ϴ�,
                        UpdatedAt = new DateTime(2024, 2, 10)
                    },
                    new StudentFile
                    {
                        FileType = "����",
                        FilePath = "/files/2022003/medical_exam.pdf",
                        StudentId = "2022003",
                        State = FileState.���ϴ�,
                        UpdatedAt = new DateTime(2024, 2, 12)
                    },
                    new StudentFile
                    {
                        FileType = "ʵϰ����",
                        FilePath = "/files/2022003/internship_report.pdf",
                        StudentId = "2022003",
                        State = FileState.���ϴ�,
                        UpdatedAt = new DateTime(2024, 2, 15)
                    }
                }
            };

            studentManager.AddStudent(student1);
            studentManager.AddStudent(student2);
            studentManager.AddStudent(student3);
            studentManager.AddStudent(student4);
            studentManager.AddStudent(student5);
            studentManager.AddStudent(student6);
        }

        /// <summary>
        /// ��ʼ������ת�������������Ĭ������
        /// </summary>
        public static void InitializeFileTransferApplicationManagerData()
        {
            var applicationManager = FileTransferApplicationManager.Instance;
            
            // �����������
            applicationManager.Clear();
            
            // ���Ĭ�ϵ���ת����������
            var application1 = new FileTransferApplication
            {
                Id = "2024001001", // ʹ��10λ����ID
                StudentId = "2023001",
                Name = "����",
                Address = "�����г�����ĳĳ�ֵ�100��",
                Telephone = "13800138001",
                Detail = "������Ҫ",
                State = TransferState.ת����,
                CreatedAt = new DateTime(2024, 1, 15)
            };

            var application2 = new FileTransferApplication
            {
                Id = "2024001002", // ʹ��10λ����ID
                StudentId = "2023002", 
                Name = "����",
                Address = "�Ϻ����ֶ�����ĳĳ·200��",
                Telephone = "13800138002",
                Detail = "��������",
                State = TransferState.�����,
                CreatedAt = new DateTime(2024, 1, 10)
            };

            var application3 = new FileTransferApplication
            {
                Id = "2024001003", // ʹ��10λ����ID
                StudentId = "2023003",
                Name = "����",
                Address = "�����������ĳĳ���300��",
                Telephone = "13800138003",
                Detail = "����Ǩ��",
                State = TransferState.����Ԥ����,
                CreatedAt = new DateTime(2024, 2, 1)
            };

            var application4 = new FileTransferApplication
            {
                Id = "2024001004",
                StudentId = "2022001",
                Name = "����",
                Address = "��������ɽ��ĳĳ�Ƽ�԰400��",
                Telephone = "13800138004",
                Detail = "��ְ�¹�˾",
                State = TransferState.����Ԥ����,
                CreatedAt = new DateTime(2024, 2, 5)
            };

            var application5 = new FileTransferApplication
            {
                Id = "2024001005",
                StudentId = "2022002",
                Name = "Ǯ��",
                Address = "������������ĳĳ·500��",
                Telephone = "13800138005",
                Detail = "������Ҫ",
                State = TransferState.ת����,
                CreatedAt = new DateTime(2024, 2, 8)
            };

            applicationManager.AddApplication(application1);
            applicationManager.AddApplication(application2);
            applicationManager.AddApplication(application3);
            applicationManager.AddApplication(application4);
            applicationManager.AddApplication(application5);
        }
    }
}
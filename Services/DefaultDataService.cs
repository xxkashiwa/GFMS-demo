using GFMS.Models;
using System;
using System.Collections.Generic;

namespace GFMS.Services
{
    /// <summary>
    /// 默认数据服务，负责为所有单例管理器（除UserManager外）加载默认数据
    /// </summary>
    public static class DefaultDataService
    {
        /// <summary>
        /// 初始化所有管理器的默认数据
        /// </summary>
        public static void InitializeAllDefaultData()
        {
            InitializeStudentManagerData();
            InitializeFileTransferApplicationManagerData();
        }

        /// <summary>
        /// 初始化学生管理器的默认数据
        /// </summary>
        public static void InitializeStudentManagerData()
        {
            var studentManager = StudentManager.Instance;
            
            // 清空现有数据
            studentManager.Clear();
            
            // 添加默认学生数据
            var student1 = new Student
            {
                StudentId = "2023001",
                Name = "张三",
                Gender = "男",
                GraduationDate = new DateTime(2027, 6, 30),
                Scores = new Dictionary<string, List<Score>>
                {
                    ["2023春季学期"] = new List<Score>
                    {
                        new Score { Subject = "高等数学", ScoreValue = 85.5 },
                        new Score { Subject = "程序设计基础", ScoreValue = 92.0 },
                        new Score { Subject = "英语", ScoreValue = 78.0 }
                    }
                },
                RewardsAndPunishments = new List<RewardAndPunishment>
                {
                    new RewardAndPunishment
                    {
                        Type = RecordType.Reward,
                        Details = "优秀学生干部",
                        Date = new DateTime(2023, 12, 15)
                    }
                }
            };

            var student2 = new Student
            {
                StudentId = "2023002",
                Name = "李四",
                Gender = "女",
                GraduationDate = new DateTime(2027, 6, 30),
                Scores = new Dictionary<string, List<Score>>
                {
                    ["2023春季学期"] = new List<Score>
                    {
                        new Score { Subject = "高等数学", ScoreValue = 88.0 },
                        new Score { Subject = "程序设计基础", ScoreValue = 95.5 },
                        new Score { Subject = "英语", ScoreValue = 82.5 }
                    }
                },
                RewardsAndPunishments = new List<RewardAndPunishment>
                {
                    new RewardAndPunishment
                    {
                        Type = RecordType.Reward,
                        Details = "学习优秀奖",
                        Date = new DateTime(2023, 11, 20)
                    }
                }
            };

            var student3 = new Student
            {
                StudentId = "2023003",
                Name = "王五",
                Gender = "男",
                GraduationDate = new DateTime(2027, 6, 30),
                Scores = new Dictionary<string, List<Score>>
                {
                    ["2023春季学期"] = new List<Score>
                    {
                        new Score { Subject = "高等数学", ScoreValue = 76.0 },
                        new Score { Subject = "程序设计基础", ScoreValue = 84.5 },
                        new Score { Subject = "英语", ScoreValue = 73.0 }
                    }
                }
            };

            var student4 = new Student
            {
                StudentId = "2022001",
                Name = "王五",
                Gender = "男",
                GraduationDate = new DateTime(2026, 6, 30),
                Scores = new Dictionary<string, List<Score>>
                {
                    ["2022春季学期"] = new List<Score>
                    {
                        new Score { Subject = "数据结构", ScoreValue = 91.0 },
                        new Score { Subject = "计算机网络", ScoreValue = 87.5 },
                        new Score { Subject = "操作系统", ScoreValue = 89.0 }
                    }
                },
                Files = new List<StudentFile>
                {
                    new StudentFile
                    {
                        FileType = "毕业登记表",
                        FilePath = "/files/2022001/graduation_form.pdf",
                        StudentId = "2022001",
                        State = FileState.已上传,
                        UpdatedAt = new DateTime(2024, 1, 15)
                    },
                    new StudentFile
                    {
                        FileType = "体检表",
                        FilePath = "/files/2022001/medical_exam.pdf",
                        StudentId = "2022001",
                        State = FileState.已审核,
                        UpdatedAt = new DateTime(2024, 1, 20)
                    },
                    new StudentFile
                    {
                        FileType = "实习报告",
                        FilePath = "/files/2022001/internship_report.pdf",
                        StudentId = "2022001",
                        State = FileState.驳回,
                        UpdatedAt = new DateTime(2024, 1, 25)
                    }
                }
            };

            var student5 = new Student
            {
                StudentId = "2022002",
                Name = "钱七",
                Gender = "女",
                GraduationDate = new DateTime(2026, 6, 30),
                Scores = new Dictionary<string, List<Score>>
                {
                    ["2022春季学期"] = new List<Score>
                    {
                        new Score { Subject = "人工智能", ScoreValue = 93.5 },
                        new Score { Subject = "机器学习", ScoreValue = 90.0 },
                        new Score { Subject = "统计学", ScoreValue = 86.5 }
                    }
                },
                RewardsAndPunishments = new List<RewardAndPunishment>
                {
                    new RewardAndPunishment
                    {
                        Type = RecordType.Reward,
                        Details = "国家奖学金",
                        Date = new DateTime(2023, 10, 1)
                    }
                },
                Files = new List<StudentFile>
                {
                    new StudentFile
                    {
                        FileType = "毕业登记表",
                        FilePath = "/files/2022002/graduation_form.pdf",
                        StudentId = "2022002",
                        State = FileState.已上传,
                        UpdatedAt = new DateTime(2024, 2, 1)
                    },
                    new StudentFile
                    {
                        FileType = "体检表",
                        FilePath = "/files/2022002/medical_exam.pdf",
                        StudentId = "2022002",
                        State = FileState.已上传,
                        UpdatedAt = new DateTime(2024, 2, 5)
                    }
                }
            };

            var student6 = new Student
            {
                StudentId = "2022003",
                Name = "孙八",
                Gender = "男",
                GraduationDate = new DateTime(2026, 6, 30),
                Scores = new Dictionary<string, List<Score>>
                {
                    ["2022春季学期"] = new List<Score>
                    {
                        new Score { Subject = "软件工程", ScoreValue = 88.0 },
                        new Score { Subject = "数据库系统", ScoreValue = 91.5 },
                        new Score { Subject = "操作系统", ScoreValue = 85.0 }
                    }
                },
                Files = new List<StudentFile>
                {
                    new StudentFile
                    {
                        FileType = "毕业登记表",
                        FilePath = "/files/2022003/graduation_form.pdf",
                        StudentId = "2022003",
                        State = FileState.已上传,
                        UpdatedAt = new DateTime(2024, 2, 10)
                    },
                    new StudentFile
                    {
                        FileType = "体检表",
                        FilePath = "/files/2022003/medical_exam.pdf",
                        StudentId = "2022003",
                        State = FileState.已上传,
                        UpdatedAt = new DateTime(2024, 2, 12)
                    },
                    new StudentFile
                    {
                        FileType = "实习报告",
                        FilePath = "/files/2022003/internship_report.pdf",
                        StudentId = "2022003",
                        State = FileState.已上传,
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
        /// 初始化档案转递申请管理器的默认数据
        /// </summary>
        public static void InitializeFileTransferApplicationManagerData()
        {
            var applicationManager = FileTransferApplicationManager.Instance;
            
            // 清空现有数据
            applicationManager.Clear();
            
            // 添加默认档案转递申请数据
            var application1 = new FileTransferApplication
            {
                Id = "2024001001", // 使用10位数字ID
                StudentId = "2023001",
                Name = "张三",
                Address = "北京市朝阳区某某街道100号",
                Telephone = "13800138001",
                Detail = "工作需要",
                State = TransferState.转递中,
                CreatedAt = new DateTime(2024, 1, 15)
            };

            var application2 = new FileTransferApplication
            {
                Id = "2024001002", // 使用10位数字ID
                StudentId = "2023002", 
                Name = "李四",
                Address = "上海市浦东新区某某路200号",
                Telephone = "13800138002",
                Detail = "继续深造",
                State = TransferState.已完成,
                CreatedAt = new DateTime(2024, 1, 10)
            };

            var application3 = new FileTransferApplication
            {
                Id = "2024001003", // 使用10位数字ID
                StudentId = "2023003",
                Name = "王五",
                Address = "广州市天河区某某大道300号",
                Telephone = "13800138003",
                Detail = "户口迁移",
                State = TransferState.档案预备中,
                CreatedAt = new DateTime(2024, 2, 1)
            };

            var application4 = new FileTransferApplication
            {
                Id = "2024001004",
                StudentId = "2022001",
                Name = "赵六",
                Address = "深圳市南山区某某科技园400号",
                Telephone = "13800138004",
                Detail = "入职新公司",
                State = TransferState.档案预备中,
                CreatedAt = new DateTime(2024, 2, 5)
            };

            var application5 = new FileTransferApplication
            {
                Id = "2024001005",
                StudentId = "2022002",
                Name = "钱七",
                Address = "杭州市西湖区某某路500号",
                Telephone = "13800138005",
                Detail = "考研需要",
                State = TransferState.转递中,
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
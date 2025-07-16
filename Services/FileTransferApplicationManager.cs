using GFMS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Services
{
    /// <summary>
    /// 档案转递申请管理器，单例模式
    /// </summary>
    public sealed class FileTransferApplicationManager
    {
        private static FileTransferApplicationManager? _instance;
        public static FileTransferApplicationManager Instance => _instance ??= new FileTransferApplicationManager();

        private FileTransferApplicationManager()
        {
            Applications = new ObservableCollection<FileTransferApplication>();
            LoadDefaultData();
        }

        /// <summary>
        /// 档案转递申请集合
        /// </summary>
        public ObservableCollection<FileTransferApplication> Applications { get; private set; }

        /// <summary>
        /// 添加档案转递申请
        /// </summary>
        /// <param name="application">要添加的申请</param>
        public void AddApplication(FileTransferApplication application)
        {
            if (application != null)
            {
                Applications.Add(application);
            }
        }

        /// <summary>
        /// 根据ID获取档案转递申请
        /// </summary>
        /// <param name="id">申请ID</param>
        /// <returns>找到的申请，如果没找到则返回null</returns>
        public FileTransferApplication? GetApplicationById(string id)
        {
            return Applications.FirstOrDefault(a => a.Id == id);
        }

        /// <summary>
        /// 根据学号获取学生的档案转递申请
        /// </summary>
        /// <param name="studentId">学号</param>
        /// <returns>该学生的所有申请</returns>
        public List<FileTransferApplication> GetApplicationsByStudentId(string studentId)
        {
            return Applications.Where(a => a.StudentId == studentId).ToList();
        }

        /// <summary>
        /// 更新申请状态
        /// </summary>
        /// <param name="id">申请ID</param>
        /// <param name="newState">新状态</param>
        public void UpdateApplicationState(string id, TransferState newState)
        {
            var application = GetApplicationById(id);
            if (application != null)
            {
                application.State = newState;
            }
        }

        /// <summary>
        /// 删除档案转递申请
        /// </summary>
        /// <param name="id">要删除的申请ID</param>
        public void RemoveApplication(string id)
        {
            var application = GetApplicationById(id);
            if (application != null)
            {
                Applications.Remove(application);
            }
        }

        /// <summary>
        /// 清空所有申请
        /// </summary>
        public void Clear()
        {
            Applications.Clear();
        }

        /// <summary>
        /// 加载默认数据
        /// </summary>
        private void LoadDefaultData()
        {
            // 添加一些默认数据用于演示
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

            Applications.Add(application1);
            Applications.Add(application2);
            Applications.Add(application3);
        }
    }
}
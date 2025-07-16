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
    }
}
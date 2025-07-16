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
    /// ����ת�����������������ģʽ
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
        /// ����ת�����뼯��
        /// </summary>
        public ObservableCollection<FileTransferApplication> Applications { get; private set; }

        /// <summary>
        /// ��ӵ���ת������
        /// </summary>
        /// <param name="application">Ҫ��ӵ�����</param>
        public void AddApplication(FileTransferApplication application)
        {
            if (application != null)
            {
                Applications.Add(application);
            }
        }

        /// <summary>
        /// ����ID��ȡ����ת������
        /// </summary>
        /// <param name="id">����ID</param>
        /// <returns>�ҵ������룬���û�ҵ��򷵻�null</returns>
        public FileTransferApplication? GetApplicationById(string id)
        {
            return Applications.FirstOrDefault(a => a.Id == id);
        }

        /// <summary>
        /// ����ѧ�Ż�ȡѧ���ĵ���ת������
        /// </summary>
        /// <param name="studentId">ѧ��</param>
        /// <returns>��ѧ������������</returns>
        public List<FileTransferApplication> GetApplicationsByStudentId(string studentId)
        {
            return Applications.Where(a => a.StudentId == studentId).ToList();
        }

        /// <summary>
        /// ��������״̬
        /// </summary>
        /// <param name="id">����ID</param>
        /// <param name="newState">��״̬</param>
        public void UpdateApplicationState(string id, TransferState newState)
        {
            var application = GetApplicationById(id);
            if (application != null)
            {
                application.State = newState;
            }
        }

        /// <summary>
        /// ɾ������ת������
        /// </summary>
        /// <param name="id">Ҫɾ��������ID</param>
        public void RemoveApplication(string id)
        {
            var application = GetApplicationById(id);
            if (application != null)
            {
                Applications.Remove(application);
            }
        }

        /// <summary>
        /// �����������
        /// </summary>
        public void Clear()
        {
            Applications.Clear();
        }
    }
}
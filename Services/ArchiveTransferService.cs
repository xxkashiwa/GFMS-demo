using GFMS.Models;
using GFMS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GFMS.Services
{
    /// <summary>
    /// ����ת�ݷ�����
    /// </summary>
    public class ArchiveTransferService
    {
        private static ArchiveTransferService? _instance;
        public static ArchiveTransferService Instance => _instance ??= new ArchiveTransferService();

        // ģ�����ݴ洢
        private readonly List<ArchiveRequest> _archiveRequests = new();
        private readonly List<Student> _students = new();
        private int _nextRequestId = 1;
        private int _nextLetterNumber = 1;

        private ArchiveTransferService()
        {
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            // ��ʼ��ʾ��ѧ������
            _students.AddRange(new[]
            {
                new Student { Id = 1, StudentId = "2021001", Name = "����", Major = "�������ѧ�뼼��", GraduationYear = 2024 },
                new Student { Id = 2, StudentId = "2021002", Name = "����", Major = "�������", GraduationYear = 2024 },
                new Student { Id = 3, StudentId = "2021003", Name = "����", Major = "���繤��", GraduationYear = 2024 },
                new Student { Id = 4, StudentId = "2021004", Name = "����", Major = "��Ϣ��ȫ", GraduationYear = 2024 }
            });

            // ��ʼ��ʾ��������������
            _archiveRequests.AddRange(new[]
            {
                new ArchiveRequest
                {
                    Id = _nextRequestId++,
                    StudentId = 1,
                    ReceiverName = "��Ϊ�������޹�˾",
                    ReceiverAddress = "�㶫ʡ��������������Ϊ�ܲ�",
                    RequestDate = DateTime.Now.AddDays(-2),
                    Status = "�����",
                    Student = _students[0]
                },
                new ArchiveRequest
                {
                    Id = _nextRequestId++,
                    StudentId = 2,
                    ReceiverName = "��Ѷ�Ƽ����޹�˾",
                    ReceiverAddress = "�㶫ʡ��������ɽ���Ƽ�԰",
                    RequestDate = DateTime.Now.AddDays(-1),
                    Status = "��ͨ��",
                    Student = _students[1]
                }
            });
        }

        #region ����������ط���

        /// <summary>
        /// ��ȡ��ǰ�û��ĵ��������б�ѧ��ֻ�ܿ��Լ��ģ�
        /// </summary>
        public List<ArchiveRequest> GetArchiveRequests(string? studentId = null)
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser == null) return new List<ArchiveRequest>();

            // ѧ��ֻ�ܲ鿴�Լ�������
            if (currentUser.Role == "Student")
            {
                var student = _students.FirstOrDefault(s => s.StudentId == currentUser.Username);
                if (student != null)
                {
                    return _archiveRequests.Where(r => r.StudentId == student.Id).ToList();
                }
                return new List<ArchiveRequest>();
            }

            // ����Ա����ʦ���Բ鿴��������
            if (!string.IsNullOrEmpty(studentId))
            {
                var student = _students.FirstOrDefault(s => s.StudentId == studentId);
                if (student != null)
                {
                    return _archiveRequests.Where(r => r.StudentId == student.Id).ToList();
                }
            }

            return _archiveRequests.ToList();
        }

        /// <summary>
        /// ����״̬ɸѡ����
        /// </summary>
        public List<ArchiveRequest> GetArchiveRequestsByStatus(string status)
        {
            var requests = GetArchiveRequests();
            if (status == "ȫ��") return requests;
            
            return requests.Where(r => r.Status == status).ToList();
        }

        /// <summary>
        /// �����µĵ������루��ѧ�����ã�
        /// </summary>
        public async Task<bool> CreateArchiveRequestAsync(ArchiveRequest request)
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser?.Role != "Student") return false;

            var student = _students.FirstOrDefault(s => s.StudentId == currentUser.Username);
            if (student == null) return false;

            request.Id = _nextRequestId++;
            request.StudentId = student.Id;
            request.Student = student;
            request.RequestDate = DateTime.Now;
            request.Status = "�����";

            _archiveRequests.Add(request);
            return true;
        }

        /// <summary>
        /// ������루������Ա����ʦ���ã�
        /// </summary>
        public async Task<bool> ReviewArchiveRequestAsync(int requestId, string status)
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser?.Role == "Student") return false;

            var request = _archiveRequests.FirstOrDefault(r => r.Id == requestId);
            if (request == null) return false;

            request.Status = status;

            return true;
        }

        #endregion

        #region ����ת����ط���

        /// <summary>
        /// ���ɵ���ת�ݺ�������Ա���ã�
        /// </summary>
        public async Task<string> GenerateTransferLetterAsync(int requestId, string remarks = "")
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser?.Role != "Admin") return "";

            var request = _archiveRequests.FirstOrDefault(r => r.Id == requestId);
            if (request?.Status != "��ͨ��") return "";

            var letterNumber = $"DF{DateTime.Now.Year}{_nextLetterNumber++:D4}";
            
            // ����Ӧ������ʵ�ʵĵ���ת�ݺ��ĵ�
            // ���ڷ��ص����������Ϊʾ��
            return letterNumber;
        }

        #endregion

        #region ת�ݼ�¼��ط���

        /// <summary>
        /// ��¼�����ĳ���Ϣ������Ա���ã�
        /// </summary>
        public async Task<bool> RecordDispatchAsync(int requestId, string trackingNumber)
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser?.Role != "Admin") return false;

            var request = _archiveRequests.FirstOrDefault(r => r.Id == requestId);
            if (request == null) return false;

            request.TrackingNumber = trackingNumber;
            request.DispatchDate = DateTime.Now;
            request.Status = "�Ѽĳ�";

            return true;
        }

        /// <summary>
        /// ����ǩ��״̬
        /// </summary>
        public async Task<bool> UpdateReceiveStatusAsync(int requestId, DateTime receiveDate)
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser?.Role == "Student") return false;

            var request = _archiveRequests.FirstOrDefault(r => r.Id == requestId);
            if (request == null) return false;

            request.ReceiveDate = receiveDate;
            request.Status = "��ǩ��";

            return true;
        }

        /// <summary>
        /// ��ȡת�ݼ�¼�б�
        /// </summary>
        public List<ArchiveRequest> GetTransferRecords()
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser == null) return new List<ArchiveRequest>();

            // ѧ��ֻ�ܲ鿴�Լ���ת�ݼ�¼
            if (currentUser.Role == "Student")
            {
                var student = _students.FirstOrDefault(s => s.StudentId == currentUser.Username);
                if (student != null)
                {
                    return _archiveRequests.Where(r => r.StudentId == student.Id && 
                        (r.Status == "�Ѽĳ�" || r.Status == "��ǩ��")).ToList();
                }
                return new List<ArchiveRequest>();
            }

            // ����Ա����ʦ���Բ鿴����ת�ݼ�¼
            return _archiveRequests.Where(r => r.Status == "�Ѽĳ�" || r.Status == "��ǩ��").ToList();
        }

        #endregion

        #region Ȩ����֤���

        /// <summary>
        /// ����Ƿ��д��������Ȩ��
        /// </summary>
        public bool CanCreateRequest()
        {
            var currentUser = UserManager.Instance.AuthedUser;
            return currentUser?.Role == "Student";
        }

        /// <summary>
        /// ����Ƿ�����������Ȩ��
        /// </summary>
        public bool CanReviewRequest()
        {
            var currentUser = UserManager.Instance.AuthedUser;
            return currentUser?.Role == "Admin" || currentUser?.Role == "Teacher";
        }

        /// <summary>
        /// ����Ƿ��й���ת�ݼ�¼��Ȩ��
        /// </summary>
        public bool CanManageTransfer()
        {
            var currentUser = UserManager.Instance.AuthedUser;
            return currentUser?.Role == "Admin";
        }

        /// <summary>
        /// ����Ƿ���Բ鿴ָ��ѧ��������Ϣ
        /// </summary>
        public bool CanViewStudentInfo(string studentId)
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser == null) return false;

            // ѧ��ֻ�ܲ鿴�Լ�����Ϣ
            if (currentUser.Role == "Student")
            {
                return currentUser.Username == studentId;
            }

            // ����Ա����ʦ���Բ鿴����ѧ����Ϣ
            return currentUser.Role == "Admin" || currentUser.Role == "Teacher";
        }

        #endregion

        #region ��������

        /// <summary>
        /// ��ȡѧ���б�
        /// </summary>
        public List<Student> GetStudents()
        {
            return _students.ToList();
        }

        /// <summary>
        /// ����ѧ�Ż�ȡѧ����Ϣ
        /// </summary>
        public Student? GetStudentByStudentId(string studentId)
        {
            return _students.FirstOrDefault(s => s.StudentId == studentId);
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        public ArchiveRequest? GetRequestById(int requestId)
        {
            var request = _archiveRequests.FirstOrDefault(r => r.Id == requestId);
            if (request?.Student == null && request != null)
            {
                request.Student = _students.FirstOrDefault(s => s.Id == request.StudentId);
            }
            return request;
        }

        #endregion
    }
}
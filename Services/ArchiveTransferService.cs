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
    /// 档案转递服务类
    /// </summary>
    public class ArchiveTransferService
    {
        private static ArchiveTransferService? _instance;
        public static ArchiveTransferService Instance => _instance ??= new ArchiveTransferService();

        // 模拟数据存储
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
            // 初始化示例学生数据
            _students.AddRange(new[]
            {
                new Student { Id = 1, StudentId = "2021001", Name = "张三", Major = "计算机科学与技术", GraduationYear = 2024 },
                new Student { Id = 2, StudentId = "2021002", Name = "李四", Major = "软件工程", GraduationYear = 2024 },
                new Student { Id = 3, StudentId = "2021003", Name = "王五", Major = "网络工程", GraduationYear = 2024 },
                new Student { Id = 4, StudentId = "2021004", Name = "赵六", Major = "信息安全", GraduationYear = 2024 }
            });

            // 初始化示例调档申请数据
            _archiveRequests.AddRange(new[]
            {
                new ArchiveRequest
                {
                    Id = _nextRequestId++,
                    StudentId = 1,
                    ReceiverName = "华为技术有限公司",
                    ReceiverAddress = "广东省深圳市龙岗区华为总部",
                    RequestDate = DateTime.Now.AddDays(-2),
                    Status = "待审核",
                    Student = _students[0]
                },
                new ArchiveRequest
                {
                    Id = _nextRequestId++,
                    StudentId = 2,
                    ReceiverName = "腾讯科技有限公司",
                    ReceiverAddress = "广东省深圳市南山区科技园",
                    RequestDate = DateTime.Now.AddDays(-1),
                    Status = "已通过",
                    Student = _students[1]
                }
            });
        }

        #region 调档申请相关方法

        /// <summary>
        /// 获取当前用户的调档申请列表（学生只能看自己的）
        /// </summary>
        public List<ArchiveRequest> GetArchiveRequests(string? studentId = null)
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser == null) return new List<ArchiveRequest>();

            // 学生只能查看自己的申请
            if (currentUser.Role == "Student")
            {
                var student = _students.FirstOrDefault(s => s.StudentId == currentUser.Username);
                if (student != null)
                {
                    return _archiveRequests.Where(r => r.StudentId == student.Id).ToList();
                }
                return new List<ArchiveRequest>();
            }

            // 管理员和老师可以查看所有申请
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
        /// 按照状态筛选申请
        /// </summary>
        public List<ArchiveRequest> GetArchiveRequestsByStatus(string status)
        {
            var requests = GetArchiveRequests();
            if (status == "全部") return requests;
            
            return requests.Where(r => r.Status == status).ToList();
        }

        /// <summary>
        /// 创建新的调档申请（供学生调用）
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
            request.Status = "待审核";

            _archiveRequests.Add(request);
            return true;
        }

        /// <summary>
        /// 审核申请（供管理员或老师调用）
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

        #region 档案转递相关方法

        /// <summary>
        /// 生成调档转递函（管理员调用）
        /// </summary>
        public async Task<string> GenerateTransferLetterAsync(int requestId, string remarks = "")
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser?.Role != "Admin") return "";

            var request = _archiveRequests.FirstOrDefault(r => r.Id == requestId);
            if (request?.Status != "已通过") return "";

            var letterNumber = $"DF{DateTime.Now.Year}{_nextLetterNumber++:D4}";
            
            // 这里应该生成实际的调档转递函文档
            // 现在返回调档函编号作为示例
            return letterNumber;
        }

        #endregion

        #region 转递记录相关方法

        /// <summary>
        /// 记录档案寄出信息（管理员调用）
        /// </summary>
        public async Task<bool> RecordDispatchAsync(int requestId, string trackingNumber)
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser?.Role != "Admin") return false;

            var request = _archiveRequests.FirstOrDefault(r => r.Id == requestId);
            if (request == null) return false;

            request.TrackingNumber = trackingNumber;
            request.DispatchDate = DateTime.Now;
            request.Status = "已寄出";

            return true;
        }

        /// <summary>
        /// 更新签收状态
        /// </summary>
        public async Task<bool> UpdateReceiveStatusAsync(int requestId, DateTime receiveDate)
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser?.Role == "Student") return false;

            var request = _archiveRequests.FirstOrDefault(r => r.Id == requestId);
            if (request == null) return false;

            request.ReceiveDate = receiveDate;
            request.Status = "已签收";

            return true;
        }

        /// <summary>
        /// 获取转递记录列表
        /// </summary>
        public List<ArchiveRequest> GetTransferRecords()
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser == null) return new List<ArchiveRequest>();

            // 学生只能查看自己的转递记录
            if (currentUser.Role == "Student")
            {
                var student = _students.FirstOrDefault(s => s.StudentId == currentUser.Username);
                if (student != null)
                {
                    return _archiveRequests.Where(r => r.StudentId == student.Id && 
                        (r.Status == "已寄出" || r.Status == "已签收")).ToList();
                }
                return new List<ArchiveRequest>();
            }

            // 管理员和老师可以查看所有转递记录
            return _archiveRequests.Where(r => r.Status == "已寄出" || r.Status == "已签收").ToList();
        }

        #endregion

        #region 权限验证相关

        /// <summary>
        /// 检查是否有创建申请的权限
        /// </summary>
        public bool CanCreateRequest()
        {
            var currentUser = UserManager.Instance.AuthedUser;
            return currentUser?.Role == "Student";
        }

        /// <summary>
        /// 检查是否有审核申请的权限
        /// </summary>
        public bool CanReviewRequest()
        {
            var currentUser = UserManager.Instance.AuthedUser;
            return currentUser?.Role == "Admin" || currentUser?.Role == "Teacher";
        }

        /// <summary>
        /// 检查是否有管理转递记录的权限
        /// </summary>
        public bool CanManageTransfer()
        {
            var currentUser = UserManager.Instance.AuthedUser;
            return currentUser?.Role == "Admin";
        }

        /// <summary>
        /// 检查是否可以查看指定学生档案信息
        /// </summary>
        public bool CanViewStudentInfo(string studentId)
        {
            var currentUser = UserManager.Instance.AuthedUser;
            if (currentUser == null) return false;

            // 学生只能查看自己的信息
            if (currentUser.Role == "Student")
            {
                return currentUser.Username == studentId;
            }

            // 管理员和老师可以查看所有学生信息
            return currentUser.Role == "Admin" || currentUser.Role == "Teacher";
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取学生列表
        /// </summary>
        public List<Student> GetStudents()
        {
            return _students.ToList();
        }

        /// <summary>
        /// 根据学号获取学生信息
        /// </summary>
        public Student? GetStudentByStudentId(string studentId)
        {
            return _students.FirstOrDefault(s => s.StudentId == studentId);
        }

        /// <summary>
        /// 获取申请详情
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
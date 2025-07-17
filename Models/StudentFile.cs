using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    /// <summary>
    /// 学生档案文件模型，用于联系学生和档案文件
    /// </summary>
    public class StudentFile
    {
        /// <summary>
        /// 文件类型，例如毕业登记表/体检表/实习报告
        /// </summary>
        public string FileType { get; set; } = string.Empty;

        /// <summary>
        /// 文件路径，用于存储文件的路径
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 关联的学生ID
        /// </summary>
        public string StudentId { get; set; } = string.Empty;

        /// <summary>
        /// 档案状态
        /// </summary>
        public FileState State { get; set; } = FileState.已上传;
    }

    /// <summary>
    /// 档案文件状态枚举
    /// </summary>
    public enum FileState
    {
        /// <summary>
        /// 已上传
        /// </summary>
        已上传,

        /// <summary>
        /// 已审核
        /// </summary>
        已审核,

        /// <summary>
        /// 驳回
        /// </summary>
        驳回
    }
}

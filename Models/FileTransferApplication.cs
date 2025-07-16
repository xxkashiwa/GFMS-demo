using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    /// <summary>
    /// 档案转递申请模型
    /// </summary>
    public class FileTransferApplication
    {
        /// <summary>
        /// 调档申请ID，自动生成10位数字
        /// </summary>
        public string Id { get; set; } = GenerateShortId();

        /// <summary>
        /// 学号
        /// </summary>
        public string StudentId { get; set; } = string.Empty;

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 档案接受地址
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 联系电话号码
        /// </summary>
        public string Telephone { get; set; } = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string Detail { get; set; } = string.Empty;

        /// <summary>
        /// 申请表的建立时间
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// 转递状态，默认为档案预备中
        /// </summary>
        public TransferState State { get; set; } = TransferState.档案预备中;

        /// <summary>
        /// 生成10位随机数字ID
        /// </summary>
        /// <returns>10位数字字符串</returns>
        private static string GenerateShortId()
        {
            var random = new Random();
            var id = "";
            for (int i = 0; i < 10; i++)
            {
                id += random.Next(0, 10).ToString();
            }
            return id;
        }
    }

    /// <summary>
    /// 转递状态枚举
    /// </summary>
    public enum TransferState
    {
        /// <summary>
        /// 档案预备中
        /// </summary>
        档案预备中,

        /// <summary>
        /// 转递中
        /// </summary>
        转递中,

        /// <summary>
        /// 已完成
        /// </summary>
        已完成
    }
}
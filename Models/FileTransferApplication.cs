using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFMS.Models
{
    /// <summary>
    /// ����ת������ģ��
    /// </summary>
    public class FileTransferApplication
    {
        /// <summary>
        /// ��������ID���Զ�����10λ����
        /// </summary>
        public string Id { get; set; } = GenerateShortId();

        /// <summary>
        /// ѧ��
        /// </summary>
        public string StudentId { get; set; } = string.Empty;

        /// <summary>
        /// ����
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// �������ܵ�ַ
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// ��ϵ�绰����
        /// </summary>
        public string Telephone { get; set; } = string.Empty;

        /// <summary>
        /// ��ע
        /// </summary>
        public string Detail { get; set; } = string.Empty;

        /// <summary>
        /// �����Ľ���ʱ��
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// ת��״̬��Ĭ��Ϊ����Ԥ����
        /// </summary>
        public TransferState State { get; set; } = TransferState.����Ԥ����;

        /// <summary>
        /// ����10λ�������ID
        /// </summary>
        /// <returns>10λ�����ַ���</returns>
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
    /// ת��״̬ö��
    /// </summary>
    public enum TransferState
    {
        /// <summary>
        /// ����Ԥ����
        /// </summary>
        ����Ԥ����,

        /// <summary>
        /// ת����
        /// </summary>
        ת����,

        /// <summary>
        /// �����
        /// </summary>
        �����
    }
}
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
    /// ѧ�����ݹ�����������������Ӧ�ó����й���ѧ������
    /// </summary>
    public sealed class StudentManager
    {
        private static StudentManager? _instance;
        public static StudentManager Instance => _instance ??= new StudentManager();

        private StudentManager()
        {
            Students = new ObservableCollection<Student>();
        }

        /// <summary>
        /// ѧ�����ݼ���
        /// </summary>
        public ObservableCollection<Student> Students { get; private set; }

        /// <summary>
        /// ���ѧ��
        /// </summary>
        /// <param name="student">Ҫ��ӵ�ѧ��</param>
        public void AddStudent(Student student)
        {
            if (student != null && !Students.Any(s => s.StudentId == student.StudentId))
            {
                Students.Add(student);
            }
        }

        /// <summary>
        /// ����ѧ��ID��ȡѧ��
        /// </summary>
        /// <param name="studentId">ѧ��ID</param>
        /// <returns>�ҵ���ѧ�������û�ҵ��򷵻�null</returns>
        public Student? GetStudentById(string studentId)
        {
            return Students.FirstOrDefault(s => s.StudentId == studentId);
        }

        /// <summary>
        /// ����ѧ����Ϣ
        /// </summary>
        /// <param name="student">Ҫ���µ�ѧ��</param>
        public void UpdateStudent(Student student)
        {
            var existingStudent = GetStudentById(student.StudentId);
            if (existingStudent != null)
            {
                var index = Students.IndexOf(existingStudent);
                Students[index] = student;
            }
        }

        /// <summary>
        /// ɾ��ѧ��
        /// </summary>
        /// <param name="studentId">Ҫɾ����ѧ��ID</param>
        public void RemoveStudent(string studentId)
        {
            var student = GetStudentById(studentId);
            if (student != null)
            {
                Students.Remove(student);
            }
        }

        /// <summary>
        /// �������ѧ������
        /// </summary>
        public void Clear()
        {
            Students.Clear();
        }
    }
}
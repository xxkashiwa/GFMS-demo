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
    /// 学生数据管理单例，用于在整个应用程序中管理学生数据
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
        /// 学生数据集合
        /// </summary>
        public ObservableCollection<Student> Students { get; private set; }

        /// <summary>
        /// 添加学生
        /// </summary>
        /// <param name="student">要添加的学生</param>
        public void AddStudent(Student student)
        {
            if (student != null && !Students.Any(s => s.StudentId == student.StudentId))
            {
                Students.Add(student);
            }
        }

        /// <summary>
        /// 根据学生ID获取学生
        /// </summary>
        /// <param name="studentId">学生ID</param>
        /// <returns>找到的学生，如果没找到则返回null</returns>
        public Student? GetStudentById(string studentId)
        {
            return Students.FirstOrDefault(s => s.StudentId == studentId);
        }

        /// <summary>
        /// 更新学生信息
        /// </summary>
        /// <param name="student">要更新的学生</param>
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
        /// 删除学生
        /// </summary>
        /// <param name="studentId">要删除的学生ID</param>
        public void RemoveStudent(string studentId)
        {
            var student = GetStudentById(studentId);
            if (student != null)
            {
                Students.Remove(student);
            }
        }

        /// <summary>
        /// 清空所有学生数据
        /// </summary>
        public void Clear()
        {
            Students.Clear();
        }
    }
}
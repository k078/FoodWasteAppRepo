using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainServices.Interfaces
{
    public interface IStudentRepo
    {
        IEnumerable<Student> GetStudents();
        Student GetStudentById(int id);
        Student GetStudentByEmail(string email);
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(int id);
    }
}

using Core.Domain;
using Core.DomainServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class StudentEFRepo : IStudentRepo
    {
        private readonly FoodWasteDbContext _context;
        public StudentEFRepo(FoodWasteDbContext context)
        {
            _context = context;
        }
        public void AddStudent(Student student)
        {
            throw new NotImplementedException();
        }

        public void DeleteStudent(int id)
        {
            throw new NotImplementedException();
        }

        public Student GetStudentByEmail(string email)
        {
            return _context.Students.FirstOrDefault(s => s.email == email);
        }

        public Student GetStudentById(int id)
        {
            return _context.Students.First(s => s.Id == id);
        }

        public IEnumerable<Student> GetStudents()
        {
            return _context.Students.ToList();
        }

        public void UpdateStudent(Student student)
        {
            throw new NotImplementedException();
        }
    }
}

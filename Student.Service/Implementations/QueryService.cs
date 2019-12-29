using Student.Models;
using Student.Repository.Contracts;
using Student.Service.Contracts;
using System;
using System.Linq;

namespace Student.Service.Implementations
{
    public class QueryService : IQueryService
    {
        private readonly IStudentRepo<Student.Models.Student> _student;
        public QueryService(IStudentRepo<Student.Models.Student> student)
        {
            _student = student;
        }

        ServiceResponse IQueryService.GetStudent(Guid studentId)
        {
            var student = _student.Table.FirstOrDefault(x => x.StudentId.Equals(studentId));
            if (student != null)
                return new ServiceResponse() { IsSuccessful = true, Data = student };
            else
                return new ServiceResponse() { IsSuccessful = false };
        }

        ServiceResponse IQueryService.GetStudents()
        {
            var student = _student.Table.Where(x => x.IsActive.Equals(true)).ToList();
            if (student.Count > 0)
                return new ServiceResponse() { IsSuccessful = true, Data = student };
            else
                return new ServiceResponse() { IsSuccessful = false };
        }
    }
}

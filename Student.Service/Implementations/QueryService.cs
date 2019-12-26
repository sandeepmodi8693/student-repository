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
        private readonly IStudentRepo<Subject> _subject;
        public QueryService(IStudentRepo<Student.Models.Student> student, IStudentRepo<Subject> subject)
        {
            _student = student;
            _subject = subject;
        }

        public ServiceResponse GetSubject(Guid subjectId)
        {
            var subject = _subject.Table.FirstOrDefault(x=>x.SubjectId.Equals(subjectId));
            if (!Guid.Empty.Equals(subject.SubjectId))
                return new ServiceResponse() { IsSuccessful = true, Data = subject };
            else
                return new ServiceResponse() { IsSuccessful = false };
        }

        public ServiceResponse GetSubjectByStudent(Guid studentId)
        {
            var subjects = _subject.Table.Where(x=>x.StudentId.Equals(studentId)).ToList();
            if (subjects.Count > 0)
                return new ServiceResponse() { IsSuccessful = true, Data = subjects };
            else
                return new ServiceResponse() { IsSuccessful = false };
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

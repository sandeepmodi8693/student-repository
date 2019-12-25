using Student.Models;
using System;

namespace Student.Service.Contracts
{
    public interface IQueryService
    {
        ServiceResponse GetSubjectByStudent(Guid studentId);
        ServiceResponse GetSubject(Guid subjectId);
        ServiceResponse GetStudents();
        ServiceResponse GetStudent(Guid studentId);
    }
}

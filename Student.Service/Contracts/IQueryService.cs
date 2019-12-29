using Student.Models;
using System;

namespace Student.Service.Contracts
{
    public interface IQueryService
    {
        ServiceResponse GetStudents();
        ServiceResponse GetStudent(Guid studentId);
    }
}

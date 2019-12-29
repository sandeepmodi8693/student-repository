using Student.Models;
using System;

namespace Student.Service.Contracts
{
    public interface ICommandService
    {
        ServiceResponse AddStudent(Models.Student model);
        ServiceResponse DeleteStudent(Guid studentId);
        ServiceResponse UpdateStudent(Models.Student model);
    }
}

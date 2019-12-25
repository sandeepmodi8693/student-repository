using Student.Models;
using System;

namespace Student.Service.Contracts
{
    public interface ICommandService
    {
        ServiceResponse AddSubject(Subject model);
        ServiceResponse DeleteSubject(Guid subjectId);
        ServiceResponse UpdateSubject(Subject model);
        ServiceResponse AddStudent(Models.Student model);
        ServiceResponse DeleteStudent(Guid studentId);
        ServiceResponse UpdateStudent(Models.Student model);
        ServiceResponse Register(RegisterModel model);
    }
}

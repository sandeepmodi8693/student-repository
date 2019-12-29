using Student.Models;
using Student.Repository.Contracts;
using Student.Service.Contracts;
using System;
using System.Linq;

namespace Student.Service.Implementations
{
    public class CommandService : ICommandService
    {
        private readonly IStudentRepo<Student.Models.Student> _student;
        public CommandService(IStudentRepo<Student.Models.Student> student)
        {
            _student = student;
        }

        #region Student
        public ServiceResponse AddStudent(Student.Models.Student model)
        {
            try
            {
                model.CreatedDate = DateTime.UtcNow;
                model.UpdatedDate = DateTime.UtcNow;
                model.IsActive = true;
                _student.Insert(model);
                return new ServiceResponse() { IsSuccessful = true, Data = model };
            }
            catch (Exception ex)
            {
                return new ServiceResponse() { IsSuccessful = false };
            }
        }

        public ServiceResponse DeleteStudent(Guid studentId)
        {
            try
            {
                var student = _student.Table.FirstOrDefault(x => x.StudentId.Equals(studentId));
                _student.Delete(student);
                return new ServiceResponse() { IsSuccessful = true, Data = student };
            }
            catch (Exception ex)
            {
                return new ServiceResponse() { IsSuccessful = false };
            }
        }

        public ServiceResponse UpdateStudent(Student.Models.Student model)
        {
            var student = _student.Table.FirstOrDefault(x => x.StudentId.Equals(model.StudentId));
            student.FirstName = model.FirstName;
            student.LastName = model.LastName;
            student.UpdatedDate = DateTime.UtcNow;
            student.Mobile = model.Mobile;
            student.Email = model.Email;
            student.IsActive = model.IsActive;
            _student.Update(student);
            if (!Guid.Empty.Equals(student.StudentId))
                return new ServiceResponse() { IsSuccessful = true, Data = student };
            else
                return new ServiceResponse() { IsSuccessful = false };
        }
        #endregion
    }
}

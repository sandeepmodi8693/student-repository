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
        private readonly IStudentRepo<Subject> _subject;
        public CommandService(IStudentRepo<Student.Models.Student> student, IStudentRepo<Subject> subject)
        {
            _student = student;
            _subject = subject;
        }

        #region Subject
        public ServiceResponse AddSubject(Subject model)
        {
            try
            {
                _subject.Insert(model);
                return new ServiceResponse() { IsSuccessful = true, Data = model };
            }
            catch (Exception ex)
            {
                return new ServiceResponse() { IsSuccessful = false };
            }
        }

        public ServiceResponse DeleteSubject(Guid subjectId)
        {
            try
            {
                var subject = _subject.Table.FirstOrDefault(x=>x.StudentId.Equals(subjectId));
                _subject.Delete(subject);
                return new ServiceResponse() { IsSuccessful = true, Data = subject };
            }
            catch (Exception ex)
            {
                return new ServiceResponse() { IsSuccessful = false };
            }
        }

        public ServiceResponse UpdateSubject(Subject model)
        {
            var subject = _subject.Table.FirstOrDefault(x => x.StudentId.Equals(model.SubjectId));
            subject.SubjectName = model.SubjectName;
            subject.UpdatedDate = DateTime.UtcNow;
            subject.Result = model.Result;
            subject.Marks = model.Marks;
            _subject.Update(subject);
            if (!Guid.Empty.Equals(subject.SubjectId))
                return new ServiceResponse() { IsSuccessful = true, Data = subject };
            else
                return new ServiceResponse() { IsSuccessful = false };
        }
        #endregion

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

        public ServiceResponse DeleteStudent(Guid subjectId)
        {
            try
            {
                var student = _student.Table.FirstOrDefault(x => x.StudentId.Equals(subjectId));
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

        public ServiceResponse Register(RegisterModel model)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

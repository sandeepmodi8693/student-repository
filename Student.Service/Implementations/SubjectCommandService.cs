using Student.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student.Service.Implementations
{
    public class SubjectCommandService : ISubjectCommandService
    {
        //private readonly IRepositoryCommand _command;
        //public SubjectCommandService(IRepositoryCommand command)
        //{
        //    _command = command;
        //}

        //public ServiceResponse AddSubject(Subject model)
        //{
        //    var subject = _command.AddSubject(model);
        //    if (!Guid.Empty.Equals(subject.UserId))
        //        return new ServiceResponse() { IsSuccessful = true, Data = subject };
        //    else
        //        return new ServiceResponse() { IsSuccessful = false };
        //}

        //public ServiceResponse DeleteSubject(Guid subjectId)
        //{
        //    var subject = _command.DeleteSubject(subjectId);
        //    if (subject)
        //        return new ServiceResponse() { IsSuccessful = true, Data = subject };
        //    else
        //        return new ServiceResponse() { IsSuccessful = false };
        //}

        //public ServiceResponse UpdateSubject(Subject model)
        //{
        //    var subject = _command.UpdateSubject(model);
        //    if (!Guid.Empty.Equals(subject.SubjectId))
        //        return new ServiceResponse() { IsSuccessful = true, Data = subject };
        //    else
        //        return new ServiceResponse() { IsSuccessful = false };
        //}
    }
}

using Student.Service.Contracts;

namespace Student.Service.Implementations
{
    public class SubjectQueryService : ISubjectQueryService
    {
        //private readonly IRepositoryQuery _query;
        //public SubjectQueryService(IRepositoryQuery query)
        //{
        //    _query = query;
        //}

        //public ServiceResponse GetSubject(Guid subjectId)
        //{
        //    var subject = _query.GetSubject(subjectId);
        //    if (!Guid.Empty.Equals(subject.SubjectId))
        //        return new ServiceResponse() { IsSuccessful = true, Data = subject };
        //    else
        //        return new ServiceResponse() { IsSuccessful = false };
        //}

        //public ServiceResponse GetSubjectByStudent(Guid studentId)
        //{
        //    var subjects = _query.GetSubjectByStudent(studentId);
        //    if (subjects.Count > 0)
        //        return new ServiceResponse() { IsSuccessful = true, Data = subjects };
        //    else
        //        return new ServiceResponse() { IsSuccessful = false };
        //}
    }
}

using Student.Models;
using Student.Service.Contracts;
using System;
using System.Web.Http;

namespace Student.Api.Controllers
{
    [Authorize]
    public class StudentController : ApiController
    {
        private readonly ICommandService _commandService;
        private readonly IQueryService _queryService;

        public StudentController(ICommandService commandService, IQueryService queryService)
        {
            _commandService = commandService;
            _queryService = queryService;
        }

        public StudentController() { }
        
        #region Subject API's
        [HttpGet]
        public IHttpActionResult GetSubject(Guid subjectId)
        {
            return Ok(_queryService.GetSubject(subjectId));
        }

        [HttpGet]
        public IHttpActionResult GetSubjectByStudent(Guid studentId)
        {
            return Ok(_queryService.GetSubjectByStudent(studentId));
        }

        [HttpPost]
        public IHttpActionResult AddSubject(Subject model)
        {
            return Ok(_commandService.AddSubject(model));
        }

        [HttpPut]
        public IHttpActionResult UpdateSubject(Subject model)
        {
            return Ok(_commandService.UpdateSubject(model));
        }

        [HttpDelete]
        public IHttpActionResult DeleteSubject(Guid studentId)
        {
            return Ok(_commandService.DeleteSubject(studentId));
        }
        #endregion

        #region Student API's
        [HttpGet]
        public IHttpActionResult GetStudent(Guid studentId)
        {
            return Ok(_queryService.GetStudent(studentId));
        }

        [HttpGet]
        public IHttpActionResult GetStudents()
        {
            return Ok(_queryService.GetStudents());
        }

        [HttpPost]
        public IHttpActionResult AddStudent(Student.Models.Student model)
        {
            return Ok(_commandService.AddStudent(model));
        }

        [HttpPut]
        public IHttpActionResult UpdateStudent(Student.Models.Student model)
        {
            return Ok(_commandService.UpdateStudent(model));
        }

        [HttpDelete]
        public IHttpActionResult DeleteStudent(Guid studentId)
        {
            return Ok(_commandService.DeleteStudent(studentId));
        }
        #endregion
    }
}

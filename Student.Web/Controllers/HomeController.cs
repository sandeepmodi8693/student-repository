using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Student.Web.Models;
using Student.Web.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Student.Web.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; }
        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Student()
        {
            if (HttpContext.Session.Get("Token") == null)
                return RedirectToAction("Index");
            var students = await ApiHelper.GetAsync<ServiceResponse<List<StudentModel>>>(Configuration.GetSection("ApiBaseURL").Value + "api/Student/GetStudents", System.Text.Encoding.UTF8.GetString(HttpContext.Session.Get("Token")));
            return View(students.Data);
        }

        [HttpGet]
        public async Task<IActionResult> StudentInfo(Guid studentId)
        {
            if (HttpContext.Session.Get("Token") == null)
                return RedirectToAction("Index");
            var students = await ApiHelper.GetAsync<ServiceResponse<StudentModel>>(Configuration.GetSection("ApiBaseURL").Value + "api/Student/GetStudent?studentId=" + studentId, System.Text.Encoding.UTF8.GetString(HttpContext.Session.Get("Token")));
            return View(students.Data);
        }

        [HttpPost]
        public async Task<IActionResult> StudentInfo(StudentModel model)
        {
            if (HttpContext.Session.Get("Token") == null)
                return RedirectToAction("Index");
            if (model.StudentId == Guid.Empty)
            {
                var students = await ApiHelper.PostAsync<BaseServiceResponse>(Configuration.GetSection("ApiBaseURL").Value + "api/Student/AddStudent", System.Text.Encoding.UTF8.GetString(HttpContext.Session.Get("Token")), new PostObject() { PostData = model });
                return RedirectToAction("Student");
            }
            else
            {
                var students = await ApiHelper.PostAsync<BaseServiceResponse>(Configuration.GetSection("ApiBaseURL").Value + "api/Student/UpdateStudent", System.Text.Encoding.UTF8.GetString(HttpContext.Session.Get("Token")), new PostObject() { PostData = model });
                return RedirectToAction("Student");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteStudent(Guid studentId)
        {
            if (HttpContext.Session.Get("Token") == null)
                return RedirectToAction("Index");
            var students = await ApiHelper.DeleteAsync<BaseServiceResponse>(Configuration.GetSection("ApiBaseURL").Value + "api/Student/DeleteStudent?studentId=" + studentId, System.Text.Encoding.UTF8.GetString(HttpContext.Session.Get("Token")));
            return RedirectToAction("Student");
        }
    }
}

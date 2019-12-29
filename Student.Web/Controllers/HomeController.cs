using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Student.Web.Models;
using Student.Web.Utility;

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

        //[HttpPost]
        //public async Task<IActionResult> Index(LoginModel model)
        //{
        //    model.grant_type = "password";
        //    var response = await ApiHelper.PostAsync<TokenResponse>(Configuration.GetSection("ApiBaseURL").Value + "token", string.Empty, new PostObject() { PostData = model });
        //    if (!string.IsNullOrEmpty(response.userName) && !string.IsNullOrEmpty(response.access_token))
        //    {
        //        HttpContext.Session.SetString("UserName", response.userName);
        //        HttpContext.Session.SetString("Token", response.access_token);
        //        return RedirectToAction("Student");
        //    }
        //    return View();
        //}

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel model)
        {
            model.grant_type = "password";
            //var students = await ApiHelper.GetAsync<ServiceResponse>(Configuration.GetSection("ApiBaseURL").Value + "api/Student/GetStudents");
            var response = await ApiHelper.PostAsync<TokenResponse>(Configuration.GetSection("ApiBaseURL").Value + "token", new PostObject() { PostData = model });
            return View();
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

        public IActionResult Student()
        {
            return View();
        }
    }
}

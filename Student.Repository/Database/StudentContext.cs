using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Student.Models;
using Student.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Student.Repository.Database
{
    public partial class StudentContext : IdentityDbContext<ApplicationUser>, IStudentContext
    {
        public StudentContext()
            : base(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString(), false)
        {
            System.Data.Entity.Database.SetInitializer<StudentContext>(new InitialMigration());
            this.Configuration.LazyLoadingEnabled = false;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public static StudentContext Create()
        {
            return new StudentContext();
        }
        public DbSet<Models.Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
    }

    public class InitialMigration : CreateDatabaseIfNotExists<StudentContext>
    {
        protected override void Seed(StudentContext context)
        {
            if (!context.Roles.Any())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                List<string> userRoles = new List<string>() { "Student", "Teacher" };
                foreach (var role in userRoles)
                {
                    roleManager.Create(new IdentityRole() { Name = role });
                }
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new ApplicationUserManager(userStore);
            if (!userManager.Users.Any())
            {
                List<Tuple<string, string, string>> UserInfo = new List<Tuple<string, string, string>>();
                UserInfo.Add(new Tuple<string, string, string>("test.student@gmail.com", "Student", "Secure*12"));
                UserInfo.Add(new Tuple<string, string, string>("test.teacher@gmail.com", "Teacher", "Secure*12"));
                foreach (var userDetail in UserInfo)
                {
                    var user = new ApplicationUser
                    {
                        Email = userDetail.Item1,
                        UserName = userDetail.Item1
                    };
                    userManager.Create(user, userDetail.Item3);
                    userManager.AddToRole(user.Id, userDetail.Item2);
                }
            }
            base.Seed(context);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Student.Api.Controllers;
using Student.Models;
using Student.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;

namespace Student.Api.Tests.Controllers
{
    [TestClass]
    public class StudentControllerTests
    {
        private MockRepository mockRepository;
        private Mock<ICommandService> mockCommandService;
        private Mock<IQueryService> mockQueryService;
        List<Student.Models.Student> students;

        [TestInitialize]
        public void TestInitialize()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
            mockCommandService = mockRepository.Create<ICommandService>();
            mockQueryService = mockRepository.Create<IQueryService>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            mockRepository.VerifyAll();
        }

        private StudentController CreateStudentController()
        {
            students = new List<Student.Models.Student>()
            {
                new Student.Models.Student() { Email = "test1@example.com", StudentId = Guid.NewGuid(), IsActive= true },
                new Student.Models.Student() { Email = "test2@example.com", StudentId = Guid.NewGuid(), IsActive= true },
                new Student.Models.Student() { Email = "test3@example.com", StudentId = Guid.NewGuid(), IsActive= true }
            };
            return new StudentController(mockCommandService.Object, mockQueryService.Object);
        }

        [TestMethod]
        public void GetStudent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var studentController = this.CreateStudentController();
            Guid studentId = default(global::System.Guid);
            ServiceResponse response = new ServiceResponse() { IsSuccessful = true, Data = students.FirstOrDefault() };

            mockQueryService.Setup(x => x.GetStudent(It.IsAny<Guid>())).Returns(response);

            // Act
            var result = studentController.GetStudent(studentId) as OkNegotiatedContentResult<ServiceResponse>;
           
            // Assert
            Assert.IsTrue(result.Content.IsSuccessful);
            
        }

        [TestMethod]
        public void GetStudents_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var studentController = this.CreateStudentController();
            ServiceResponse response = new ServiceResponse() { IsSuccessful = true, Data = students };
            mockQueryService.Setup(x => x.GetStudents()).Returns(response);
            // Act
            var result = studentController.GetStudents() as OkNegotiatedContentResult<ServiceResponse>;

            // Assert
            Assert.IsTrue(result.Content.IsSuccessful);
        }

        [TestMethod]
        public void AddStudent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var studentController = this.CreateStudentController();
            Student.Models.Student model = students.FirstOrDefault();
            ServiceResponse response = new ServiceResponse() { IsSuccessful = true, Data = model };

            mockCommandService.Setup(x => x.AddStudent(It.IsAny<Student.Models.Student>())).Returns(response);
            // Act
            var result = studentController.AddStudent(model) as OkNegotiatedContentResult<ServiceResponse>;

            // Assert
            Assert.IsTrue(result.Content.IsSuccessful);
        }

        [TestMethod]
        public void UpdateStudent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var studentController = this.CreateStudentController();
            Student.Models.Student model = students.FirstOrDefault();
            ServiceResponse response = new ServiceResponse() { IsSuccessful = true, Data = model };
            mockCommandService.Setup(x => x.UpdateStudent(It.IsAny<Student.Models.Student>())).Returns(response);

            // Act
            var result = studentController.UpdateStudent(model) as OkNegotiatedContentResult<ServiceResponse>;

            // Assert
            Assert.IsTrue(result.Content.IsSuccessful);
        }

        [TestMethod]
        public void DeleteStudent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var studentController = this.CreateStudentController();
            Guid studentId = students.FirstOrDefault().StudentId;
            ServiceResponse response = new ServiceResponse() { IsSuccessful = true };
            mockCommandService.Setup(x => x.DeleteStudent(It.IsAny<Guid>())).Returns(response);
            // Act
            var result = studentController.DeleteStudent(studentId) as OkNegotiatedContentResult<ServiceResponse>;

            // Assert
            Assert.IsTrue(result.Content.IsSuccessful);
        }
    }
}

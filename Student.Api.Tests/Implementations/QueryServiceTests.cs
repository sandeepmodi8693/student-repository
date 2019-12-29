using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Student.Repository.Contracts;
using Student.Service.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Student.Api.Tests.Implementations
{
    [TestClass]
    public class QueryServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IStudentRepo<Student.Models.Student>> mockStudentRepo;
        List<Student.Models.Student> students;
        [TestInitialize]
        public void TestInitialize()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockStudentRepo = this.mockRepository.Create<IStudentRepo<Student.Models.Student>>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.mockRepository.VerifyAll();
        }

        private QueryService CreateService()
        {
            students = new List<Student.Models.Student>()
            {
                new Student.Models.Student() { Email = "test1@example.com", StudentId = Guid.NewGuid(), IsActive= true },
                new Student.Models.Student() { Email = "test2@example.com", StudentId = Guid.NewGuid(), IsActive= true },
                new Student.Models.Student() { Email = "test3@example.com", StudentId = Guid.NewGuid(), IsActive= true }
            };
            return new QueryService(
                this.mockStudentRepo.Object);
        }

        [TestMethod]
        public void GetStudent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            Guid studentId = students.FirstOrDefault().StudentId;
            mockStudentRepo.Setup(x => x.Table).Returns(students.AsQueryable());
            // Act
            var result = service.GetStudent(studentId);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public void GetStudents_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            mockStudentRepo.Setup(x => x.Table).Returns(students.AsQueryable());

            // Act
            var result = service.GetStudents();

            // Assert
            Assert.IsTrue(result.IsSuccessful);
        }
    }
}

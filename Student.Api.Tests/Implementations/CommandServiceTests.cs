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
    public class CommandServiceTests
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

        private CommandService CreateService()
        {
            students = new List<Student.Models.Student>()
            {
                new Student.Models.Student() { Email = "test1@example.com", StudentId = Guid.NewGuid() },
                new Student.Models.Student() { Email = "test2@example.com", StudentId = Guid.NewGuid() },
                new Student.Models.Student() { Email = "test3@example.com", StudentId = Guid.NewGuid() }
            };
            return new CommandService(
                this.mockStudentRepo.Object);
        }

        [TestMethod]
        public void AddStudent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            Student.Models.Student model = new Student.Models.Student() { Email = "test@example.com", StudentId = Guid.NewGuid() };
            mockStudentRepo.Setup(x => x.Insert(It.IsAny<Student.Models.Student>()));
            // Act
            var result = service.AddStudent(model);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public void DeleteStudent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();

            mockStudentRepo.Setup(x => x.Table).Returns(students.AsQueryable());
            mockStudentRepo.Setup(x => x.Delete(It.IsAny<Student.Models.Student>()));
            // Act
            var result = service.DeleteStudent(students.FirstOrDefault().StudentId);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
        }

        [TestMethod]
        public void UpdateStudent_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            Student.Models.Student model = students.FirstOrDefault();

            mockStudentRepo.Setup(x => x.Table).Returns(students.AsQueryable());
            mockStudentRepo.Setup(x => x.Update(It.IsAny<Student.Models.Student>()));
            // Act
            var result = service.UpdateStudent(model);

            // Assert
            Assert.IsTrue(result.IsSuccessful);
        }
    }
}

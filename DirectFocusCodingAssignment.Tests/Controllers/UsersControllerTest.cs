using AutoMapper;
using DirectFocusCodingAssignment.Controllers;
using DirectFocusCodingAssignment.Data;
using DirectFocusCodingAssignment.Data.Entities;
using DirectFocusCodingAssignment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace DirectFocusCodingAssignment.Tests
{
    [TestClass]
    public class UsersControllerTest
    {
        private Mock<ILogger<UsersController>> logger;
        private Mock<IMapper> mapper;
        private UsersController usersController;
        private Mock<IUserRepository> repository;

        public UsersControllerTest()
        {
            logger = new Mock<ILogger<UsersController>>();
            mapper = new Mock<IMapper>();

            repository = new Mock<IUserRepository>();
            usersController = new UsersController(repository.Object, mapper.Object, logger.Object);
        }

        private List<UserViewModel> MockUserViewModel()
        {
            return new List<UserViewModel>(){
                    new UserViewModel(){  Id = 1,
                                  Name = "Alice",
                                  Address = "North Vancouver",
                                  Age = 10
                                },
                    new UserViewModel(){ Id = 2,
                                Name = "Jack",
                                Address = "Vancouver",
                                Age = 112
                    }
            };
        }

        private static List<User> MockUserList()
        {
            return new List<User>(){
                    new User(){  Id = 1,
                                  Name = "Alice",
                                  Address = "North Vancouver",
                                  Age = 10
                                },
                    new User(){ Id = 2,
                                Name = "Jack",
                                Address = "Vancouver",
                                Age = 112
                    }
            };
        }

        [TestMethod]
        public void Get_HappyPath()
        {
            //Arrange

            List<User> mockUserList = MockUserList();
            List<UserViewModel> mockUserViewModel = MockUserViewModel();

            mapper.Setup(m => m.Map<List<User>, List<UserViewModel>>(It.IsAny<List<User>>())).Returns(mockUserViewModel);
            repository.Setup(u => u.GetUsers()).Returns(mockUserList);

            //Act

            var okResult = usersController.Get() as OkObjectResult;
            List<UserViewModel> result = (List<UserViewModel>)okResult.Value;
            
            //Assert
            Assert.AreEqual(okResult.StatusCode, 200);
            Assert.AreEqual(result.Count, mockUserViewModel.Count);
            CollectionAssert.AreEqual(result, mockUserViewModel);

        }

        [TestMethod]
        public void Get_Return_NotFound_When_NoUser()
        {
            //Arrange

            List<User> users = null;
            repository.Setup(u => u.GetUsers()).Returns(users);

            //Act

            var result = usersController.Get() as NotFoundResult;

            //Assert
            Assert.AreEqual(result.StatusCode, 404);
            

        }

        [TestMethod]
        public void Get_Return_BadRequest_When_Exception()
        {
            //Arrange
            repository.Setup(u => u.GetUsers()).Throws(new System.Exception("Error!"));

            //Act
            var result = usersController.Get() as BadRequestObjectResult;

            //Assert
            Assert.AreEqual(result.StatusCode, 400);

        }
        [TestMethod]
        public void Post_HappyPath()
        {
            //Arrange           
            var mockUserVM= new UserViewModel()
            {
                Id = 1,
                Name = "Alice",
                Address = "North Vancouver",
                Age = 10
            };
            var mockUser = new User()
            {
                Id = 1,
                Name = "Alice",
                Address = "North Vancouver",
                Age = 10
            };
            mapper.Setup(m => m.Map<UserViewModel, User>(It.IsAny<UserViewModel>())).Returns(mockUser);
            repository.Setup(u => u.CreateUser(mockUser));
            repository.Setup(u => u.SaveAll()).Returns(true);
            mapper.Setup(m => m.Map< User, UserViewModel>(It.IsAny<User>())).Returns(mockUserVM);

            //Act
            var createdResult = usersController.Post(mockUserVM) as CreatedResult;
            UserViewModel result = (UserViewModel)createdResult.Value;

            //Assert
            Assert.AreEqual(createdResult.StatusCode, 201);
            Assert.AreEqual(mockUserVM.Address, result.Address);
            Assert.AreEqual(mockUserVM.Name, result.Name);
            Assert.AreEqual(mockUserVM.Age, result.Age);
            Assert.AreEqual(mockUserVM.Id, result.Id);
           
        }



    }
}

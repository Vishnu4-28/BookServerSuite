using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_commerce.Server.Model.DTO;
using E_commerce.Server.Model.Entities;
using E_commerce.Server.Service;
using Microsoft.AspNetCore.Http;
using Moq;

namespace UnitTestProject.Controller
{

    public class UnitTestAuthController
    {
        private readonly Mock<IAuth> _authServiceMock;

        public UnitTestAuthController()
        {
            _authServiceMock = new Mock<IAuth>();
        }

        [Fact]
        public void SignUpUser()
        {
            // Arrange  
            var newUser = new UserReq
            {
                First_Name = "John",
                Last_Name = "Doe",
                Date_Of_Birth = new DateTime(1990, 1, 1),
                Email = "John@gmail.com",
                Password = "password123",
                Phone_Number = "1234567890"
            };

            _authServiceMock.Setup(service => service.UserSignup(It.IsAny<UserReq>()))
                .ReturnsAsync((200, true));

            // Act  
            var result = _authServiceMock.Object.UserSignup(newUser).Result;

            // Assert  
            Assert.Equal(200, result.statusCode);
            Assert.True(result.success);
        }

        [Fact]
        public void SignInUser()
        {
            // Arrange  
            var newUser = new SignInReq
            { 
                Email = "John@gmail.com", 
                Password = "password123" 
            };
            var UserRes = new User
            {
                User_Id = 1,
                Role = UserRole.User,     
                First_Name = "John",
                Last_Name = "Doe",
                Email = "John@gmail.com",
                Password = "password123",  
                Date_Of_Birth = new DateTime(1990, 1, 1),      
                Phone_Number = "1234567890"
            };

            _authServiceMock.Setup(service => service.UserSignIn(newUser));
            _authServiceMock.Setup(service => service.UserSignIn(It.IsAny<SignInReq>())).ReturnsAsync((200, new List<User> { UserRes }, true));
                //.ReturnsAsync((200, UserRes, true));

            // Act  
            var signInRequest = new SignInReq { Email = "John@gmail.com", Password = "password123" };
            var result = _authServiceMock.Object.UserSignIn(signInRequest).Result;

            // Assert  
            Assert.Equal(200, result.statusCode);
            Assert.NotNull(result.Users);
            Assert.True(result.success);
        }

        [Fact]
        public void PostFileAsyncIntoDatabase()
        {
            // Arrange  
            var fileData = new Mock<IFormFile>();
            var imageCaption = "Sample Image";
            var imageDescription = "This is a sample image description.";
            int Book_id = 1;

            var expectedBookImg = new BookImg
            {
                Book_id = Book_id,
                ImageCaption = imageCaption,
                ImageDescription = imageDescription,
                FilePath = "path/to/image.jpg"
            };
            _authServiceMock.Setup(service => service.PostFileAsync(fileData.Object, imageCaption, imageDescription, Book_id))
                .ReturnsAsync(expectedBookImg);


            // Act  
            var result = _authServiceMock.Object.PostFileAsync(fileData.Object, imageCaption, imageDescription, Book_id).Result;

            // Assert  
            Assert.NotNull(result);
            Assert.Equal(expectedBookImg.Book_id, result.Book_id);
            Assert.Equal(expectedBookImg.ImageCaption, result.ImageCaption);
            Assert.Equal(expectedBookImg.ImageDescription, result.ImageDescription);
        }



    }
}

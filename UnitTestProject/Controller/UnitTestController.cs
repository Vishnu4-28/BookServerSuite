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
    public class UnitTestController
    {
        private readonly Mock<IBookService> _serviceMock;


        public UnitTestController()
        {
            _serviceMock = new Mock<IBookService>();
        }

        [Fact]
        public void getAllBooks_ListOfBooks()
        {
            // Arrange
            var expectedBooks = new List<BookRes>
            {
                new BookRes { Book_Id = 1, Title = "Book 1", Author = "Author 1", ISBN = 123456, Quantity = 10 },
                new BookRes { Book_Id = 2, Title = "Book 2", Author = "Author 2", ISBN = 789012, Quantity = 5 }
            };

            _serviceMock.Setup(s => s.GetBooks())
                .ReturnsAsync((200, expectedBooks, true));

            // Act
            var result = _serviceMock.Object.GetBooks().Result;

            // Assert
            Assert.Equal(200, result.statusCode);
            Assert.True(result.success);
            Assert.NotNull(result.Books);
            Assert.Equal(expectedBooks.Count, result.Books.Count());
        }

        [Fact]
        public void AddBooksIntoDatabase()
        {
            // Arrange
            var newBook = new BookReq
            {
                Title = "New Book",
                Author = "New Author",
                ISBN = 123456789,
                Quantity = 10
            };

            _serviceMock.Setup(s => s.AddBooks(newBook))
                .ReturnsAsync((201, true));

            // Act
            var result = _serviceMock.Object.AddBooks(newBook).Result;

            // Assert
            Assert.Equal(201, result.statusCode);
            Assert.True(result.success);
        }

        [Fact]
        public void DeleteBookFromDatabase()
        {
            // Arrange
            int bookIdToDelete = 1;
            _serviceMock.Setup(s => s.deleteBook(bookIdToDelete))
                .ReturnsAsync((204, true));

            // Act
            var result = _serviceMock.Object.deleteBook(bookIdToDelete).Result;

            // Assert
            Assert.Equal(204, result.statusCode);
            Assert.True(result.success);
        }

        [Fact]

        public void UpdateByIdInDatabase()
        {
            //Arrange
            int bookIdToUpdate = 1;

            _serviceMock.Setup(s => s.UpdateById(It.IsAny<UpdateBookReq>(), bookIdToUpdate))
                .ReturnsAsync((200, true));

            //Act
            var result = _serviceMock.Object.UpdateById(new UpdateBookReq
            {
                Title = "Updated Book",
                Author = "Updated Author",
                ISBN = 987654321,
                Quantity = 15
            }, bookIdToUpdate).Result;

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.True(result.success);

        }

        [Fact]
        public void GetByFromDatabase()
        {
            // Arrange
            int bookIdToGet = 1;

            var expectedBook = new Books
            {
                Book_Id = bookIdToGet,
                Title = "Book 1",
                Author = "Author 1",
                ISBN = 123456,
                Quantity = 10
            };

            _serviceMock.Setup(s => s.GetById(bookIdToGet))
                .ReturnsAsync((200, new List<Books> { expectedBook }, true));

            // Act
            var result = _serviceMock.Object.GetById(bookIdToGet).Result;

            // Assert
            Assert.Equal(200, result.StatusCode);
            Assert.True(result.success);
            Assert.NotNull(result.Books);
            Assert.Single(result.Books);
            Assert.Equal(expectedBook.Book_Id, result.Books.First().Book_Id);
        }



        [Fact]
        public void SoftDeleteBookFromDatabase()
        {
            // Arrange
            int bookIdToSoftDelete = 1;
            _serviceMock.Setup(s => s.SoftDeleteBook(bookIdToSoftDelete))
                .ReturnsAsync((true, 200, "Book soft deleted successfully"));

            // Act
            var result = _serviceMock.Object.SoftDeleteBook(bookIdToSoftDelete).Result;

            // Assert
            Assert.True(result.success);
            Assert.Equal(200, result.statusCode);
            Assert.Equal("Book soft deleted successfully", result.message);

        }

        [Fact]
        public void getDeleteBooksFromDatabase()
        {
            //Arrange 

            var expectedBooks = new List<BookRes>
            {
                new BookRes { Book_Id = 1, Title = "Book 1", Author = "Author 1", ISBN = 123456, Quantity = 10 },
                new BookRes { Book_Id = 2, Title = "Book 2", Author = "Author 2", ISBN = 789012, Quantity = 5 }
            };

            _serviceMock.Setup(s => s.getDeleteBooks())
                .ReturnsAsync((200, expectedBooks, true));

            // Act
            var result = _serviceMock.Object.getDeleteBooks().Result;

            //Assert 
            Assert.Equal(200, result.statusCode);
            Assert.True(result.success);

        }


        [Fact]
        public void RestoreBookFromDatabase()
        {
            //Arrange
            int bookIdToRestore = 1;

            _serviceMock.Setup(s => s.RestoreBook(bookIdToRestore))
                .ReturnsAsync((200, true));

            //Act
            var result = _serviceMock.Object.RestoreBook(bookIdToRestore).Result;


            //Assert
            Assert.Equal(200, result.StatusCode);
            Assert.True(result.success);
        }


        [Fact]
        public void PostExcelFileFromDatabase()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var expectedBooks = new List<Books>
            {
                new Books { Book_Id = 1, Title = "Book 1", Author = "Author 1", ISBN = 123456, Quantity = 10 },
                new Books { Book_Id = 2, Title = "Book 2", Author = "Author 2", ISBN = 789012, Quantity = 5 }
            };
               
            _serviceMock.Setup(s => s.PostExcelFile(It.IsAny<IFormFile>()))
                .ReturnsAsync(expectedBooks);

            // Act
            var result = _serviceMock.Object.PostExcelFile(fileMock.Object).Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedBooks.Count, result.Count);
            Assert.Equal(expectedBooks.First().Book_Id, result.First().Book_Id);
        }


    }
}

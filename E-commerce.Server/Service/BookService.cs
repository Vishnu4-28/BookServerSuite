﻿using System;
using System.Net;
using System.Runtime.Intrinsics.X86;
using E_commerce.Server.DAL.BASE;
using E_commerce.Server.data;
using E_commerce.Server.Model.DTO;
using E_commerce.Server.Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using static System.Net.WebRequestMethods;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;

namespace E_commerce.Server.Service
{
    public class BookService : IBookService
    {
        private readonly IRepository<Books> _booksRepository;
        private readonly IRepository<BookImg> _bookImgRepository;
        private readonly ApplicationDbContext _DbContext;

        public BookService(IRepository<Books> booksRepository ,IRepository<BookImg> BookImgRepo  ,ApplicationDbContext dbContext)
        {
            _booksRepository = booksRepository;
            _DbContext = dbContext;
            _bookImgRepository = BookImgRepo;
        }

        public async Task<(int statusCode, IEnumerable<BookRes>? Books, bool success)> GetBooks()
        {
            try
            {
                var books = await _booksRepository.GetAll();
                var bookImg = await _bookImgRepository.GetAll();

                if (books == null || !books.Any())
                {
                    return (404, null, false);
                }
                
                if (books.Any(b => b.IsDeleted))
                {
                    books = books.Where(b => !b.IsDeleted);
                }

                if (!books.Any())
                {
                    return (404, null, false);
                }


                var data = (from bookdetail in books
                            join img in bookImg
                            on bookdetail.Book_Id equals img.Book_id into bookImages
                            from img in bookImages.DefaultIfEmpty()
                            select new BookRes
                            {
                                Book_Id = bookdetail.Book_Id,
                                Title = bookdetail.Title,
                                Author = bookdetail.Author,
                                ISBN = bookdetail.ISBN,
                                Quantity = bookdetail.Quantity,
                                IsDeleted = bookdetail.IsDeleted,
                                FilePath = img != null ? img.FilePath : null
                            }).ToList();


                return (200, data , true);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error in GetBooks: {ex.Message}");
                return (500, null, false);
            }
        }


        public async Task<(int statusCode, IEnumerable<BookRes>? Books, bool success)> getDeleteBooks()
        {
            try
            {
                var books = await _booksRepository.GetAll();

                var bookImg = await _bookImgRepository.GetAll();

                if (books == null || !books.Any())
                {
                    return (404, null, false);
                }

                if (books.Any(b => b.IsDeleted))
                {
                    books = books.Where(b => b.IsDeleted);
                }
                else
                {
                    books = [];
                }

                if (!books.Any())
                {
                    return (404, null, false);
                }


                var data = (from bookdetail in books
                            join img in bookImg
                            on bookdetail.Book_Id equals img.Book_id into bookImages
                            from img in bookImages.DefaultIfEmpty()

                            select new BookRes
                            {
                                Book_Id = bookdetail.Book_Id,
                                Title = bookdetail.Title,
                                Author = bookdetail.Author,
                                ISBN = bookdetail.ISBN,
                                Quantity = bookdetail.Quantity,
                                IsDeleted = bookdetail.IsDeleted,
                                FilePath = img != null ? img.FilePath : null
                            }).ToList();

                return (200, data, true);

            }
            catch
            {
                return (500, null, false);
            }
        }


        public async Task<(int statusCode, bool success)> AddBooks(BookReq req)
        {
            try
            {
                var Books = new Books
                {
                    Title = req.Title ?? "",
                    Author = req.Author ?? "",
                    ISBN = req.ISBN,
                    Quantity = req.Quantity
                };

                await _booksRepository.Add(Books);

                return (201, true);
            }
            catch
            {
                return (500, false);
            }
        }


        public async Task<(int StatusCode, bool success)> RestoreBook( int book_id)
        {
            var book = await _booksRepository.GetById(book_id);
            if (book == null)
                return ( 404, false);


            book.IsDeleted = false;
            await _booksRepository.Update(book);
            return (200, true);
        }



        public async Task<(int StatusCode, bool success)> UpdateById(UpdateBookReq req, int book_id)
        {
            try
            {
                var book = await _booksRepository.GetById(book_id);
                if (book == null)
                {
                    return (404, false);
                }

                book.Title = req.Title ?? book.Title;
                book.Author = req.Author ?? book.Author;

               
                if (req.ISBN.HasValue && req.ISBN.Value != book.ISBN)
                {
                    book.ISBN = req.ISBN.Value;
                }

                if (req.Quantity.HasValue && req.Quantity.Value != book.Quantity)
                {
                    book.Quantity = req.Quantity.Value;
                }

                await _booksRepository.Update(book);
                return (200, true);
            }
            catch
            {
                return (500, false);
            }
        }
            

        public async Task<(int statusCode, bool success)> deleteBook(int book_id)
        {
            try
            {
                var book = await _booksRepository.GetById(book_id);

                var BookImg = await _bookImgRepository.GetAll();

                var img = BookImg.FirstOrDefault(b => b.Book_id == book_id);

                //if (img == null)
                //{
                //    return (404, false);
                //}

                if (book == null)
                {
                    return (404, false);
                }

                await _booksRepository.Delete(book);
                if (img != null)
                {
                await _bookImgRepository.Delete(img);
                }
                return (200, true);
            }
            catch
            {
                return (500, false);
            }
        }



        public async Task<(int StatusCode, IEnumerable<Books>? Books, bool success)> GetById(int book_id)
        {
            try
            {
                var book = await _booksRepository.GetById(book_id);
                if (book == null)
                {
                    return (404, null, false);
                }

                return (200, new List<Books> { book }, true);
            }
            catch
            {
                return (500, null, false);
            }
        }


        public async Task<(bool success, int statusCode, string message)> SoftDeleteBook(int bookId)
        {
            var book = await _booksRepository.GetById(bookId);
            if (book == null)
                return (false, 404, "Book not found");

            if (book.IsDeleted)
                return (false, 400, "Book already deleted");

            book.IsDeleted = true;
            await _booksRepository.Update(book);
            return (true, 200, "Book soft deleted successfully");
        }

        public async Task<List<Books>> PostExcelFile(IFormFile fileData)
        {
            if (fileData == null || fileData.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileData.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileData.CopyToAsync(stream);
            }

            var books = new List<Books>();


            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {


                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
                {
                    reader.Read();
                    while (reader.Read())
                    {
                        var book = new Books
                        {
                            Title = reader.GetValue(0)?.ToString(),
                            Author = reader.GetValue(1)?.ToString(),
                            ISBN = reader.GetValue(2) != null ? Convert.ToInt32(reader.GetValue(2)) : 0,
                            Quantity = reader.GetValue(3) != null ? Convert.ToInt32(reader.GetValue(3)) : 0,
                            IsDeleted = false
                        };
                        books.Add(book);
                        await _booksRepository.Add(book);
                    }
                    //await _dbContext.SaveChangesAsync();
                }
            }
            return books;
        }



    }

}


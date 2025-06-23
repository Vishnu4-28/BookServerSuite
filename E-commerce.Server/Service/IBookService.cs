﻿using E_commerce.Server.Model.DTO;
using E_commerce.Server.Model.Entities;

namespace E_commerce.Server.Service
{
    public interface IBookService
    {
        Task<(int statusCode, IEnumerable<BookRes>? Books, bool success)> GetBooks();

        Task<(int statusCode, bool success)> AddBooks(BookReq req);

        Task<(int statusCode, bool success)> deleteBook(int book_id);

        Task<(int StatusCode, bool success)> UpdateById(UpdateBookReq req, int book_id);

        Task<(int StatusCode, IEnumerable<Books>? Books,  bool success)> GetById( int book_id);


        Task<(bool success, int statusCode, string message)> SoftDeleteBook(int bookId);

        Task<(int statusCode, IEnumerable<BookRes>? Books, bool success)> getDeleteBooks();

        Task<(int StatusCode, bool success)> RestoreBook( int book_id);

        Task<List<Books>> PostExcelFile(IFormFile fileData);

    }
}

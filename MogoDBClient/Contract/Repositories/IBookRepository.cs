using MogoDBClient.Contract.DTO;
using MogoDBClient.Contract.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MogoDBClient.Contract.Repositories
{
    public interface IBookRepository : _IRepository<Book>
    {
        Task<Book> GetBookByIdAsync(string bookId);

        Task<string> CreateBookAsync(CreateOrUpdateBookDto model);

        Task<Book> UpdateBookAsync(string id, CreateOrUpdateBookDto model);
        Task DeleteBookAsync(string id);
    }
}

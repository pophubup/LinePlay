using MogoDBClient.Contract;
using MogoDBClient.Contract.DTO;
using MogoDBClient.Contract.Entities;
using MogoDBClient.Contract.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MogoDBClient.Core.Repositories
{
    public class BookDetailRepository : IBookDetailRepository
    {
        private readonly IMongoCollection<BookLocationDetail> _books;
        public BookDetailRepository(IMongoDatabase database)
        {
            _books = database.GetCollection<BookLocationDetail>(MongoCollectionNames.BookLocationDetail);
        }

        public async Task AddAsync(BookLocationDetail obj)
        {
            await _books.InsertOneAsync(obj);
        }

        public async Task<string> CreateBookAsync(BookLocationDetailDto model)
        {
            BookLocationDetail bookLocationDetail = new BookLocationDetail()
            {
                BookID = model.BookID,
                Address = model.Address,
                Area = model.Area,
                City = model.City,
                StoreName = model.StoreName
            };
             await AddAsync(bookLocationDetail);
            return bookLocationDetail._Id;
        }

        public Task DeleteAsync(Expression<Func<BookLocationDetail, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task DeleteBookAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<BookLocationDetail> GetAll()
        {
            return _books.AsQueryable();
        }

        public Task<BookLocationDetail> GetBookDetailByIdAsync(string bookDetId)
        {
            throw new NotImplementedException();
        }

        public Task<BookLocationDetail> GetSingleAsync(Expression<Func<BookLocationDetail, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<BookLocationDetail> UpdateAsync(BookLocationDetail obj)
        {
            throw new NotImplementedException();
        }

        public Task<BookLocationDetail> UpdateBookAsync(string id, BookLocationDetailDto model)
        {
            throw new NotImplementedException();
        }
    }
}

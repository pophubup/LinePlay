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
    public class PurchaseUserRepository : IPurchaseUserRepository
    {
        private readonly IMongoCollection<PurchaseUser> _books;

        public PurchaseUserRepository(IMongoDatabase database)
        {
            _books = database.GetCollection<PurchaseUser>(MongoCollectionNames.PurchaseUser);
        }
        public async Task AddAsync(PurchaseUser obj)
        {

            await _books.InsertOneAsync(obj);
        }
        public async Task<string> CreateBulkPurchaseUserAsync(List<PurchaseUserDto> models)
        {
            var t = models.Select(y => new PurchaseUser() { Contact = y.Contact, DetailID = y.DetailID, Name = y.Name, PurchaseDate = y.PurchaseDate });
           await  _books.InsertManyAsync(t);
            return String.Join(",", t.Select(y => y._Id)) ;
        }
        public async Task<string> CreatePurchaseUserAsync(PurchaseUserDto model)
        {
            PurchaseUser bookLocationDetail = new PurchaseUser()
            {
                DetailID = model.DetailID,
                Name = model.Name,
                Contact = model.Contact,
                PurchaseDate = model.PurchaseDate,
            };
            await AddAsync(bookLocationDetail);
            return bookLocationDetail._Id;
        }

        public Task DeleteAsync(Expression<Func<PurchaseUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public IQueryable<PurchaseUser> GetAll()
        {
            return _books.AsQueryable();
        }

        public Task<BookLocationDetail> GetPurchaseUserByIdAsync(string DetailID)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseUser> GetSingleAsync(Expression<Func<PurchaseUser, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseUser> UpdateAsync(PurchaseUser obj)
        {
            throw new NotImplementedException();
        }
    }
}

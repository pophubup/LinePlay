using MogoDBClient.Contract.DataService;
using MogoDBClient.Contract.Repositories;
using MogoDBClient.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MogoDBClient.Core.Services
{
    public class DataService : IDataService
    {
        private readonly MongoContext _dbContext;

        public DataService(MongoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IBookRepository Books => new BookRepository(_dbContext.Database);
        public IBookDetailRepository BookDetail => new BookDetailRepository(_dbContext.Database);
        public IPurchaseUserRepository PurchaseUser => new PurchaseUserRepository(_dbContext.Database);
    }
}

using MogoDBClient.Contract.Repositories;
using MogoDBClient.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MogoDBClient.Contract.DataService
{
    public interface IDataService
    {
        public IBookRepository Books { get; }
        public IBookDetailRepository BookDetail { get; }
        public IPurchaseUserRepository PurchaseUser { get; }
    }
}

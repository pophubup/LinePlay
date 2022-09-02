using MogoDBClient.Contract.DTO;
using MogoDBClient.Contract.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MogoDBClient.Contract.Repositories
{
    public interface IBookDetailRepository : _IRepository<BookLocationDetail>
    {
        Task<BookLocationDetail> GetBookDetailByIdAsync(string bookDetId);

        Task<string> CreateBookAsync(BookLocationDetailDto model);

       

    }
}

using MogoDBClient.Contract.DTO;
using MogoDBClient.Contract.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MogoDBClient.Contract.Repositories
{
    public interface IPurchaseUserRepository : _IRepository<PurchaseUser>
    {
        Task<BookLocationDetail> GetPurchaseUserByIdAsync(string DetailID);

        Task<string> CreatePurchaseUserAsync(PurchaseUserDto model);
        Task<string> CreateBulkPurchaseUserAsync(List<PurchaseUserDto> model);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MogoDBClient.Contract.DTO
{
    public class CreateOrUpdateBookDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ISBN { get; set; }
        public double Price { get; set; }
        public string AuthorName { get; set; }
    }

    public class BookLocationDetailDto
    {
        public string BookID { get; set; }
        public string StoreName { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public List<PurchaseUserDto> purchaseUsers { get; set; }
    }
    public class PurchaseUserDto
    {
        public string DetailID { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}

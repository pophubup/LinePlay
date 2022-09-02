using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MogoDBClient.Contract.Entities
{
    public class PurchaseUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        public string DetailID { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public DateTime PurchaseDate { get; set; }

    }
}

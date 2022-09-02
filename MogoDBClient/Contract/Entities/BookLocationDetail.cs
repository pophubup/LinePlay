using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MogoDBClient.Contract.Entities
{
    public class BookLocationDetail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        public string BookID { get; set; }
        public string StoreName { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
    }
  
  
}

using Core.Arango;
using Core.Arango.Protocol;
using Core.Arango.Modules;
using Newtonsoft.Json.Linq;
using Core.Arango.Linq;
using Newtonsoft.Json;

namespace ArangoDBClient
{
    public static class ArangoDBService
    {
        public static IServiceCollection AddArangoDBServices(this IServiceCollection service)
        {
           // var arango = new ArangoContext(@"");
            //bool CreateDBSuccess = arango.Database.CreateAsync("mydb").GetAwaiter().GetResult();

            #region Category
            //arango.Collection.CreateAsync("mydb", new ArangoCollection
            //{
            //    Name = "myCategory",
            //    Type = ArangoCollectionType.Document,
            //    KeyOptions = new ArangoKeyOptions
            //    {
            //        Type = ArangoKeyType.Padded,
            //        AllowUserKeys = false
            //    }
            //}).GetAwaiter().GetResult();
            //var cat = arango.Document.CreateManyAsync("mydb", "mycategory", new list<category>() {
            //   new category()
            //   {
            //         categoryname = "肉類"
            //   },
            //   new category()
            //   {
            //        categoryname ="蛋奶類"
            //   },
            //   new category()
            //   {
            //       categoryname ="蔬果類"
            //   }

            //}).Getawaiter().Getresult();
            #endregion

            #region Store
            //arango.Collection.CreateAsync("mydb", new ArangoCollection
            //{
            //    Name = "myStore",
            //    Type = ArangoCollectionType.Document,
            //    KeyOptions = new ArangoKeyOptions
            //    {
            //        Type = ArangoKeyType.Padded,
            //        AllowUserKeys = false
            //    }
            //}).GetAwaiter().GetResult();
            //var rangestroe = Enumerable.Range(0, 1000).Select((x , i)=> new Store()
            //{
            //     Quantity = x * 10,
            //     StoreName = $"我是店{x}",
            //     StoreAddr = $"{x}{x}{x}路{i}{i}號"
            //});
            //var prod = arango.Document.CreateManyAsync("mydb", "myStore", rangestroe).GetAwaiter().GetResult();
            #endregion

            #region Products
            //var target = arango.Query<myStore>("mydb").ToListAsync().GetAwaiter().GetResult().Select(y => new
            //{
            //    Key = y.Key,
            //    StoreName = y.StoreName,
            //    Index = int.Parse(y.StoreName.Replace('我', ' ').Replace('是', ' ').Replace('店', ' ').Trim()),
            //    StoreAddr = y.StoreAddr,
            //    Quantity = y.Quantity
            //});
            //var target2 = arango.Query<myCategory>("mydb").ToListAsync().GetAwaiter().GetResult();
            //List<myProduct> products = new List<myProduct>();
            //for (int i = 1; i <= 5; i++)
            //{

            //    var currentDatas = Enumerable.Range(0, 1000).Skip(i * 200 - 200).Take(200);
            //    var final = target.Where(u => currentDatas.Any(y => y == u.Index)).Select(x => new myStore() { Quantity = x.Quantity, StoreAddr = x.StoreAddr, StoreName = x.StoreName, Key = x.Key });
            //    var index = new Random().Next(0, 2);
            //    if (i < 3)
            //    {
            //        products.Add(new myProduct()
            //        {
            //            PrdouctName = Guid.NewGuid().ToString(),
            //            CategoryID = "000000000098c6ce",
            //            Strores = final.Select(x => x.Key).ToList()
            //        });
            //    }
            //    if (i == 3)
            //    {
            //        products.Add(new myProduct()
            //        {
            //            PrdouctName = Guid.NewGuid().ToString(),
            //            CategoryID = "000000000098c6cf",
            //            Strores = final.Select(x => x.Key).ToList()
            //        });
            //    }

            //    if (i > 3)
            //    {
            //        products.Add(new myProduct()
            //        {
            //            PrdouctName = Guid.NewGuid().ToString(),
            //            CategoryID = "000000000098c6d0",
            //            Strores = final.Select(x => x.Key).ToList()
            //        });
            //    }



            //}
            //arango.Collection.CreateAsync("mydb", new ArangoCollection
            //{
            //    Name = "myProduct",
            //    Type = ArangoCollectionType.Document,
            //    KeyOptions = new ArangoKeyOptions
            //    {
            //        Type = ArangoKeyType.Padded,
            //        AllowUserKeys = false
            //    }
            //}).GetAwaiter().GetResult();
            //var prod = arango.Document.CreateManyAsync("mydb", "myProduct", products).GetAwaiter().GetResult();
            #endregion

            return service;
        }
    }
    public class ArangoDBBase
    {
        [JsonProperty("_key")]
        public string Key { get; set; } = null;
    }
    public class myProduct 
    {
        public string PrdouctName { get; set; }
        public string CategoryID { get; set; }
        public List<string> Strores { get; set; }

    }
    public class myCategory : ArangoDBBase
    {
        public string CategoryName { get; set; }
    }

    public class myStore : ArangoDBBase
    {
        public string StoreName { get; set; }   
        public string StoreAddr { get; set; }
        public int Quantity { get; set; }
    }
}
using Microsoft.Extensions.DependencyInjection;
using MogoDBClient.Contract.DataService;
using MogoDBClient.Core.Services;
using MogoDBClient.Core;
using Microsoft.Extensions.Configuration;
using MogoDBClient.Contract;

namespace MogoDBClient
{
    public static class MogoDBInstance
    {
        public static IServiceCollection AddMogoDBClient(this IServiceCollection service)
        {
            //service.AddSingleton<MongoContext>();

            //service.AddScoped<IDataService, DataService>();

            //var main = new MogoDBClient.Contract.DTO.CreateOrUpdateBookDto()
            //{
            //    AuthorName = "!23",
            //    Description = "ㄟㄟ",
            //    ISBN = "45667",
            //    Name = "fffff",
            //    Price = double.Parse("123.44")
            //};
            //service.BuildServiceProvider().GetService<MongoContext>().Database.DropCollection("Books");
            //service.BuildServiceProvider().GetService<MongoContext>().Database.DropCollection("PurchaseUser");
            //service.BuildServiceProvider().GetService<MongoContext>().Database.DropCollection("BookLocationDetail");
            //var id = service.BuildServiceProvider().GetService<IDataService>().Books.CreateBookAsync(main).GetAwaiter().GetResult();
            //var detail = new MogoDBClient.Contract.DTO.BookLocationDetailDto()
            //{
            //    BookID = id,
            //    Address = "GGg路123號",
            //    Area = "XXX區",
            //    City = "uuu市",
            //    StoreName = "嗚嗚嗚分店",
            //    purchaseUsers = new List<Contract.DTO.PurchaseUserDto>()
            //    {
            //         new Contract.DTO.PurchaseUserDto()
            //         {
            //              Contact = "uuu",
            //              Name = "cccc",
            //              PurchaseDate = DateTime.Now
            //         },
            //             new Contract.DTO.PurchaseUserDto()
            //         {
            //              Contact = "uuu",
            //              Name = "cccc",
            //              PurchaseDate = DateTime.Now,
                         
            //         } 
            //    }
                
            //};
            //var yid = service.BuildServiceProvider().GetService<IDataService>().BookDetail.CreateBookAsync(detail).GetAwaiter().GetResult();
            //service.BuildServiceProvider().GetService<IDataService>().PurchaseUser.CreateBulkPurchaseUserAsync(detail.purchaseUsers.Select(y => new Contract.DTO.PurchaseUserDto() { 
            
            // Contact = y.Contact,
            // Name = y.Name,
            //  DetailID = yid,
            //  PurchaseDate = DateTime.Now
            
            //}).ToList()).GetAwaiter().GetResult();
  
            return service;
        }
    }
}

// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Linq.Expressions;
using StackExchange.Redis;

Console.WriteLine("Hello, World!");
ConnectionMultiplexer multiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
IDatabase db = multiplexer.GetDatabase();
ISubscriber sub = multiplexer.GetSubscriber();

sub.Subscribe("messages", (channel, message) => {
    Console.WriteLine((string)message);
});
for (int i = 0; i < 10; i++)
{
    multiplexer.GetDatabase().PublishAsync("messages", $"{i} Hello World! -----{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff")}");

}

ServiceCollection serviceCollection = new ServiceCollection();
serviceCollection.AddScoped<Iepo<myModel>, mymy>();
Console.ReadLine();

public interface Iepo<T> where T : class
{
    public IQueryable<T> GetAll<TProperty>(Expression<Func<T, TProperty>> expression );
    public IQueryable<T> GetAll();
}
class mymy : Iepo<myModel>
{
    Test test = new Test();
  
    IQueryable<myModel> Iepo<myModel>.GetAll<subData>(Expression<Func<myModel, subData>> navigationPropertyPath)
    {
        return test.myModels.Include(navigationPropertyPath);
    }

    IQueryable<myModel> Iepo<myModel>.GetAll()
    {
        return test.myModels;
    }
}
class subData
{
    public string ID { get; set; }
    public string Name { get; set; }    
    public myModel myModel { get; set; }
}
class myModel
{
    public Guid ID { get => Guid.NewGuid(); }
    
    public string Name { get; set; }

    public ICollection<subData> subDatas { get => new List<subData>()
    {
         new subData()
         {
              Name = "123cccc",
              ID = "123"
         },
         new subData()
         {
             ID = "455bbb",
             Name = "455"
         }

    };
    }
}

class Test : DbContext
{
    public DbSet<myModel> myModels;
    public DbSet<subData>  subData;
}
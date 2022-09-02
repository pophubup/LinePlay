using ArangoDBClient;
using GooglePlaceDetailClient;
using GoogleTextSerachClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.MSSqlServer;
using System.Configuration;
using System.Reflection;
using System.Linq.Expressions;
using MogoDBClient.Contract;
using MogoDBClient;
using MogoDBClient.Contract.Repositories;
using MogoDBClient.Contract.Entities;
using MogoDBClient.Core;
using MogoDBClient.Contract.DataService;
using TemplateEngine.Docx;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLineMessageApiClient();
builder.Services.AddArangoDBServices();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var ddd = builder.Configuration.GetSection("Serilog");
var logDB = "data source=gcpdb.goodarc.com,1433;initial catalog=DBLogTest;user id=taian;Password=yjo494SU3taian;MultipleActiveResultSets=True;";
var sinkOpts = new MSSqlServerSinkOptions { TableName = "Logs123", AutoCreateSqlTable= true , };
var columnOpts = new ColumnOptions();
var mongodb = new MongoClient("mongodb://localhost:27017");
builder.Services.Configure<DatabaseSettings>(
       builder.Configuration.GetSection("MongoConnection"));
builder.Services.AddMogoDBClient();

Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            //.ReadFrom.Configuration(builder.Configuration)
        //    .WriteTo.MSSqlServer(
        //connectionString: logDB,
        //sinkOptions: sinkOpts,
        //columnOptions: columnOpts,
        //appConfiguration: builder.Configuration
        //   )
            .WriteTo.Console()
           // .WriteTo.File(new CompactJsonFormatter(), $"{DateTime.Now.ToString("yyyyMMddhhmmss")}.json")
            .WriteTo.Seq("http://localhost:5341") //
             //.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200/")) { 
             //   AutoRegisterTemplate = true,
             //   TypeName = null,
                
             //    FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
             //    IndexFormat = $"lol-{DateTime.UtcNow:yyyy-MM}",
             //   ModifyConnectionSettings = x => x.BasicAuthentication("elastic", "changeme"),
             //})
            .CreateLogger();
Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));


builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/POST", (HttpContext httpContext) => { return app.Services.GetService<ILineBot>()?.RunAsync(httpContext); }).WithName("POST");
app.MapPost("/StockQuery", (HttpContext httpContext) => {
   
    for (int i = 0; i < 1000; i++)
    {
        if (i % 2 == 0)
        {
            Log.Information("{Value}{Query}", "test" + i, httpContext.Request.Path.ToString());
        }
        else
        {
            Log.Error("{Exception}{Message}", typeof(DbUpdateException), new DbUpdateException("wrong").Message);
        }
      
    }
 
   
    return "";
}).WithName("StockQuery");

app.Run();

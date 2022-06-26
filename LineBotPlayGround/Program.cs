using GooglePlaceDetailClient;
using GoogleTextSerachClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLineMessageApiClient();
builder.Configuration.AddJsonFile(@"C:\Users\Yohoo\Desktop\key.json");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/POST",   ( HttpContext httpContext ) => { return app.Services.GetService<ILineBot>()?.RunAsync(httpContext); }).WithName("POST");


app.Run();

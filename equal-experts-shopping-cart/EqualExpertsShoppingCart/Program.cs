using System.Configuration;
using CartModels;
using EqualExpertsShoppingCartAbstract;
using EqualExpertsShoppingCartImplementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen();
builder.Services.Configure<TaxSettings>(builder.Configuration.GetSection("TaxSettings"));
builder.Services.AddSingleton<ICartsManager, CartsManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
}

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();

app.Run();

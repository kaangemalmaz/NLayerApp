using Microsoft.EntityFrameworkCore;
using NLayerApp.Core.Repositories;
using NLayerApp.Core.Repository;
using NLayerApp.Core.Services;
using NLayerApp.Core.UnitOfWorks;
using NLayerApp.Repository;
using NLayerApp.Repository.Repositories;
using NLayerApp.Repository.UnitofWorks;
using NLayerApp.Service.Mappings.AutoMapper;
using NLayerApp.Service.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


//Configuration
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); //E�er generic ise a�ma kapama i�areti gerekli unutma generic oldu�u i�in b�yle
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); //E�er 1 den fazla tip alsayd� <,> virg�ll� olmal�yd�.
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddAutoMapper(typeof(MapProfile));



builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
     {
         //options.MigrationsAssembly("NLayerApp.Repository") => hardcoded iyi de�ildir.
         option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name); //=>tip g�venli hale getiriyoruz.
     });
});



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

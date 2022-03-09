using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayerApp.API.Extensions.Middlewares;
using NLayerApp.API.Filters;
using NLayerApp.API.Modules;
using NLayerApp.Repository;
using NLayerApp.Service.Mappings.AutoMapper;
using NLayerApp.Service.Validations.FluentValidation;
using System.Reflection;



var builder = WebApplication.CreateBuilder(args);

//Configuration
// Add services to the container.

//bu ProductDtoValidator �n bulundu�u assemblydeki t�m validatorlar� al anlam�na geliyor.
//builder.Services.AddControllers().AddFluentValidation(x=>x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());

//e�er bir �ey global ise buraya tan�mlan�r bu filter t�m controllera tan�mlanm�� oldu �uanda.
builder.Services.AddControllers(opt => opt.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());

builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    //filter d���ndaki model stateleri bask�la demektir.
    opt.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); //E�er generic ise a�ma kapama i�areti gerekli unutma generic oldu�u i�in b�yle
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); //E�er 1 den fazla tip alsayd� <,> virg�ll� olmal�yd�.
//builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
//builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddScoped<IProductService, ProductService>();
//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//builder.Services.AddScoped<ICategoryService, CategoryService>();


//filterlar i�in �zel olabilir.
builder.Services.AddAutoMapper(typeof(MapProfile));
//e�er bir filter i�inde constr. bir servis ge�iyorsan bunu tan�mlamak zorundas�nd�r unutma!
builder.Services.AddScoped(typeof(NotFoundFilter<>));



builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
     {
         //options.MigrationsAssembly("NLayerApp.Repository") => hardcoded iyi de�ildir.
         option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name); //=>tip g�venli hale getiriyoruz.
     });
});


builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//run i�erdi�i i�in yukarlarda olmas� �nemlidir.
app.UseCustomException();

app.UseAuthorization();

app.MapControllers();

app.Run();

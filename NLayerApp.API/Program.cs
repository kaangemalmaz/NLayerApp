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

//bu ProductDtoValidator ýn bulunduðu assemblydeki tüm validatorlarý al anlamýna geliyor.
//builder.Services.AddControllers().AddFluentValidation(x=>x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());

//eðer bir þey global ise buraya tanýmlanýr bu filter tüm controllera tanýmlanmýþ oldu þuanda.
builder.Services.AddControllers(opt => opt.Filters.Add(new ValidateFilterAttribute())).AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());

builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    //filter dýþýndaki model stateleri baskýla demektir.
    opt.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); //Eðer generic ise açma kapama iþareti gerekli unutma generic olduðu için böyle
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); //Eðer 1 den fazla tip alsaydý <,> virgüllü olmalýydý.
//builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
//builder.Services.AddScoped<IProductRepository, ProductRepository>();
//builder.Services.AddScoped<IProductService, ProductService>();
//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//builder.Services.AddScoped<ICategoryService, CategoryService>();


//filterlar için özel olabilir.
builder.Services.AddAutoMapper(typeof(MapProfile));
//eðer bir filter içinde constr. bir servis geçiyorsan bunu tanýmlamak zorundasýndýr unutma!
builder.Services.AddScoped(typeof(NotFoundFilter<>));



builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
     {
         //options.MigrationsAssembly("NLayerApp.Repository") => hardcoded iyi deðildir.
         option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name); //=>tip güvenli hale getiriyoruz.
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

//run içerdiði için yukarlarda olmasý önemlidir.
app.UseCustomException();

app.UseAuthorization();

app.MapControllers();

app.Run();

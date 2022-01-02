using Common.Auth.Core.Configuration;
using Common.Auth.Core.Models;
using Common.Auth.Core.Repositories;
using Common.Auth.Core.Services;
using Common.Auth.Core.UnitOfWork;
using Common.Auth.Data;
using Common.Auth.Data.Repositories;
using Common.Auth.Service.Services;
using Common.Shared.Configuration;
using Common.Shared.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped(typeof(IService<,>), typeof(GenericService<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServer"),
        sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("Common.Auth.Data");
        }
        );
});

builder.Services.AddIdentity<UserApp, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequireNonAlphanumeric = false;

}).AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();


builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOptions"));
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));


var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();
builder.Services.AddCustomTokenAuth(tokenOptions);



builder.Services.AddControllers().AddFluentValidation(options =>
{
    //Program class ýnýn bulunduðu assambly i içindeki tüm AbstractValidator larý bulur.
    options.RegisterValidatorsFromAssemblyContaining<Program>();
});

//Validasyon için kendi yaptýðýmýz ortak sýnýfý kullandýk.
builder.Services.UseCustomValidationResponse();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseCustomException();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Run();

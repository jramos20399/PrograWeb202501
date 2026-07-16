using BackEnd.Services.Implementations;
using BackEnd.Services.Interfaces;
using DAL.Implementations;
using DAL.Interfaces;
using Entities.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region Serilog

builder.Logging.ClearProviders();
builder.Host.UseSerilog((ctx,lc   )=> lc
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day) 
    .MinimumLevel.Error()
    );

#endregion

#region BD


builder.Services.AddDbContext<NorthWindContext>(options =>
                    options.UseSqlServer(
                        builder
                        .Configuration
                        .GetConnectionString("DefaultConnection")
                        ));

builder.Services.AddDbContext<AuthDBContext>(options =>
                    options.UseSqlServer(
                        builder
                        .Configuration
                        .GetConnectionString("DefaultConnection")
                        ));
#endregion

#region Identity


builder.Services.AddIdentityCore<IdentityUser>()
             .AddRoles<IdentityRole>()
            .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("una")
            .AddEntityFrameworkStores<AuthDBContext>()
            .AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 5;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;

});
#endregion

#region  JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

})

    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });



#endregion

#region DI

builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryDAL, CategoryDAL>();
builder.Services.AddScoped<ISupplierDAL, SupplierDAL>();
builder.Services.AddScoped<IProductDAL, ProductDAL>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

builder.Services.AddScoped<ITokenService, TokenService>();  

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();

using External.IsbnSoapClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nubelity.API.Middlewares;
using Nubelity.Application.Configurations;
using Nubelity.Application.Interfaces;
using Nubelity.Application.Services;
using Nubelity.Infrastructure.Persistence;
using Nubelity.Infrastructure.Persistence.Repositories;
using Nubelity.Infrastructure.Services;
using System.ServiceModel;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseNpgsql(connectionString)
);

// JWT configuration
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSection);

var jwtSettings = jwtSection.Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

//DI
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<INormalizationService, NormalizationService>();
builder.Services.AddScoped<IIsbnSoapService, SoapIsbnService>();
builder.Services.AddHttpClient<ICoverImageService, CoverImageService>();

builder.Services.AddScoped<SBNServiceSoapTypeClient>(sp =>
{
    var binding = new BasicHttpBinding();
    var endpoint = new EndpointAddress("http://webservices.daehosting.com/services/isbnservice.wso");
    return new SBNServiceSoapTypeClient(binding, endpoint);
});

builder.Services.AddScoped<IIsbnSoapService, SoapIsbnService>();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IAuthService, AuthService>();





var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    await SeedData.InitializeAsync(db);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseErrorHandlingMiddleware();

app.UseAuthorization();

app.MapControllers();

app.Run();

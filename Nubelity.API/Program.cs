
using Microsoft.EntityFrameworkCore;
using Nubelity.API.Middlewares;
using Nubelity.Application.Interfaces;
using Nubelity.Infrastructure.Persistence;
using Nubelity.Infrastructure.Persistence.Repositories;

namespace Nubelity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<LibraryDbContext>(options =>
                options.UseNpgsql(connectionString)
            );



            //DI

            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            var app = builder.Build();

            

            // Configure the HTTP request pipeline.
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
        }
    }
}

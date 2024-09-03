using Biblioteca.Backend.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace Biblioteca.Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(x => x.JsonSerializerOptions
                .ReferenceHandler = ReferenceHandler.IgnoreCycles);

            //builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=ConexionSQL"));
            builder.Services.AddDbContext<DataContext>(o =>
            {
                o.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSQL"));
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddTransient<SeedDb>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var key = builder.Configuration["Jwt:key"];

            builder.Services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(key!))
                };
            });

            builder.Services.AddHttpClient();

            var app = builder.Build();

            SeedData(app);
            void SeedData(WebApplication app)
            {
                IServiceScopeFactory scopedFactory = app.Services.GetService<IServiceScopeFactory>();
                using (IServiceScope scope = scopedFactory.CreateScope())
                {
                    SeedDb service = scope.ServiceProvider.GetService<SeedDb>();
                    service.SeedAsync().Wait();
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
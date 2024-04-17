using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Middlewares;
using Models.Entities;
using QuestPDF.Infrastructure;
using Repositories;
using Repositories.Interfaces;
using Repositories.Utils;
using Repositories.Utils.PasswordHasher;
using Services;
using Services.Interfaces;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Utils.Middleware;

namespace AcademyGestionGeneral
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IDistrictService, DistrictService>();
            services.AddTransient<IDistrictRepository, DistrictRepository>();
            services.AddTransient<IServiceService, ServiceService>();
            services.AddTransient<IServiceRepository, ServiceRepository>();
            services.AddTransient<IPasswordHasher, PasswordHasher>();

            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddDbContext<ManagementServiceContext>(options =>

               options.UseSqlServer(Environment.GetEnvironmentVariable("dbConnectionString"))
            );

            DotNetEnv.Env.Load();

            var jwtIssuer = Environment.GetEnvironmentVariable("Jwt_Issuer");
            var jwtAudience = Environment.GetEnvironmentVariable("Jwt_Audience");
            var jwtKey = Environment.GetEnvironmentVariable("Jwt_Key");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(config =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Users General Management",
                    Description = "Esta app conlleva a la gestión de usuarios."
                });
                config.IncludeXmlComments(xmlPath);

                // JWT
                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Bearer Token",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            services.AddAutoMapper(typeof(AutoMapperProfiles));

            services.AddCors(options =>
            {
                options.AddPolicy("Policy",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Management");
                    c.DefaultModelsExpandDepth(1);

                    // Habilitar la interfaz de autorización de Swagger
                    c.InjectStylesheet("/swagger-ui/custom.css"); // Opcional: Personalizar el estilo
                    c.DisplayOperationId();
                    c.EnableFilter();
                    c.EnableDeepLinking();
                    c.EnableValidator();
                    c.DocExpansion(DocExpansion.None); // Opcional: Configura la expansión de la documentación
                });
            }

            // Configurando licencia de QuestPDF:
            QuestPDF.Settings.License = LicenseType.Community;

            app.UseCors("Policy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseMiddleware<LogRequestMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
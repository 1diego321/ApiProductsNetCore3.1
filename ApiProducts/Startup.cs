using ApiProducts.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApiProducts.Repositories.IRepository;
using ApiProducts.Repositories;
using ApiProducts.Services;
using ApiProducts.Services.IService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using ApiProducts.Controllers;

namespace ApiProducts
{
    public class Startup
    {
        private const string _DEFAULTCONNECTION = "DefaultConnection";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Controllers
            services.AddControllers();

            //DbContext
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString(_DEFAULTCONNECTION)));

            //Repositories
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();

            //Services
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISubCategoryService, SubCategoryService>();

            //AutoMapper
            services.AddAutoMapper(typeof(Extensions.ApplicationMapper));

            //JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Secret").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            //CORS
            services.AddCors();

            //Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(nameof(ApplicationUserController), new OpenApiInfo //Param1 = GroupName
                {
                    Title = nameof(ApplicationUserController), // Titulo
                    Version = "1",
                    Description = "Controlador de los usuarios de la aplicación.",
                    Contact = new OpenApiContact
                    {
                        Email = "1diego321@gmail.com",
                        Name = "Luis Diego Solis Camacho",
                        Url = new Uri("https://github.com/1diego321")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                });

                options.SwaggerDoc(nameof(AccessController), new OpenApiInfo()
                {
                    Title = nameof(AccessController),
                    Version = "1",
                    Description = "Controlador de acceso de usuarios (login)",
                    Contact = new OpenApiContact
                    {
                        Email = "1diego321@gmail.com",
                        Name = "Luis Diego Solis Camacho",
                        Url = new Uri("https://github.com/1diego321")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                });

                options.SwaggerDoc(nameof(ProductController), new OpenApiInfo()
                {
                    Title = nameof(ProductController),
                    Version = "1",
                    Description = "Controlador de los productos",
                    Contact = new OpenApiContact
                    {
                        Email = "1diego321@gmail.com",
                        Name = "Luis Diego Solis Camacho",
                        Url = new Uri("https://github.com/1diego321")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                });

                options.SwaggerDoc(nameof(CategoryController), new OpenApiInfo()
                {
                    Title = nameof(CategoryController),
                    Version = "1",
                    Description = "Controlador de las categorias",
                    Contact = new OpenApiContact
                    {
                        Email = "1diego321@gmail.com",
                        Name = "Luis Diego Solis Camacho",
                        Url = new Uri("https://github.com/1diego321")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                });

                options.SwaggerDoc(nameof(SubCategoryController), new OpenApiInfo()
                {
                    Title = nameof(SubCategoryController),
                    Version = "1",
                    Description = "Controlador de las sub categorias",
                    Contact = new OpenApiContact
                    {
                        Email = "1diego321@gmail.com",
                        Name = "Luis Diego Solis Camacho",
                        Url = new Uri("https://github.com/1diego321")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Autenticacion JWT (Bearer)",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });

            //ModelState AutoValidation
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/" + nameof(AccessController) + "/swagger.json", nameof(AccessController)); //Param2 = Definition Name
                    options.SwaggerEndpoint("/swagger/" + nameof(ApplicationUserController) + "/swagger.json", nameof(ApplicationUserController)); //Param2 = Definition Name
                    options.SwaggerEndpoint("/swagger/" + nameof(ProductController) + "/swagger.json", nameof(ProductController)); //Param2 = Definition Name
                    options.SwaggerEndpoint("/swagger/" + nameof(CategoryController) + "/swagger.json", nameof(CategoryController)); //Param2 = Definition Name
                    options.SwaggerEndpoint("/swagger/" + nameof(SubCategoryController) + "/swagger.json", nameof(SubCategoryController)); //Param2 = Definition Name

                    options.RoutePrefix = "SwaggerDocumentation";
                });
            }

            app.UseHttpsRedirection();

            //Swagger
            

            app.UseRouting();

            //Authorization & Authentication
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Cors
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        }
    }
}

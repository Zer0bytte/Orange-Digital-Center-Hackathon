using AuthenticationStudent;
using BusinessLogic;
using BusinessLogic.AdminLogic;
using BusinessLogic.AdminLogic.Classes;
using BusinessLogic.AdminLogic.Interfaces;
using BusinessLogic.StudentLogic.Classes;
using BusinessLogic.StudentLogic.Interfaces;
using Domains;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentAuthManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODC_Students
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("NIUXq4yk8GTGelfusivAHfSrdPEHvXWMXA1Khnlhnpk=")),
                    ValidateIssuer = false,
                    ValidateAudience = false,

                };
            });

            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<IStudentAuthStructure, StudentAuthStructure>();
            services.AddScoped<IAdminAuthentication, AdminAuthentication>();
            services.AddScoped<IStudentsStructure, StudentsStructure>();
            services.AddScoped<ICategoriesStructure, CategoriesStructure>();
            services.AddScoped<ICoursesStructure, CoursesStructure>();
            services.AddScoped<IExamsStructure, ExamsStructure>();
            services.AddScoped<IQuestionsStructure, QuestionsStructure>();
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IEnrollsStructure, EnrollsStructure>();
            services.AddScoped<IStudentCoursesStructure, StudentCoursesStructure>();

            services.AddDbContext<ODCCoursesManagmentContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                   );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using System.Text;
using evisa.api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using UsedBooks.Data;
using UsedBooks.Features.User;
using UsedBooks.UnitOfWork;
namespace UsedBooks;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        var dbConnectionString = builder.Configuration.GetConnectionString("dbConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(dbConnectionString));
        
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        builder.Services.AddControllers();
        
        builder.Services.AddCors(o => o.AddDefaultPolicy(option => 
            option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
        
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddAuthorization();
        /*
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        */
        
        var appSettingsSection = builder.Configuration.GetSection("AppSettings");
        builder.Services.Configure<AppSettings>(appSettingsSection);
        
        builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"] ?? throw new InvalidOperationException("Set Jwt__SigningKey in the environment."))),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        builder.Services.AddScoped<IUnitOfWork,Data.UnitOfWork>();
        /*builder.Services.AddScoped<IApplicationsRepository, ApplicationsRepository>();*/

        return builder.Build();
    }


    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        // app.MapRazorPages()
        //     .RequireAuthorization();

        return app;
    }
}
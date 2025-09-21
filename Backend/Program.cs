using System.Text.Json.Serialization;
using Backend.Data;
using Backend.Exceptions;
using Backend.FluentValidation;
using Backend.Interfaces;
using Backend.Models;
using Backend.Repository;
using Backend.Services;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Запуск приложения");
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddOpenApi();

builder.Services.AddProblemDetails(options =>
{
    options.IncludeExceptionDetails = (_, _) => false;

    options.Map<CustomExceptions.InternalServerErrorException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
    
    options.Map<CustomExceptions.InvalidNewUserModelException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
    
    options.Map<CustomExceptions.UserAlreadyExistsException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
    
    options.Map<CustomExceptions.UnauthorizedPasswordException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
    
    options.Map<CustomExceptions.UnauthorizedUsernameException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
    
    options.Map<CustomExceptions.UnauthorizedException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
    
    options.Map<CustomExceptions.WorkspaceNotFoundException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
    
    options.Map<CustomExceptions.RoomNotFoundException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
    
    options.Map<CustomExceptions.BookingNotFoundException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
    
    options.Map<CustomExceptions.BookingCanNotBeCancelledException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
    
    options.Map<CustomExceptions.BookingArleadyExistsException>(ex => new ProblemDetails
    {
        Type = ex.Type,
        Title = ex.Title,
        Status = (int)ex.StatusCode,
        Detail = ex.Message
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme =
        options.DefaultChallengeScheme = 
            options.DefaultForbidScheme = 
                options.DefaultScheme =
                    options.DefaultSignInScheme =
                        options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
        )
    };
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Coworkify", Version = "v1" });
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            []
        }
    });
});

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(c =>
        c.UseNpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddHangfireServer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("v1/swagger.json", "Coworkify V1"));
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseProblemDetails();
app.UseMiddleware<ValidationExceptionMiddleware>();

app.MapControllers();

app.UseHangfireDashboard("/hangfire");

RecurringJob.AddOrUpdate<IBookingRepository>(
    "complete-bookings",
    i => i.CompleteBookings(),
    Cron.Minutely);

app.Run();
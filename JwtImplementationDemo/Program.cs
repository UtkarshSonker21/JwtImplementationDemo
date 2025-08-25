using AutoMapper;
using JwtImplementationDemo.Dto;
using JwtImplementationDemo.Entities;
using JwtImplementationDemo.Helper.Filters;
using JwtImplementationDemo.Helper.Middlewares;
using JwtImplementationDemo.Interface;
using JwtImplementationDemo.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// db-connection string
// Scaffold-DbContext "Name=DefaultConnection" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force
builder.Services.AddDbContext<ParcticeContext>(options => 
     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => 
{
    // Override the default 401/403 responses
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            // Skip the default response
            context.HandleResponse();

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return context.Response.WriteAsJsonAsync(new ErrorModel
            {
                //StatusCode = 401,
                Message = "You are not authorized to access this resource.",
                ErrorCode = "UNAUTHORIZED"
            });
        },
        OnForbidden = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return context.Response.WriteAsJsonAsync(new ErrorModel
            {
                //StatusCode = 403,
                Message = "You do not have permission to access this resource.",
                ErrorCode = "FORBIDDEN"
            });
        }
    };

    var key = builder.Configuration["JWT:Key"];
    if (string.IsNullOrEmpty(key))
    {
        throw new InvalidOperationException("JWT:Key is missing in configuration");
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

// Automapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Register in DI container, 1 instance per request.
builder.Services.AddScoped<PerformanceMonitorFilter>();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    // MVC itself creates the filter once, so it behaves like a singleton (shared across all requests).
    //options.Filters.Add<PerformanceMonitorFilter>();

    // MVC asks DI for the filter per request, so it respects the lifetime you registered (Scoped in your case).
    options.Filters.AddService<PerformanceMonitorFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add Jwt Swagger Integration
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });

});


// Repositories & Services
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// custom middleware
app.UseMiddleware<CustomExceptionMiddleware>();


app.UseHttpsRedirection();


// for authentication & authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();




// 1. Add Authentication

//“Hey, our app will use JWT for authentication.”
//DefaultAuthenticateScheme ? decides how to check who the user is (we use JWT here).
//DefaultChallengeScheme ? decides what to do when user is not authenticated (again, JWT).
//Think of this like: “Whenever someone enters, check their JWT token as the identity proof.”


// 2. Add JwtBearer
// We’re telling ASP.NET Core how to validate the token that the client sends.
// Example: Check if it’s expired, check if signature is correct, etc.


// 3.Token Validation Parameters
// ValidateIssuer = false
// “Don’t check who created the token.”
// Normally you set this to true if you want to only trust tokens from your server.

// ValidateAudience = false
// “Don’t check who the token is intended for.”
// Normally useful if your API is only for a specific app/client.

// ValidateLifetime = true
// Check if the token has expired or not.
// If expired ? reject request.

// ValidateIssuerSigningKey = true
// Check if the token’s signature matches with the server’s secret key.
// Prevents tampering with token.

// IssuerSigningKey = new SymmetricSecurityKey(...)
// This is your secret key
// It’s used to sign the token when created and to verify the token when received.
// Only the server knows this key.
// Think of this secret key like the stamp on a movie ticket
// If it’s missing or fake, the ticket is invalid.
// Encoding.UTF8.GetBytes(...) = “Turn my string secret key into a binary form (bytes) that the encryption algorithm can actually use.”
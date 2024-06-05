using BiteStation.Domain.Abstractions;
using BiteStation.Domain.Models;
using BiteStation.Domain.Settings;
using BiteStation.Infrastructure.Data;
using BiteStation.Infrastructure.Services;
using BiteStation.Presentation.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

{
    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAutoMapper(typeof(Program).Assembly);

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IImageService, ImageService>();
    builder.Services.AddScoped<IEmailService, EmailService>();
    builder.Services.AddScoped<IUserService, UserService>();

    builder.Services.AddRouting(options => options.LowercaseUrls = true);

    // Database connection config
    var conStr = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(conStr));

    // Rate limiter
    builder.Services.AddRateLimiter(options =>
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        options.AddFixedWindowLimiter("fixed", options =>
        {
            options.Window = TimeSpan.FromSeconds(5);
            options.QueueLimit = 10;
            options.PermitLimit = 50;
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; //this is the default
        });
    });

    // Auth config
    builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequiredLength = 5;
    }).AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

    var jwtOptionsSection = builder.Configuration.GetSection("JwtOptions");
    builder.Services.Configure<JwtOptions>(jwtOptionsSection);
    var jwtOptions = jwtOptionsSection.Get<JwtOptions>();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidAudience = jwtOptions.Audience,
            ValidIssuer = jwtOptions.Issuer,
            RequireExpirationTime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
            ValidateIssuerSigningKey = true
        };
    });
}

var app = builder.Build();

{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        //app.ApplyMigrations();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseRateLimiter();

    app.UseStaticFiles();

    app.MapControllers();
}

app.Run();

using BiteStation.Domain.Abstractions;
using BiteStation.Domain.Models;
using BiteStation.Infrastructure;
using BiteStation.Infrastructure.Data;
using BiteStation.Presentation.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

{
    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAutoMapper(typeof(Program).Assembly);

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IImageService, ImageService>();

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
    builder.Services.AddAuthorization();
    builder.Services.AddAuthentication()
        .AddBearerToken(IdentityConstants.BearerScheme);

    builder.Services.AddIdentityCore<User>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddApiEndpoints();
}

var app = builder.Build();

{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.ApplyMigrations();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseRateLimiter();

    app.UseStaticFiles();

    app.MapControllers();

    app.MapIdentityApi<User>();
}

app.Run();

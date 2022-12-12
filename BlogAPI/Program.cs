using BlogAPI.Core.Application.Common.Helpers;
using BlogAPI.Core.Application.Common.Interfaces;
using BlogAPI.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.AspNetCore;
using BlogAPI.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());
});

builder.Services.AddControllers()
    .AddFluentValidation();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddTransient<ISlugHelper, SlugHelper>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("BlogDb"));
    }
);

builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.AddGlobalErrorHandler();

app.MapControllers();

app.Run();

using ExpenseTracker;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Controller;
using ExpenseTracker.Infrastructure;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog;

/*
 * +-------+         +---------+          +-------------+          +-------+
   | User  |         | Frontend|          | ASP.NET Core|          |SQLite |
   |       |         |         |          | Backend     |          |DB     |
   +-------+         +---------+          +-------------+          +-------+
   |                 |                      |                      |
   |1.Enter Expense  |                      |                      |
   |---------------->|                      |                      |
   |                 |                      |                      |
   |                 |2. HTTP POST Request  |                      |
   |                 |--------------------->|                      |
   |                 |                      |                      |
   |                 |                      |3. Save Expense via   |
   |                 |                      |   Entity Framework   |
   |                 |                      |--------------------->|
   |                 |                      |                      |
   |                 |                      |    4. Expense Saved  |
   |                 |                      |<---------------------|
   |                 |                      |                      |
   |                 |   5. HTTP 200 OK     |                      |
   |                 |<---------------------|                      |
   |                 |                      |                      |
   |6. Confirmation  |                      |                      |
   |<----------------|                      |                      |
   |                 |                      |                      |
 */
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();
builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlite("Data Source=app.db");
});

builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Services.AddCors(o =>
{
    o.AddPolicy("OnlyUs", builder =>
    {
        builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:4200");
    });
});

// Logging
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
builder.Services.AddSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.RegisterExpenseEndpoints();

app.UseCors("OnlyUs");

app.UseExceptionHandler(_ => { });

// Add our middleware
/*
 * Browser           Middleware           Controller/Action
   |                 |                  |
   |---Request------>|                  |
   |                 |---Request------->|
   |                 |<--Response-------|
   |                 X                  X
   |<--Response------|                  |
   |                 |                  |
   |---Request------>|                  |
   |                 |---Request------->|
   |                 |<--Exception------|
   |<--Error Object--|                  |
 */
app.Use(async (context, next) =>
{
    var logger = app.Logger;
    logger.LogDebug("Before Request with Uri '{Uri}'", context.Request.GetDisplayUrl());
    await next(context);
    logger.LogDebug("After Request with Uri '{Uri}'", context.Request.GetDisplayUrl());
});

app.UseHttpsRedirection();

app.MapHub<ExpenseHub>("expensehub");

app.Run();
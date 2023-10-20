using ExpenseTracker;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ExpenseTracker.Controller;
using ExpenseTracker.Infrastructure;

/*
 * +-------+         +---------+          +-------------+          +-------+
   | User  |         | Angular |          | ASP.NET Core|          |SQLite |
   |       |         | Frontend|          | Backend     |          |DB     |
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

// Tell OpenAPI how to handle DateOnly
builder.Services.AddSwaggerGen(options => options.MapType<DateOnly>(() => new OpenApiSchema()
{
    Type = "string",
    Format = "date"
}));

builder.Services.AddSignalR();
builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlite("Data Source=app.db");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.RegisterExpenseEndpoints();

// Add our middleware
app.UseGlobalExceptionHandling();
app.UseUnitOfWorkMiddleware();

app.UseHttpsRedirection();

app.MapHub<ExpenseHub>("/expensehub");

app.Run();
using ExpenseTracker;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ExpenseTracker.Controller;
using ExpenseTracker.Infrastructure;

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
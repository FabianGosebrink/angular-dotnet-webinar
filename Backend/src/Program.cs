using ExpenseTracker;
using ExpenseTracker.Controller;
using ExpenseTracker.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddUserServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("singleton", (SingletonGuidService s1, SingletonGuidService s2) =>
{
    return $"{s1.MyGuid} + {s2.MyGuid}";
});
app.MapGet("scoped", (ScopedGuidService s1, ScopedGuidService s2) =>
{
    return $"{s1.MyGuid} + {s2.MyGuid}";
});
app.MapGet("transient", (TransientGuidService s1, TransientGuidService s2) =>
{
    return $"{s1.MyGuid} + {s2.MyGuid}";
});

app.UseHttpsRedirection();

app.UseDisplayUrlConsole();
app.UseCorrelationId();

app.UseUserEndpoints();
app.UseWeatherforecastEndpoints();

app.Run();


public class UserService : IUserService
{
    public UserService()
    {
    }
    
    public void AbsolutNichts()
    {
    }

    public void B()
    {
        
    }

    private void C()
    {
    }
}

public interface IUserService
{
    void AbsolutNichts();
}

public class RegistrationService
{
    private IUserService _userService;

    public RegistrationService(IUserService userService)
    {
        this._userService = userService;
    }

    public string Greeting()
    {
        _userService.AbsolutNichts();
        return "Hello";
    }
}

public class UserRequest
{
    public string Name { get; set; }
}

public partial class Program;
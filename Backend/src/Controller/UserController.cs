namespace ExpenseTracker.Controller;

public static class UserController
{
    public static void UseUserEndpoints(this WebApplication app)
    {
        var group = app
            .MapGroup("user")
            .WithOpenApi();
        
        group.MapPost("{name}", (UserRequest user, string name) =>
        {
            return $"Hello {user.Name}";
        });

        group.MapGet("", (RegistrationService service) =>
        {
            return service.Greeting();
        });
    }
}
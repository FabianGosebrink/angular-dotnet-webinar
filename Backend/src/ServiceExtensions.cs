namespace ExpenseTracker;

public static class ServiceExtensions
{
    public static void AddUserServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<RegistrationService>();
        services.AddSingleton<SingletonGuidService>();
        services.AddScoped<ScopedGuidService>();
        services.AddTransient<TransientGuidService>();
    }
}
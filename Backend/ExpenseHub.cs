using Microsoft.AspNetCore.SignalR;

namespace ExpenseTracker;

public class ExpenseHub : Hub
{
    public async Task NotifyBudgetExceeded(string user, string message)
    {
        await Clients.User(user).SendAsync("Notification", message);
    }
}
using Microsoft.AspNetCore.SignalR;

namespace ExpenseTracker;

/*
 * +--------+             +------------------+              +-------+
   | Client |             | Server (SignalR) |              | Data  |
   | (User) |             | Hub              |              | Layer |
   +--------+             +------------------+              +-------+
   |                          |                            |
   | 1. Connect to Hub        |                            |
   |------------------------->|                            |
   |                          |                            |
   | 2. Enter Expense         |                            |
   |------------------------->|                            |
   |                          |                            |
   |                          | 3. Check if Expense        |
   |                          |     exceeds limit          |
   |                          |--------------------------->|
   |                          |                            |
   |                          |     4. Exceeded?           |
   |                          |<---------------------------|
   |                          |                            |
   | 5. Notify Client via     |                            |
   |    SignalR               |                            |
   |<-------------------------|                            |
   |                          |                            |
 */
public class ExpenseHub : Hub
{
    public async Task NotifyBudgetExceeded(string user, string message)
    {
        await Clients.User(user).SendAsync("Notification", message);
    }
}
namespace ExpenseTracker;

public class SingletonGuidService
{
    public Guid MyGuid = Guid.NewGuid();
}

public class ScopedGuidService
{
    public Guid MyGuid = Guid.NewGuid();
}

public class TransientGuidService
{
    public Guid MyGuid = Guid.NewGuid();
}
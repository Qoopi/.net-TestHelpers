namespace Qoopi.Waiter;

public record WaitOptions
{
    public int Retries { get; set; }
    public TimeSpan TimeBetweenRetries { get; set; }
}
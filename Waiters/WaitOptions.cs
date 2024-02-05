namespace Waiters;

public record WaitOptions
{
    public int Retries { get; set; }
    public TimeSpan TimeBetweenRetries { get; set; }
}
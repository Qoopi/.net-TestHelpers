using BenchmarkDotNet.Attributes;
using static Qoopi.Waiter.WaitHelper;

namespace Qoopi.Waiter.Tests.Benchmark;

[MemoryDiagnoser]
[ExceptionDiagnoser]
public class WaiterBenchmark
{
    [Benchmark]
    public void CheckReturningErrors()
    {
        Wait<int?>(() => throw new Exception("Expected exception"), r => r == null);
    }

    [Benchmark]
    public void CheckReturningNulls()
    {
        Wait<int?>(() => null, r => r == null);
    }
}
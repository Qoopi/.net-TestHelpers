﻿using Microsoft.Extensions.Configuration;
using Polly;
using static Polly.Policy;

namespace Waiters;

public static class WaitHelper
{
    public static readonly WaitOptions Options;

    static WaitHelper()
    {
        Options = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("")}.json", false, true)
            .AddEnvironmentVariables()
            .Build()
            .GetSection(nameof(WaitOptions))
            .Get<WaitOptions>() ?? new WaitOptions
        {
            Retries = 10,
            TimeBetweenRetries = TimeSpan.FromSeconds(1)
        };
    }

    public static PolicyResult<T> Wait<T>(
        Func<T> func,
        Func<T, bool> repeatCondition)
    {
        return Wait(func, repeatCondition, Options.Retries, Options.TimeBetweenRetries);
    }

    public static PolicyResult<T> Wait<T>(
        Func<T> func,
        Func<T, bool> repeatCondition,
        int retryCount,
        TimeSpan timeBetweenRetries)
    {
        return Handle<Exception>()
            .OrResult(repeatCondition)
            .WaitAndRetry(retryCount, _ => timeBetweenRetries)
            .ExecuteAndCapture(func.Invoke);
    }

    public static async Task<PolicyResult<T>> WaitAsync<T>(
        Func<Task<T>> func,
        Func<T, bool> repeatCondition)
    {
        return await WaitAsync(func, repeatCondition, Options.Retries, Options.TimeBetweenRetries);
    }

    public static async Task<PolicyResult<T>> WaitAsync<T>(
        Func<Task<T>> func,
        Func<T, bool> repeatCondition,
        int retryCount,
        TimeSpan timeBetweenRetries)
    {
        return await Handle<Exception>()
            .OrResult(repeatCondition)
            .WaitAndRetryAsync(retryCount, _ => timeBetweenRetries)
            .ExecuteAndCaptureAsync(func.Invoke);
    }
}
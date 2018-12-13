using System;
using System.Threading.Tasks;

namespace StudentSystem.Infrastructure.RetryPolicy
{
    public interface IRetryPolicy
    {
        Task<TResult> ExecuteAsync<TResult, TException>(Func<Task<TResult>> action, int maxRetries)
            where TException : Exception;

        Task<TResult> ExecuteWithDelayAsync<TResult, TException>(Func<Task<TResult>> action, int maxRetries, TimeSpan delay)
            where TException : Exception;

        TResult Execute<TResult, TException>(Func<TResult> action, int maxRetries)
            where TException : Exception;

        TResult ExecuteWithDelay<TResult, TException>(Func<TResult> action, int maxRetries, TimeSpan delay)
            where TException : Exception;
    }
}

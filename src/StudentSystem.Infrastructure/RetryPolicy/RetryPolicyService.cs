using System;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using StudentSystem.Common.Logging;

namespace StudentSystem.Infrastructure.RetryPolicy
{
    public class RetryPolicyService : IRetryPolicy
    {
        public virtual Task<TResult> ExecuteWithDelayAsync<TResult, TException>(Func<Task<TResult>> action, int maxRetries, TimeSpan delay)
            where TException : Exception
        {
            var policy = SetupRetryPolicyWithDelayAsync<TException>(maxRetries, delay);
            return policy.ExecuteAsync(action);
        }
        public virtual Task<TResult> ExecuteAsync<TResult, TException>(Func<Task<TResult>> action, int maxRetries)
              where TException : Exception
        {
            var policy = SetupRetryPolicyAsync<TException>(maxRetries);
            return policy.ExecuteAsync(action);
        }

        public virtual TResult ExecuteWithDelay<TResult, TException>(Func<TResult> action, int maxRetries, TimeSpan delay)
            where TException : Exception
        {
            var policy = SetupRetryPolicyWithDelay<TException>(maxRetries, delay);
            return policy.Execute(action);
        }
        public virtual TResult Execute<TResult, TException>(Func<TResult> action, int maxRetries)
            where TException : Exception
        {
            var policy = SetupRetryPolicy<TException>(maxRetries);
            return policy.Execute(action);
        }

        //TODO Polly.Retry 
        private static Polly.Retry.RetryPolicy SetupRetryPolicyWithDelayAsync<T>(int maxRetries, TimeSpan delay) where T : Exception
        {
            return Policy.Handle<T>()
                 .WaitAndRetryAsync(maxRetries, count => delay, (exception, retryCount, context) =>
                 {
                     Log<Polly.Retry.RetryPolicy>.Error($"Transient error, retry count: {retryCount}", exception);
                 });
        }
        private static Polly.Retry.RetryPolicy SetupRetryPolicyAsync<T>(int maxRetries) where T : Exception
        {
            return Policy.Handle<T>()
                    .RetryAsync(maxRetries, (exception, retryCount, context) =>
                    {
                        Log<Polly.Retry.RetryPolicy>.Error($"Transient error, retry count: {retryCount}", exception);
                    });
        }

        private static Polly.Retry.RetryPolicy SetupRetryPolicyWithDelay<T>(int maxRetries, TimeSpan delay) where T : Exception
        {
            return Policy.Handle<T>()
                 .WaitAndRetry(maxRetries, count => delay, (exception, retryCount, context) =>
                 {
                     Log<Polly.Retry.RetryPolicy>.Error($"Transient error, retry count: {retryCount}", exception);
                 });
        }
        private static Polly.Retry.RetryPolicy SetupRetryPolicy<T>(int maxRetries) where T : Exception
        {
            return Policy.Handle<T>()
                .Retry(maxRetries, (exception, retryCount, context) =>
                {
                    Log<Polly.Retry.RetryPolicy>.Error($"Transient error, retry count: {retryCount}", exception);
                });
        }
    }
}

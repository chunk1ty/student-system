namespace StudentSystem.Common
{
    public class OperationStatus<T>
    {
        public OperationStatus(bool isSuccessful, T result)
        {
            IsSuccessful = isSuccessful;
            Result = result;
        }

        public OperationStatus(bool isSuccessful, string message)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }

        public bool IsSuccessful { get; }

        public string Message { get; }
       
        public T Result { get; }
    }

    public class SuccessStatus<T> : OperationStatus<T> 
    {
        public SuccessStatus(T result)
            : base(true, result)
        {
        }
    }

    public class FailureStatus<T> : OperationStatus<T> 
    {
        public FailureStatus(string message)
            : base(false, message)
        {
        }

    }
}

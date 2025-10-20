namespace Poc.BoardK8sApi.Shared.Results
{
    public sealed class Result<T> : Result
    {
        public T? Data { get; }

        private Result(T? data, bool isSuccess, string? msgError) : base(isSuccess, msgError)
            => Data = data;

        public static Result<T> Success(T value) => new(value, true, null);
        public static Result<T> Failure(string error) => new(default, false, error);
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public string? MsgError { get; }

        protected Result(bool isSuccess, string? msgError)
        {
            IsSuccess = isSuccess;
            MsgError = msgError;
        }

        public static Result Success() => new(true, null);
        public static Result Failure(string error) => new(false, error);
    }
}

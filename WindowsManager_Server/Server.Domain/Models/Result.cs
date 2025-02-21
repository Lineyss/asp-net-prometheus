namespace Server.Domain.Models
{
    public class Result<T>
    {
        public T? Object { get; }
        public Error Error { get; }
        public bool IsSuccess { get; }

        private Result(T? Object)
        {
            this.Object = Object;
            IsSuccess = true;
        }

        private Result(Error Error)
        {
            this.Error = Error ?? throw new Exception("Error is null");
            IsSuccess = false;
        }

        public static Result<T> Success(T? Object) => new(Object);
        public static Result<T> Fail(Error Error) => new(Error);
    }
}


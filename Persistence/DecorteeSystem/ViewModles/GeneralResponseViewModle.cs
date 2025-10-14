using Application.Dtos;

namespace DecorteeSystem.ViewModles
{
    public class GeneralResponseViewModle<T>
    {
        public static GeneralResponseViewModle<T> Success(T data) => new() { Data = data, IsSuccess = true, Message = string.Empty };

        public static GeneralResponseViewModle<T> Success() => new() { IsSuccess = true, Message = string.Empty };

        // Error constructors
        public static GeneralResponseViewModle<T> Fail(ErrorType type, string message)
            => new() { Error = type, IsSuccess = false, Message = message };

        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public ErrorType Error { get; set; }
        public string Message { get; set; }
    }
}

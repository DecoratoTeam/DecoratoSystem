using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class GeneralResponseDto<T>
    {
        public static GeneralResponseDto<T> Success(T data) 
            => new() { Data = data, IsSuccess = true, Message = string.Empty };

        public static GeneralResponseDto<T> Success() 
            => new() { IsSuccess = true, Message = string.Empty };

        public static GeneralResponseDto<T> Fail(ErrorType type, string message)
            => new() { Error = type, IsSuccess = false, Message = message };

        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public ErrorType Error { get; set; }
        public string Message { get; set; }
    }
}

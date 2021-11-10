using System;
using System.Collections.Generic;
using System.Text;

namespace PM.Common.CommonModels
{
    public class Result
    {
        public Result(int statusCode, bool isSuccess, string message)
        {
            StatusCode = statusCode;
            IsSuccess = isSuccess;
            Message = message;
        }

        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public static Result GetSuccessInstance() => new Result(0, true, "OK");
    }

    public class Result<T> : Result
    {
        public Result(int statusCode, bool isSuccess, string message, T data)
            : base(statusCode, isSuccess, message)
        {
            Data = data;
        }

        public T Data { get; set; }
        public static Result<T> GetSuccessInstance(T value) 
            => new Result<T>(0, true, "OK", value);
    }
}

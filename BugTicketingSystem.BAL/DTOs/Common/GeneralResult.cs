using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.BAL.DTOs.Common
{
    public class GeneralResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

        public Resulterror[] Errors { get; set; } = [];

        public static GeneralResult Success(string? message = null) => new()
        {
            IsSuccess = true,
            Message = message
        };
        public static GeneralResult Failure(params Resulterror[] errors) => new()
        {
            IsSuccess = false,
            Errors = errors
        };

        public static GeneralResult Failure(string message, string code = "GENERAL_ERROR") => new()
        {
            IsSuccess = false,
            Errors = [new Resulterror { Code = code, Message = message }]
        };
    }

    public class GeneralResult<T> : GeneralResult
    {
        public T? Data { get; set; }

        public static GeneralResult<T> Success(T data) => new()
        {
            IsSuccess = true,
            Data = data
        };

        public static new GeneralResult<T> Failure(string message, string code = "GENERAL_ERROR") => new()
        {
            IsSuccess = false,
            Errors = [new Resulterror { Code = code, Message = message }]
        };

        public static new GeneralResult<T> Failure(params Resulterror[] errors) => new()
        {
            IsSuccess = false,
            Errors = errors
        };
    }

    public class Resulterror
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}

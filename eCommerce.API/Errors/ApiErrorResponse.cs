using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace eCommerce.API.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }

        public ApiErrorResponse(int statusCode, string? errorMessage = null)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage ?? DefaultErrorMessage(statusCode);
        }

        private string DefaultErrorMessage(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                404 => "Page Not Found",
                500 => "Internal Server Error",
                _ => "Error"
            };
        }
    }
}
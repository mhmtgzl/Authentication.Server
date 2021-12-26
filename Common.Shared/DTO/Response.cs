using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Shared.DTO
{
    public class Response<T> where T : class
    {
        public int StatusCode { get; private set; }

        public T Data { get; private set; }

        public ErrorDto ErrorDto { get; private set; }


        [JsonIgnore]
        public bool IsSuccessful { get; set; }

        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default, StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Fail(ErrorDto error, int statusCode)
        {
            return new Response<T> { ErrorDto = error, StatusCode = statusCode, IsSuccessful = false };
        }

        public static Response<T> Fail(string errorMessage, int statusCode, bool isShow)
        {
            ErrorDto dto = new ErrorDto(errorMessage, isShow);

            return new Response<T> { ErrorDto = dto, StatusCode = statusCode, IsSuccessful = false };
        }
    }
}

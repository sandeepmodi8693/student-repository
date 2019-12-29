using System;

namespace Student.Web.Models
{
    public class BaseServiceResponse
    {
        public bool IsSuccessful { get; set; }
    }

    public class ServiceResponse<T> : BaseServiceResponse
    {
        public T Data { get; set; }
    }
}

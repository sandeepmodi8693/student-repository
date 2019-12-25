using System;

namespace Student.Models
{
    public class ServiceResponse
    {
        public bool IsSuccessful { get; set; }
        public object Data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Models.DTO.Application
{
    /// <summary>
    /// Default endpoints response model
    /// </summary>
    public class Response
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}

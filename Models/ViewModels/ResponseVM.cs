using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class ResponseVM
    {
        public int statusCode { get; set; }

        public string message { get; set; }

        public object? data { get; set; }
    }
}

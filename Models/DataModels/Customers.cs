using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DataModels
{
    public class Customers: Users
    {
        public long CustomerId { get; set; }
    }
}

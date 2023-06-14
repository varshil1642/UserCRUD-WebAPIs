using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DataModels
{
    public class Publishers : Users
    {
        public long PublisherId { get; set; }

        public string PublisherName { get; set; } = string.Empty;
    }
}

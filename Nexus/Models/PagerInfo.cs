using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nexus.Models
{
    public class PagerInfo
    {
        public int TotalPages { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}

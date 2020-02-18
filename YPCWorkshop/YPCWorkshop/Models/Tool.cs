using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YPCWorkshop.Models
{
    public class Tool
    {
        
        public int ToolId { get; set; }
        public string Name { get; set; }
        public int AssetNumber { get; set; }
        public string Brand { get; set; }
        public bool Active { get; set; }
        public bool Available { get; set; }


    }
}
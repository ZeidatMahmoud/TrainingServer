using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace androidapp.Models
{
    public class Images
    {
       
        public int id { get; set; }
        public string fileName { get; set; }
        public byte[] fileStore { get; set; }
        public string mimeType { get; set; }

    }
}

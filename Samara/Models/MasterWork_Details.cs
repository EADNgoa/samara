using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Samara
{
    public class MAsterWork_Details
    {
        public MasterWork Work { get; set; }
        public IEnumerable<MasterWorkDetails> WorkDets { get; set; }
    }
}
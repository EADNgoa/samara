using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Samara
{
    public class MasterWorkDetails
    {
        public int WorkDetailID { get; set; }
        public string WorkName { get; set; }
        public string UnitName { get; set; }
        public string ItemName { get; set; }
        public int Qty { get; set; }

        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
    }
}
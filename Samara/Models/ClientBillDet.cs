using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Samara
{
    public class ClientBillDet
    {
        public int CBillDetailID { get; set; }
        public int CBillID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int Qty { get; set; }
        public decimal UnitCostPrice { get; set; }
        public decimal UnitSellPrice { get; set; }
        public decimal TaxPerc { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Samara
{
    public class GetSupDet
    {
        public int SBillDetailID { get; set; }
        public int SupplierID { get; set; }
        public string Tdate { get; set; }

        public int SBillID { get; set; }
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public int QtyRec { get; set; }
        public int QtySold { get; set; }

    }
}
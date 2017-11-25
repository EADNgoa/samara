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
     
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public byte DebitCredit { get; set; }
        public byte BeforeTax { get; set; }
        public decimal TaxPerc { get; set; }

    }
}
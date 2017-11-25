using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Samara
{
    public class ClientDet
    {
        public int CBillID { get; set; }

        public int ClientID { get; set; }

        public string ClientName { get; set; }
        public DateTime Tdate { get; set; }
        public decimal RetentionPerc { get; set; }
        public decimal RetentionAmt { get; set; }
        public bool RetentionAmtIsPaid { get; set; }
        public decimal TaxPerc { get; set; }



    }
}
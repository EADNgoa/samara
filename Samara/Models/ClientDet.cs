using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Samara
{
    public class ClientDet
    {
        public int CBillID { get; set; }

        public int ClientID { get; set; }
        public string SiteName { get; set; }

        public string ClientName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Tdate { get; set; }

        public decimal RetentionPerc { get; set; }
        public decimal RetentionAmt { get; set; }
        public bool RetentionAmtIsPaid { get; set; }
        public decimal GrandTotalNoTax { get; set; }
        public decimal TaxAmt { get; set; }



        public decimal TaxPerc { get; set; }



    }
}
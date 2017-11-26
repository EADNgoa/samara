using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Samara
{
    [MetadataType(typeof(SuppDetMetadata))]
    public class SuppDet
    {
        public int SBillID { get; set; }

        public string SupplierName { get; set; }
        public string SiteName { get; set; }


        public DateTime Tdate { get; set; }
        public decimal TDSperc { get; set; }



    }
}
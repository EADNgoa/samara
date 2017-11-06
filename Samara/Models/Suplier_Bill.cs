using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Samara
{
    public class Suplier_Bill
    {
        public SuppDet SuplierDets { get; set; }
        public IEnumerable<SuppBillDet> SuplierBillDets { get; set; }
    }
}
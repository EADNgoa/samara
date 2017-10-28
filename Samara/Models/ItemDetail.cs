using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Samara
{
    public class ItemDetail
    {
        public int ItemID { get; set; }

        public string ItemName { get; set; }

        public string UnitName { get; set; }

        public decimal ReorderLevel{ get; set; }

        public decimal TaxPerc { get; set; }

    }
}
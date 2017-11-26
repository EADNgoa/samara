﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Samara
{
    public class BossTransDet
    {
        public int SiteTransID { get; set; }

        public DateTime Tdate { get; set; }

        public string Email { get; set; }

        public string SiteName { get; set; }

        public string SupplierName { get; set; }

        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }

        public int QtyAdded { get; set; }

        public string Remarks { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Samara
{
    public class StockSummaryDetails
    {
        public int ItemId { get; set; }

        public DateTime Tdate { get; set; }

        public string ItemName { get; set; }

        public int Qty { get; set; }

    }
}
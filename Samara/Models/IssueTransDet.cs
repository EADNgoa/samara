using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Samara
{
    public class IssueTransDet
    {
        public int SiteTransID { get; set; }

        public DateTime Tdate { get; set; }

        public string Email { get; set; }

        public string SiteName { get; set; }
        public string ClientName { get; set; }


        public string ItemName { get; set; }

        public int QtyRemoved { get; set; }

        public int QtyAdded { get; set; }


        public string Remarks { get; set; }


    }
}
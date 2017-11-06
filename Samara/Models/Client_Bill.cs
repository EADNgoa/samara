using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Samara
{
    public class Client_Bill
    {
        public ClientDet ClientDets { get; set; }
        public IEnumerable<ClientBillDet> ClientBillDets { get; set; }
    }
}
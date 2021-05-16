using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KembimValutor.Models
{
    public class Wallet
    {
        public int UserId { get; set; }
        public string Curr { get; set; }
        public double CurrVal { get; set; }
    }
}
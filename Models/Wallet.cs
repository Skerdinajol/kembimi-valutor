using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KembimValutor.Models
{
    public class Wallet
    {
        public int UserId { get; set; }
        public double Eur { get; set; }
        public double Usd { get; set; }
        public double Gbp { get; set; }
        public double All { get; set; }
    }
}
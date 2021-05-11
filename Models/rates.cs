using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KembimValutor.Models
{
    public class rates
    {
        public int RateId { get; set; }
        public string Curr1 { get; set; }
        public string Curr2 { get; set; }
        public float Rate { get; set; }
    }
}
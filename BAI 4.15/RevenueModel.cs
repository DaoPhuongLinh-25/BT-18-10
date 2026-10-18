using Bai_4_15;
using System;
using System.Collections.Generic;
using System.Text;


namespace Bai_4_15
{
    public class RevenueModel
    {
        public string TransactionId { get; set; }

        public DateTime TransactionDate { get; set; }

        public string Branch { get; set; }

        public string Description { get; set; }

        public decimal Revenue { get; set; }
    }
}

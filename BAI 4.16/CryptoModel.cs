using System;
using System.Collections.Generic;
using System.Text;

namespace Bai_4__16

{
    public class CryptoModel
    {
        public DateTime UpdateTime { get; set; }

        public decimal BTCPrice { get; set; }

        public decimal PriceChange { get; set; }

        public string Status { get; set; }

        public CryptoModel()
        {
            UpdateTime = DateTime.Now;
            BTCPrice = 65000m;
            PriceChange = 0m;
            Status = "Khoi tao";
        }

        public CryptoModel(decimal oldPrice, decimal newPrice)
        {
            UpdateTime = DateTime.Now;
            BTCPrice = newPrice;
            PriceChange = newPrice - oldPrice;

            if (PriceChange > 0)
                Status = "Tang";
            else if (PriceChange < 0)
                Status = "Giam";
            else
                Status = "Khong doi";
        }
    }
}

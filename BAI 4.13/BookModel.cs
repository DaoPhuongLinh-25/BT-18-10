using System;
using System.Collections.Generic;
using System.Text;

namespace BAI4_13
{
    public class BookModel
    {
        public string BookId { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string CoverPath { get; set; }

        public decimal TotalValue
        {
            get { return Quantity * Price; }
        }
    }
}

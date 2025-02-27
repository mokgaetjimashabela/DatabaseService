using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseService
{
    public class ShoppingCart
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public object ItemName { get; internal set; }
        public string Description { get; set; }
        public int ItemPrice { get; internal set; }

        [Ignore]
        public string FormattedPrice => $"R{ItemPrice.ToString("F2")}";

        // Property to calculate total price
        [Ignore]
        public double TotalPrice => ItemPrice * Quantity;

        // Property to format total price in Rands
        [Ignore]
        public string FormattedTotalPrice => $"R{TotalPrice.ToString("F2")}";
    }
}

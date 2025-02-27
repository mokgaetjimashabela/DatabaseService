using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.Threading.Tasks;

namespace DatabaseService
{
    public  class ShoppingItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string ItemName { get; set; }
        public double ItemPrice { get; set; }
        public string ItemDescription { get; set; }
        public int StockQuantity { get; set; }
        public string Name { get; internal set; }
        public double Price { get; internal set; }
        public int Stock { get; internal set; }
    }
}

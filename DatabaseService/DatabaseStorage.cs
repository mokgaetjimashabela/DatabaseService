using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DatabaseService
{
    public class DatabaseStorage
    {
        private SQLiteConnection _connection;
        private readonly string _dbPath;

        public DatabaseStorage(string dbPath)
        {
            _dbPath = dbPath;
            _connection = new SQLiteConnection(_dbPath);
            _connection.CreateTable<Profile>();
            _connection.CreateTable<ShoppingItem>();
            _connection.CreateTable<ShoppingCart>();

            // Initialize shopping items if none exist
            InitializeShoppingItems();
        }

        // Insert Profile Data
        public void SaveProfile(Profile profile)
        {
            _connection.Insert(profile);
        }

        // Insert Shopping Item
        public void SaveShoppingItem(ShoppingItem item)
        {
            _connection.Insert(item);
        }

        // Pre-populate Shopping Items if empty
        private void InitializeShoppingItems()
        {
            if (!_connection.Table<ShoppingItem>().Any()) // Check if the table has no items
            {
                var items = new List<ShoppingItem>
                {
                    new ShoppingItem { Name = "Bread", Price = 20.00, Stock = 10 },
                    new ShoppingItem { Name = "Milk", Price = 15.00, Stock = 8 },
                    new ShoppingItem { Name = "Eggs", Price = 30.00, Stock = 12 },
                    new ShoppingItem { Name = "Butter", Price = 40.00, Stock = 6 },
                    new ShoppingItem { Name = "Sugar", Price = 25.00, Stock = 9 }
                };

                _connection.InsertAll(items);
            }
        }

        // Get All Shopping Items
        public List<ShoppingItem> GetShoppingItems()
        {
            return _connection.Table<ShoppingItem>().ToList();
        }

        // Insert to Shopping Cart
        public void AddToShoppingCart(ShoppingCart cart)
        {
            var itemInCart = _connection.Table<ShoppingCart>()
                                         .Where(x => x.ProfileId == cart.ProfileId && x.ItemId == cart.ItemId)
                                         .FirstOrDefault();

            if (itemInCart != null)
            {
                // Update quantity if item is already in the cart
                itemInCart.Quantity += cart.Quantity;
                _connection.Update(itemInCart);
            }
            else
            {
                _connection.Insert(cart);
            }
        }

        // Get Shopping Cart Items by Profile
        public List<ShoppingCart> GetShoppingCartItems(int profileId)
        {
            return _connection.Table<ShoppingCart>()
                              .Where(x => x.ProfileId == profileId)
                              .ToList();
        }

        // Remove Item from Cart
        public void RemoveFromCart(int profileId, int itemId)
        {
            var item = _connection.Table<ShoppingCart>()
                                  .Where(x => x.ProfileId == profileId && x.ItemId == itemId)
                                  .FirstOrDefault();
            if (item != null)
            {
                _connection.Delete(item);
            }
        }

        internal void ClearCart(int profileId)
        {
            throw new NotImplementedException();
        }

        internal int CreateOrder(int profileId, List<ShoppingCart> cartItems)
        {
            throw new NotImplementedException();
        }

        internal void UpdateOrderStatus(int orderId, string v)
        {
            throw new NotImplementedException();
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.Threading.Tasks;

namespace DatabaseService
{
    public partial class ShoppingCartPage : ContentPage
    {
        private DatabaseStorage _databaseStorage;
        private int _profileId;
        private List<ShoppingCart> _cartItems;

        public ShoppingCartPage()
        {
            InitializeComponent();

            // Initialize database connection
            _databaseStorage = new DatabaseStorage(App.DatabasePath);

            // Get profile ID from logged-in user (you might want to pass this as a parameter)
            _profileId = 1;

            // Load cart items
            LoadCartItems();

            // Update the title to show the number of items
            UpdateTitle();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Refresh cart items when the page appears
            LoadCartItems();
        }

        private void LoadCartItems()
        {
            // Get shopping cart items from database
            _cartItems = _databaseStorage.GetShoppingCartItems(_profileId);

            // Set as the ListView's ItemSource
            shoppingCartListView.ItemsSource = _cartItems;

            // Update the title
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            int totalItems = _cartItems?.Sum(item => item.Quantity) ?? 0;
            Title = $"Shopping Cart ({totalItems} items)";
        }

        private void RemoveItemButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var cartItem = button?.BindingContext as ShoppingCart;

            if (cartItem != null)
            {
                // Remove the item from the database
                _databaseStorage.RemoveFromCart(cartItem.ProfileId, cartItem.ItemId);

                // Display confirmation
                DisplayAlert("Success", $"{cartItem.ItemName} removed from cart", "OK");

                // Refresh the cart items
                LoadCartItems();
            }
        }

        private async void ClearCartButton_Clicked(object sender, EventArgs e)
        {
            // Ask for confirmation
            bool answer = await DisplayAlert("Clear Cart", "Are you sure you want to clear your cart?", "Yes", "No");

            if (answer)
            {
                // Clear all items from the database
                _databaseStorage.ClearCart(_profileId);

                // Display confirmation
                await DisplayAlert("Cart Cleared", "Your shopping cart has been cleared", "OK");

                // Refresh the cart items
                LoadCartItems();
            }
        }

        private async void CheckoutButton_Clicked(object sender, EventArgs e)
        {
            if (_cartItems == null || _cartItems.Count == 0)
            {
                await DisplayAlert("Empty Cart", "Your cart is empty", "OK");
                return;
            }

            // Calculate total price
            double total = _cartItems.Sum(item => item.ItemPrice * item.Quantity);

            // Display checkout message
            bool proceed = await DisplayAlert("Checkout", $"Your total is R{total.ToString("F2")}. Proceed to payment?", "Yes", "No");

            if (proceed)
            {
                // Process the order
                bool success = await ProcessOrder();

                if (success)
                {
                    // Clear the cart after successful purchase
                    _databaseStorage.ClearCart(_profileId);

                    // Display confirmation
                    await DisplayAlert("Order Placed", "Thank you for your purchase!", "OK");

                    // Refresh the cart items
                    LoadCartItems();

                    // Navigate back to the shopping list
                    await Navigation.PopAsync();
                }
            }
        }

        private async Task<bool> ProcessOrder()
        {
            try
            {
                // Create order record in the database
                int orderId = _databaseStorage.CreateOrder(_profileId, _cartItems);

                // Here you would implement actual payment processing
                await Task.Delay(1000); // Simulating payment processing

                // Update order status to paid
                _databaseStorage.UpdateOrderStatus(orderId, "Paid");

                return true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to process order: {ex.Message}", "OK");
                return false;
            }
        }

        private async void ViewOrderHistoryButton_Clicked(object sender, EventArgs e)
        {
            // Navigate to order history page
            // await Navigation.PushAsync(new OrderHistoryPage(_profileId));
            await DisplayAlert("Coming Soon", "Order history feature is coming soon", "OK");
        }
    }
}

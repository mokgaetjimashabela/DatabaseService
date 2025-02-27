using System.Collections.ObjectModel;

namespace DatabaseService;

public partial class ShoppingListPage : ContentPage
{
    // Observable collection that will automatically update the UI when modified
    public ObservableCollection<ShoppingItem> ShoppingItems { get; set; }
    public static object CartItems { get; internal set; }

    public ShoppingListPage(Profile profile)
    {
        InitializeComponent();

        // Initialize the collection
        ShoppingItems = new ObservableCollection<ShoppingItem>();

        // Set as the ListView's ItemSource
        shoppingListView.ItemsSource = ShoppingItems;

        // Load the items (normally this would come from a database)
        LoadShoppingItems();
    }

    private void LoadShoppingItems()
    {
        // Add sample items with prices in Rands (replace this with your actual data loading logic)
        ShoppingItems.Add(new ShoppingItem { ItemName = "Apple", ItemPrice = 19.99, StockQuantity = 50 });
        ShoppingItems.Add(new ShoppingItem { ItemName = "Banana", ItemPrice = 9.99, StockQuantity = 40 });
        ShoppingItems.Add(new ShoppingItem { ItemName = "Orange", ItemPrice = 24.99, StockQuantity = 30 });
        ShoppingItems.Add(new ShoppingItem { ItemName = "Milk", ItemPrice = 39.99, StockQuantity = 20 });
        ShoppingItems.Add(new ShoppingItem { ItemName = "Bread", ItemPrice = 29.99, StockQuantity = 15 });

        // You would typically get this data from a database service
        // Example: ShoppingItems = await _databaseService.GetShoppingItemsAsync();
    }

    private void AddToCartButton_Clicked(object sender, EventArgs e)
    {
        // Get the button that was clicked
        var button = (Button)sender;

        // Get the parent view cell (contains the Entry control)
        var viewCell = (ViewCell)button.Parent.Parent;

        // Get the binding context (the ShoppingItem)
        var item = (ShoppingItem)viewCell.BindingContext;

        // Find the entry control within the cell to get the quantity
        var entry = viewCell.View.FindByName<Entry>("quantityEntry");

        if (int.TryParse(entry.Text, out int quantity) && quantity > 0)
        {
            // Add to cart logic here with Rand currency format
            DisplayAlert("Added to Cart", $"Added {quantity} {item.ItemName}(s) to cart - Total: R{(quantity * item.ItemPrice).ToString("F2")}", "OK");

            // Clear the entry
            entry.Text = string.Empty;
        }
        else
        {
            DisplayAlert("Invalid Quantity", "Please enter a valid quantity", "OK");
        }
    }
}

// Model class for shopping items
public class ShoppingItems
{
    public string ItemName { get; set; }
    public double ItemPrice { get; set; }

    // Property to format price in Rands
    public string FormattedPrice => $"R{ItemPrice.ToString("F2")}";

    public int StockQuantity { get; set; }
    public int Quantity { get; set; }
}

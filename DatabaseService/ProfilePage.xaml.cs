using Microsoft.Maui;
using SQLite;
namespace DatabaseService;

public partial class ProfilePage : ContentPage
{
    private DatabaseStorage _databaseStorage;

    public ProfilePage()
    {
        InitializeComponent();
        _databaseStorage = new DatabaseStorage(App.DatabasePath); // Initialize with the database path
    }

    private async void OnSaveProfileButtonClicked(object sender, EventArgs e)
    {
        // Logic to save profile here
        var profile = new Profile
        {
            Name = nameEntry.Text,
            Surname = surnameEntry.Text,
            Email = emailEntry.Text,
            Gender = genderPicker.SelectedItem.ToString(),
            Bio = bioEditor.Text
        };

        // Save profile to database
        _databaseStorage.SaveProfile(profile);

        // Navigate to the ShoppingListPage after saving the profile
        await Navigation.PushAsync(new ShoppingListPage(profile));
    }
}
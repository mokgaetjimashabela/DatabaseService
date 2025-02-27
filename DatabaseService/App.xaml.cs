using SQLite;
namespace DatabaseService
{
    public partial class App : Application
    {
        public static string DatabasePath { get; internal set; }
        public App()
        {
            InitializeComponent();

            DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "shoppingApp.db3");


            MainPage = new NavigationPage(new ProfilePage());
        }

        
    }
}

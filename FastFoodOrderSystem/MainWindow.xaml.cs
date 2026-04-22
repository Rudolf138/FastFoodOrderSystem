using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FastFoodOrderSystem
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<FoodItem> AllItems { get; set; }
        public ObservableCollection<FoodItem> FilteredItems { get; set; }
        public ObservableCollection<CartItem> Cart { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            
            AllItems = new ObservableCollection<FoodItem> {
                new FoodItem("Gastro Burger", "Burger", "Hovädzie mäso, brioche", 9.50m),
                new FoodItem("Pizza Margherita", "Pizza", "Paradajky, mozzarella", 7.90m),
                new FoodItem("Coke Zero", "Drink", "0.5l fľaša", 1.50m),
                new FoodItem("Veggie Pizza", "Pizza", "Zelenina, pesto", 8.50m),
                new FoodItem("Cheese Burger", "Burger", "Double cheddar", 10.20m),
                new FoodItem("Domáca Limonáda", "Drink", "Citrón, mäta", 2.50m)
            };

            FilteredItems = new ObservableCollection<FoodItem>(AllItems);
            Cart = new ObservableCollection<CartItem>();
            this.DataContext = this;
        }

        public decimal Total => Cart.Sum(i => i.Price * i.Quantity);

        
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (UsernameInput.Text != "" && PasswordInput.Password != "")
            {
                LoginGrid.Visibility = Visibility.Collapsed;
                MainAppGrid.Visibility = Visibility.Visible;
            }
            else MessageBox.Show("Zadajte meno a heslo!");
        }

        private void Register_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Účet úspešne vytvorený! Môžete sa prihlásiť.");
        private void Logout_Click(object sender, RoutedEventArgs e) { MainAppGrid.Visibility = Visibility.Collapsed; LoginGrid.Visibility = Visibility.Visible; }

        
        private void FilterCategory_Click(object sender, RoutedEventArgs e)
        {
            string cat = (sender as Button).Tag.ToString();
            FilteredItems.Clear();
            var items = cat == "All" ? AllItems : AllItems.Where(i => i.Category == cat);
            foreach (var i in items) FilteredItems.Add(i);
        }

        private void NavRestaurants_Click(object sender, RoutedEventArgs e) => FilterCategory_Click(sender, e);

       
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var food = (sender as Button).DataContext as FoodItem;
            var inCart = Cart.FirstOrDefault(c => c.Name == food.Name);
            if (inCart != null) inCart.Quantity++;
            else Cart.Add(new CartItem { Name = food.Name, Price = food.Price, Quantity = 1 });
            OnPropertyChanged(nameof(Total));
        }

        private void Qty_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var item = btn.DataContext as CartItem;
            if (btn.Tag.ToString() == "plus") item.Quantity++;
            else { if (item.Quantity > 1) item.Quantity--; else Cart.Remove(item); }
            OnPropertyChanged(nameof(Total));
        }

        
        private async void Pay_Click(object sender, RoutedEventArgs e)
        {
            if (Cart.Count == 0) return;

            StatusText.Text = "Spracovávam platbu... ";
            await Task.Delay(2000); 

            StatusText.Text = "Jedlo sa pripravuje... ";
            Cart.Clear();
            OnPropertyChanged(nameof(Total));

           
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += (s, ev) =>
            {
                StatusText.Text = "Jedlo je HOTOVÉ! ";
                MessageBox.Show("Vaša objednávka je pripravená na vyzdvihnutie! Prajeme dobrú chuť.", "PrimePlate NOTIFIKÁCIA");
                timer.Stop();
            };
            timer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string n) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }

    public class FoodItem
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public FoodItem(string n, string c, string d, decimal p) { Name = n; Category = c; Description = d; Price = p; }
    }

    public class CartItem : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        private int _q;
        public int Quantity { get => _q; set { _q = value; OnPropertyChanged("Quantity"); } }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string n) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
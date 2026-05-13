//using System;
//using System.Collections.ObjectModel;
//using System.ComponentModel;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Threading;

//namespace FastFoodOrderSystem
//{
//    public partial class MainWindow : Window, INotifyPropertyChanged
//    {
//        // Simple login only (no user store or SMTP notifications)
//        public ObservableCollection<FoodItem> AllItems { get; set; }
//        public ObservableCollection<FoodItem> FilteredItems { get; set; }
//        public ObservableCollection<CartItem> Cart { get; set; }
//        public ObservableCollection<string> Restaurants { get; set; }
//        public ObservableCollection<string> RecentOrders { get; set; }
//        public int LoyaltyPoints { get; set; }
//        private string _selectedRestaurant = "All";
//        public string SelectedRestaurant
//        {
//            get => _selectedRestaurant;
//            set { _selectedRestaurant = value; ApplyFilters(); OnPropertyChanged(nameof(SelectedRestaurant)); }
//        }
//        private string _selectedCategory = "All";
//        private bool _isDiscountFilter = false;
//        public ObservableCollection<string> Cuisines { get; set; }
//        private string _selectedCuisine = "All";
//        public string SelectedCuisine
//        {
//            get => _selectedCuisine;
//            set { _selectedCuisine = value; ApplyFilters(); OnPropertyChanged(nameof(SelectedCuisine)); }
//        }

//        public MainWindow()
//        {
//            InitializeComponent();


//            AllItems = new ObservableCollection<FoodItem> {
//                // Gastro Grill (American)
//                new FoodItem("Gastro Burger", "Burger", "Hovädzie mäso, brioche", 9.50m, "Gastro Grill", "American", 10m),
//                new FoodItem("Onion Rings", "Sides", "Chrumkavé cibuľové krúžky", 3.50m, "Gastro Grill", "American", 0m),
//                new FoodItem("Gourmet Fries", "Sides", "Hranolky s parmezánom", 4.00m, "Gastro Grill", "American", 0m),
//                new FoodItem("BBQ Bacon Burger", "Burger", "Slanina, BBQ omáčka", 11.00m, "Gastro Grill", "American", 15m),
//                new FoodItem("Chicken Wings", "Sides", "Grilované krídelká s BBQ", 6.50m, "Gastro Grill", "American", 0m),

//                // Pizzeria Roma (Italian)
//                new FoodItem("Pizza Margherita", "Pizza", "Paradajky, mozzarella, bazalka", 7.90m, "Pizzeria Roma", "Italian", 0m),
//                new FoodItem("Veggie Pizza", "Pizza", "Zelenina, pesto", 8.50m, "Pizzeria Roma", "Italian", 5m),
//                new FoodItem("Calzone", "Pizza", "Plnená pizza so šunkou a syrom", 9.20m, "Pizzeria Roma", "Italian", 0m),
//                new FoodItem("Four Cheese", "Pizza", "Mix štyroch syrov", 9.00m, "Pizzeria Roma", "Italian", 0m),
//                new FoodItem("Tiramisu", "Dessert", "Tradičné tiramisu", 4.20m, "Pizzeria Roma", "Italian", 0m),

//                // Green Garden (Vegetarian)
//                new FoodItem("Quinoa Salad", "Salad", "Quinoa, avokádo, cherry paradajky", 6.50m, "Green Garden", "Vegetarian", 10m),
//                new FoodItem("Falafel Wrap", "Wrap", "Domáci falafel, tahini", 5.90m, "Green Garden", "Middle Eastern", 0m),
//                new FoodItem("Vegan Bowl", "Main", "Mix sezónnej zeleniny a hummus", 7.50m, "Green Garden", "Vegetarian", 0m),
//                new FoodItem("Grilled Halloumi", "Sides", "Grilovaný syr halloumi", 5.00m, "Green Garden", "Vegetarian", 0m),
//                new FoodItem("Smoothie Boost", "Drink", "Ovocné smoothie", 3.50m, "Green Garden", "International", 0m),

//                // Sushi House (Asian)
//                new FoodItem("Salmon Nigiri", "Sushi", "Čerstvý losos", 4.50m, "Sushi House", "Asian", 0m),
//                new FoodItem("California Roll", "Sushi", "Krab, avokádo, uhorka", 6.00m, "Sushi House", "Asian", 0m),
//                new FoodItem("Miso Soup", "Soup", "Tradičná japonská polievka", 2.20m, "Sushi House", "Asian", 0m),
//                new FoodItem("Spicy Tuna Roll", "Sushi", "Tuniak, pikantná omáčka", 6.80m, "Sushi House", "Asian", 5m),
//                new FoodItem("Tempura", "Sides", "Krevety tempura", 5.50m, "Sushi House", "Asian", 0m),

//                // PrimePlate chain (Drinks & Desserts)
//                new FoodItem("Coke Zero", "Drink", "0.5l fľaša", 1.50m, "PrimePlate", "International", 0m),
//                new FoodItem("Domáca Limonáda", "Drink", "Citrón, mäta", 2.50m, "PrimePlate", "International", 0m),
//                new FoodItem("Chocolate Cake", "Dessert", "Domáci čokoládový koláč", 3.80m, "PrimePlate", "International", 20m),
//                new FoodItem("Cheesecake", "Dessert", "Smotanový cheesecake", 4.00m, "PrimePlate", "International", 0m),
//                new FoodItem("Espresso", "Drink", "Intenzívna káva", 1.80m, "PrimePlate", "International", 0m),

//                // Thai Corner (Asian)
//                new FoodItem("Pad Thai", "Noodles", "Ryžové rezance, krevety, arašidy", 8.90m, "Thai Corner", "Asian", 5m),
//                new FoodItem("Green Curry", "Curry", "Korenisté zelené curry s tofu", 9.00m, "Thai Corner", "Asian", 0m),
//                new FoodItem("Drunken Noodles", "Noodles", "Pikantné širšie rezance", 8.30m, "Thai Corner", "Asian", 0m),
//                new FoodItem("Spring Rolls", "Sides", "Zeleninové jarné rolky", 3.20m, "Thai Corner", "Asian", 0m),
//                new FoodItem("Thai Iced Tea", "Drink", "Sladený čaj s mliekom", 2.20m, "Thai Corner", "Asian", 0m),

//                // Spice India (Indian)
//                new FoodItem("Tandoori Chicken", "Main", "Marinované kuracie kúsky", 10.50m, "Spice India", "Indian", 0m),
//                new FoodItem("Butter Naan", "Sides", "Domáci indický chlieb", 1.80m, "Spice India", "Indian", 0m),
//                new FoodItem("Chana Masala", "Main", "Cícerové kari", 7.20m, "Spice India", "Indian", 10m),
//                new FoodItem("Biryani", "Main", "Aromatická ryža s korením", 9.50m, "Spice India", "Indian", 0m),
//                new FoodItem("Mango Lassi", "Drink", "Osviežujúci jogurtový nápoj", 2.80m, "Spice India", "Indian", 0m),

//                // Additional restaurants
//                new FoodItem("Taco Al Pastor", "Taco", "Tradičné mexické taco s bravčovým", 2.50m, "Taco Fiesta", "Mexican", 0m),
//                new FoodItem("Beef Burrito", "Main", "Plnené hovädzím mäsom a ryžou", 7.50m, "Taco Fiesta", "Mexican", 0m),
//                new FoodItem("Guacamole", "Sides", "Domáce guacamole", 3.90m, "Taco Fiesta", "Mexican", 0m),
//                new FoodItem("Mediterranean Plate", "Main", "Grilované zelenina, hummus, pita", 8.20m, "Mediterraneo", "Mediterranean", 5m),
//                new FoodItem("Lamb Kebab", "Main", "Špíz z jahňaciny", 9.90m, "Mediterraneo", "Mediterranean", 0m),
//            };

//            // Populate restaurants list (including All)
//            Restaurants = new ObservableCollection<string>(new[] { "All" }.Concat(AllItems.Select(i => i.Restaurant).Distinct()));

//            // Populate cuisines list
//            Cuisines = new ObservableCollection<string>(new[] { "All" }.Concat(AllItems.Select(i => i.Cuisine).Distinct()));

//            // defaults
//            SelectedRestaurant = "All";
//            SelectedCuisine = "All";

//            FilteredItems = new ObservableCollection<FoodItem>(AllItems);
//            Cart = new ObservableCollection<CartItem>();
//            RecentOrders = new ObservableCollection<string>();
//            LoyaltyPoints = 0;
//            this.DataContext = this;
//        }

//        public decimal Total => Cart.Sum(i => i.LineTotal);


//        //private void Login_Click(object sender, RoutedEventArgs e)
//        //{
//            // Defensive null checks in case XAML controls are not initialized
//            if (UsernameInput == null || PasswordInput == null)
//            {
//                MessageBox.Show("UI nie je inicializované.");
//                return;
//            }

//            if (!string.IsNullOrWhiteSpace(UsernameInput.Text) && !string.IsNullOrWhiteSpace(PasswordInput.Password))
//            {
//                LoginGrid.Visibility = Visibility.Collapsed;
//                MainAppGrid.Visibility = Visibility.Visible;
//            }
//            else
//            {
//                MessageBox.Show("Zadajte meno a heslo!");
//            }
//        }

//        private void Register_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Účet úspešne vytvorený! Môžete sa prihlásiť.");
//        private void Logout_Click(object sender, RoutedEventArgs e) { MainAppGrid.Visibility = Visibility.Collapsed; LoginGrid.Visibility = Visibility.Visible; }


//        private void FilterCategory_Click(object sender, RoutedEventArgs e)
//        {
//            var btn = sender as Button;
//            if (btn == null) return;

//            var tag = btn.Tag?.ToString() ?? "All";

//            if (tag == "Discount")
//            {
//                _isDiscountFilter = true;
//                _selectedCategory = "All";
//            }
//            else
//            {
//                _isDiscountFilter = false;
//                _selectedCategory = tag;
//            }
//            ApplyFilters();
//        }

//        private void ApplyFilters()
//        {
//            FilteredItems.Clear();
//            var items = AllItems.AsEnumerable();
//            if (_isDiscountFilter)
//            {
//                items = items.Where(i => i.Discount > 0);
//            }
//            else if (!string.IsNullOrEmpty(_selectedCategory) && _selectedCategory != "All") items = items.Where(i => i.Category == _selectedCategory);
//            if (!string.IsNullOrEmpty(SelectedRestaurant) && SelectedRestaurant != "All") items = items.Where(i => i.Restaurant == SelectedRestaurant);
//            if (!string.IsNullOrEmpty(SelectedCuisine) && SelectedCuisine != "All") items = items.Where(i => i.Cuisine == SelectedCuisine);
//            foreach (var i in items) FilteredItems.Add(i);
//        }

//        private void NavRestaurants_Click(object sender, RoutedEventArgs e)
//        {
//            // Show all restaurants / reset filters
//            SelectedRestaurant = "All";
//            SelectedCuisine = "All";
//            _selectedCategory = "All";
//            _isDiscountFilter = false;
//            ApplyFilters();
//        }

//        private void NavProfile_Click(object sender, RoutedEventArgs e)
//        {
//            var pw = new ProfileWindow(LoyaltyPoints, RecentOrders) { Owner = this };
//            pw.ShowDialog();
//        }


//        private void AddToCart_Click(object sender, RoutedEventArgs e)
//        {
//            var btn = sender as Button;
//            if (btn == null) return;

//            var food = btn.DataContext as FoodItem;
//            if (food == null) return;
//            var inCart = Cart.FirstOrDefault(c => c.Name == food.Name && c.Price == food.Price && c.DiscountPercent == food.Discount);
//            if (inCart != null) inCart.Quantity++;
//            else Cart.Add(new CartItem { Name = food.Name, Price = food.Price, Quantity = 1, DiscountPercent = food.Discount, Restaurant = food.Restaurant });
//            OnPropertyChanged(nameof(Total));
//        }

//        private void Qty_Click(object sender, RoutedEventArgs e)
//        {
//            var btn = sender as Button;
//            if (btn == null) return;
//            var item = btn.DataContext as CartItem;
//            if (item == null) return;
//            if ((btn.Tag ?? string.Empty).ToString() == "plus") item.Quantity++;
//            else { if (item.Quantity > 1) item.Quantity--; else Cart.Remove(item); }
//            OnPropertyChanged(nameof(Total));
//        }

//        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
//        {
//            var btn = sender as Button;
//            if (btn == null) return;
//            var item = btn.DataContext as CartItem;
//            if (item == null) return;
//            if (Cart.Contains(item)) Cart.Remove(item);
//            OnPropertyChanged(nameof(Total));
//        }

//        private void ClearCart_Click(object sender, RoutedEventArgs e)
//        {
//            Cart.Clear();
//            OnPropertyChanged(nameof(Total));
//        }


//        private async void Pay_Click(object sender, RoutedEventArgs e)
//        {
//            if (Cart.Count == 0) return;

//            // Open payment window
//            var payment = new PaymentWindow(Total, LoyaltyPoints) { Owner = this };
//            if (payment == null) return;

//            var res = payment.ShowDialog();
//            if (res != true) return; // cancelled

//            int usedPoints = payment.UsedPoints;
//            decimal pointsValue = usedPoints * 0.10m; // 1 point = 0.10€

//            // Calculate payable amount
//            decimal amount = Total;
//            decimal amountAfterPoints = Math.Max(0, amount - pointsValue);

//            StatusText.Text = "Spracovávam platbu... ";
//            await Task.Delay(2000); // simulate processing

//            // Update loyalty
//            int earnedPoints = (int)Math.Floor(amountAfterPoints);
//            LoyaltyPoints = Math.Max(0, LoyaltyPoints - usedPoints) + earnedPoints;

//            // Record recent order
//            RecentOrders.Insert(0, $"{DateTime.Now:HH:mm} - Zaplatené: {amountAfterPoints:N2}€ - Body získané: {earnedPoints}");

//            // Start kitchen timer based on items
//            int itemsCount = Cart.Sum(i => i.Quantity);
//            int waitSeconds = 30 + (itemsCount * 10); // base 30s + 10s per item

//            Cart.Clear();
//            OnPropertyChanged(nameof(Total));

//            DispatcherTimer timer = new DispatcherTimer();
//            timer.Interval = TimeSpan.FromSeconds(1);
//            int remaining = waitSeconds;
//            string FormatRemain(int secs)
//            {
//                var ts = TimeSpan.FromSeconds(Math.Max(0, secs));
//                return ts.ToString(@"mm\:ss");
//            }

//            StatusText.Text = $"Objednávka prijatá — pripravené približne za {FormatRemain(remaining)}";
//            timer.Tick += (s, ev) =>
//            {
//                if (remaining <= 0)
//                {
//                    StatusText.Text = "Jedlo je HOTOVÉ! ";
//                    MessageBox.Show("Vaša objednávka je pripravená na vyzdvihnutie! Prajeme dobrú chuť.", "PrimePlate NOTIFIKÁCIA");
//                    timer.Stop();
//                }
//                else
//                {
//                    StatusText.Text = $"Objednávka pripravená za {FormatRemain(remaining)}";
//                    remaining--;
//                }
//            };
//            timer.Start();
//        }

//        public event PropertyChangedEventHandler? PropertyChanged;
//        protected void OnPropertyChanged(string n) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
//    }

//    public class FoodItem
//    {
//        public string Name { get; set; }
//        public string Category { get; set; }
//        public string Description { get; set; }
//        public decimal Price { get; set; }
//        public string Restaurant { get; set; }
//        public string Cuisine { get; set; }
//        public decimal Discount { get; set; } // percent, e.g., 10 means 10%
//        public decimal PriceAfterDiscount => Math.Round(Price * (1 - Discount / 100m), 2);
//        public FoodItem(string n, string c, string d, decimal p, string r, string cuisine = "International", decimal discount = 0m) { Name = n; Category = c; Description = d; Price = p; Restaurant = r; Cuisine = cuisine; Discount = discount; }
//    }

//    public class CartItem : INotifyPropertyChanged
//    {
//        public string Name { get; set; }
//        public decimal Price { get; set; }
//        public decimal DiscountPercent { get; set; }
//        public string Restaurant { get; set; }
//        private int _q;
//        public int Quantity { get => _q; set { _q = value; OnPropertyChanged("Quantity"); OnPropertyChanged("LineTotal"); } }
//        public decimal PriceAfterDiscount => Math.Round(Price * (1 - DiscountPercent / 100m), 2);
//        public decimal LineTotal => PriceAfterDiscount * Quantity;
//        public event PropertyChangedEventHandler PropertyChanged;
//        protected void OnPropertyChanged(string n) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
//    }
//}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FastFoodOrderSystem
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // full data set
        public ObservableCollection<FoodItem> AllItems { get; set; } = new ObservableCollection<FoodItem>();
        // string lists for comboboxes (XAML expects strings)
        public ObservableCollection<string> Restaurants { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Cuisines { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<FoodItem> FilteredItems { get; set; } = new ObservableCollection<FoodItem>();
        public ObservableCollection<CartItem> Cart { get; set; } = new ObservableCollection<CartItem>();
        public ObservableCollection<string> RecentOrders { get; set; } = new ObservableCollection<string>();
        // loyalty points for current session/user
        public int LoyaltyPoints { get; set; } = 0;

        private string _selectedRestaurant = "All";
        public string SelectedRestaurant
        {
            get => _selectedRestaurant;
            set { _selectedRestaurant = value; ApplyFilters(); OnPropertyChanged(nameof(SelectedRestaurant)); }
        }

        private string _selectedCuisine = "All";
        public string SelectedCuisine
        {
            get => _selectedCuisine;
            set { _selectedCuisine = value; ApplyFilters(); OnPropertyChanged(nameof(SelectedCuisine)); }
        }

        private string _selectedCategory = "All";
        private bool _isDiscountFilter = false;

        public decimal Total => Math.Round(Cart.Sum(i => i.LineTotal), 2);

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            LoadInitialData();
        }

        private void LoadInitialData()
        {
            AllItems.Clear();

           
            var restaurants = new (string Name, string Cuisine)[]
            {
                ("McDonalds","American"), ("Pizza Hut","Italian"), ("Gastro Grill","American"), ("Green Garden","Vegetarian"),
                ("Sushi House","Asian"), ("PrimePlate","International"), ("Thai Corner","Asian"), ("Spice India","Indian"),
                ("Taco Fiesta","Mexican"), ("Mediterraneo","Mediterranean"), ("Burger Barn","American"), ("Seafood Shack","Seafood")
            };

            var categories = new[] { "Main", "Sides", "Drink", "Dessert", "Salad", "Pizza", "Sushi", "Noodles", "Wrap", "Soup" };
            var baseNames = new[] { "Classic", "Deluxe", "Special", "Crispy", "Spicy", "Signature", "House", "Ultimate", "Mini", "Supreme", "Golden", "Hot", "Cheesy", "Smoky", "Herb" };

            foreach (var r in restaurants)
            {
                for (int i = 0; i < 15; i++)
                {
                    var name = $"{r.Name} {baseNames[i % baseNames.Length]} {(i + 1)}";
                    var category = categories[i % categories.Length];
                    decimal price = Math.Round(2.50m + (i % 7) * 1.75m + (r.Name.Length % 3) * 0.5m, 2);
                    decimal discount = (i % 6 == 0) ? 10m : ((i % 9 == 0) ? 15m : 0m);
                    var item = new FoodItem(name, category, $"{category} from {r.Name}", price, r.Name, r.Cuisine, discount);
                    AllItems.Add(item);
                }
            }

           
            Restaurants.Clear();
            Restaurants.Add("All");
            foreach (var s in AllItems.Select(x => x.Restaurant).Distinct().OrderBy(x => x)) Restaurants.Add(s);

            Cuisines.Clear();
            Cuisines.Add("All");
            foreach (var c in AllItems.Select(x => x.Cuisine).Distinct().OrderBy(x => x)) Cuisines.Add(c);

            
            SelectedRestaurant = "All";
            SelectedCuisine = "All";
            _selectedCategory = "All";
            _isDiscountFilter = false;

            
            FilteredItems.Clear();
            foreach (var it in AllItems) FilteredItems.Add(it);

            Cart.Clear();
            RecentOrders.Clear();
            LoyaltyPoints = 0;
            OnPropertyChanged(nameof(Total));
        }

        private void ApplyFilters()
        {
            FilteredItems.Clear();
            var items = AllItems.AsEnumerable();
            if (_isDiscountFilter)
            {
                items = items.Where(i => i.Discount > 0);
            }
            else if (!string.IsNullOrEmpty(_selectedCategory) && _selectedCategory != "All")
            {
                items = items.Where(i => i.Category == _selectedCategory);
            }
            if (!string.IsNullOrEmpty(SelectedRestaurant) && SelectedRestaurant != "All") items = items.Where(i => i.Restaurant == SelectedRestaurant);
            if (!string.IsNullOrEmpty(SelectedCuisine) && SelectedCuisine != "All") items = items.Where(i => i.Cuisine == SelectedCuisine);
            foreach (var i in items) FilteredItems.Add(i);
            if (PageTitle != null) PageTitle.Text = (SelectedRestaurant == "All") ? "Kompletná ponuka" : SelectedRestaurant;
        }

        private void ShowAllFood()
        {
            _isDiscountFilter = false;
            _selectedCategory = "All";
            SelectedRestaurant = "All";
            SelectedCuisine = "All";
            FilteredItems.Clear();
            foreach (var it in AllItems) FilteredItems.Add(it);
            if (PageTitle != null) PageTitle.Text = "Kompletná ponuka";
        }

        private void Auth_Click(object sender, RoutedEventArgs e)
        {
            if (UsernameInput == null || PasswordInput == null) return;
            if (!string.IsNullOrWhiteSpace(UsernameInput.Text) && !string.IsNullOrWhiteSpace(PasswordInput.Password))
            {
                LoginGrid.Visibility = Visibility.Collapsed;
                MainAppGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Zadajte meno a heslo!");
            }
        }

        private void FilterCategory_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button b && b.Tag is string tag)
            {
                if (tag == "Discount")
                {
                    _isDiscountFilter = true;
                    _selectedCategory = "All";
                }
                else
                {
                    _isDiscountFilter = false;
                    _selectedCategory = tag;
                }
                ApplyFilters();
            }
        }

        private void NavRestaurants_Click(object sender, RoutedEventArgs e) => ShowAllFood();
        private void NavProfile_Click(object sender, RoutedEventArgs e)
        {
            var pw = new ProfileWindow(LoyaltyPoints, RecentOrders) { Owner = this };
            pw.ShowDialog();
        }

        private void Login_Click(object sender, RoutedEventArgs e) => Auth_Click(sender, e);
        private void Register_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Účet úspešne vytvorený! Môžete sa prihlásiť.");
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginGrid.Visibility = Visibility.Visible;
            MainAppGrid.Visibility = Visibility.Collapsed;
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is FoodItem food)
            {
                var existing = Cart.FirstOrDefault(c => c.Name == food.Name && c.Price == food.Price && c.Restaurant == food.Restaurant);
                if (existing != null) existing.Quantity++;
                else Cart.Add(new CartItem { Name = food.Name, Price = food.Price, Quantity = 1, DiscountPercent = food.Discount, Restaurant = food.Restaurant });
                OnPropertyChanged(nameof(Total));
            }
        }

        private void Qty_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is CartItem item)
            {
                var tag = (sender as Button).Tag?.ToString() ?? string.Empty;
                if (tag == "plus") item.Quantity++;
                else if (tag == "minus") { if (item.Quantity > 1) item.Quantity--; else Cart.Remove(item); }
                OnPropertyChanged(nameof(Total));
            }
        }

        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is CartItem item)
            {
                if (Cart.Contains(item)) Cart.Remove(item);
                OnPropertyChanged(nameof(Total));
            }
        }

        private void ClearCart_Click(object sender, RoutedEventArgs e)
        {
            Cart.Clear();
            OnPropertyChanged(nameof(Total));
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            if (Cart.Count == 0) return;
            MessageBox.Show($"Objednávka odoslaná! Cena: {Total:N2}€");
            Cart.Clear();
            OnPropertyChanged(nameof(Total));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

   
    public class FoodItem
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Restaurant { get; set; }
        public string Cuisine { get; set; }
        public decimal Discount { get; set; } 
        public decimal PriceAfterDiscount => Math.Round(Price * (1 - Discount / 100m), 2);

        public FoodItem(string n, string category, string desc, decimal price, string restaurant, string cuisine = "International", decimal discount = 0m)
        {
            Name = n; Category = category; Description = desc; Price = price; Restaurant = restaurant; Cuisine = cuisine; Discount = discount;
        }
    }

    public class CartItem : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercent { get; set; }
        public string Restaurant { get; set; }

        private int _q;
        public int Quantity
        {
            get => _q;
            set
            {
                if (_q == value) return;
                _q = value;
                OnPropertyChanged(nameof(Quantity));
                OnPropertyChanged(nameof(LineTotal));
            }
        }

        public decimal PriceAfterDiscount => Math.Round(Price * (1 - DiscountPercent / 100m), 2);
        public decimal LineTotal => Math.Round(PriceAfterDiscount * Quantity, 2);

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string n) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}

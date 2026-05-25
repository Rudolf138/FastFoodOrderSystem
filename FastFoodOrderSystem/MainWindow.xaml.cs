using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using FastFoodOrderSystem.Data;

namespace FastFoodOrderSystem
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // full data set
        public ObservableCollection<FoodItem> AllItems { get; set; } = new ObservableCollection<FoodItem>();
        // string lists for comboboxes (XAML expects strings)
        public ObservableCollection<string> Restaurants { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> Cuisines { get; set; } = new ObservableCollection<string>();
        // Additional runtime filter/search properties
        public ObservableCollection<FoodItem> FilteredItems { get; set; } = new ObservableCollection<FoodItem>();
        // Free-text search
        private string _searchText;
        public string SearchText { get => _searchText; set { _searchText = value; ApplyFilters(); OnPropertyChanged(nameof(SearchText)); } }

        // Price range filters
        private decimal? _minPrice;
        public decimal? MinPrice { get => _minPrice; set { _minPrice = value; ApplyFilters(); OnPropertyChanged(nameof(MinPrice)); } }
        private decimal? _maxPrice;
        public decimal? MaxPrice { get => _maxPrice; set { _maxPrice = value; ApplyFilters(); OnPropertyChanged(nameof(MaxPrice)); } }

        // Rating & favorites
        private double _minRating = 0;
        public double MinRating { get => _minRating; set { _minRating = value; ApplyFilters(); OnPropertyChanged(nameof(MinRating)); } }
        private bool _favoritesOnly = false;
        public bool FavoritesOnly { get => _favoritesOnly; set { _favoritesOnly = value; ApplyFilters(); OnPropertyChanged(nameof(FavoritesOnly)); } }

        // Sorting selection (simple string keys used by FilterService)
        private string _sortOption = string.Empty;
        public string SortOption { get => _sortOption; set { _sortOption = value; ApplyFilters(); OnPropertyChanged(nameof(SortOption)); } }
        
        // Payment & courier
        public ObservableCollection<string> PaymentMethods { get; set; } = new ObservableCollection<string> { "Hotovosť", "Karta", "Google Pay" };
        public string SelectedPaymentMethod { get; set; } = "Karta";
        public ObservableCollection<string> Couriers { get; set; } = new ObservableCollection<string> { "Lokálny kuriér", "DHL", "GLS", "Osobný odber" };
        public string SelectedCourier { get; set; } = "Lokálny kuriér";

        // loyalty usage
        private bool _usePoints = false;
        public bool UsePoints { get => _usePoints; set { _usePoints = value; OnPropertyChanged(nameof(UsePoints)); OnPropertyChanged(nameof(Total)); } }

        private int _pointsToUse = 0;
        public int PointsToUse { get => _pointsToUse; set { _pointsToUse = Math.Max(0, Math.Min(value, LoyaltyPoints)); OnPropertyChanged(nameof(PointsToUse)); OnPropertyChanged(nameof(Total)); } }
        public ObservableCollection<CartItem> Cart { get; set; } = new ObservableCollection<CartItem>();
        public ObservableCollection<string> RecentOrders { get; set; } = new ObservableCollection<string>();
        // loyalty points for current session/user
        public int LoyaltyPoints { get; set; } = 0;

        // derived totals
        public decimal SubTotal => Math.Round(Cart.Sum(i => i.LineTotal), 2);
        public decimal Total => Math.Round(Math.Max(0, SubTotal - ((UsePoints ? PointsToUse : 0) * 0.10m)), 2);

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

        

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            
            using (var db = new AppDbContext())
            {
                db.Database.EnsureCreated();

                if (db.FoodItems.Any())
                {
                    AllItems.Clear();
                    foreach (var f in db.FoodItems.AsNoTracking().ToList()) AllItems.Add(f);
                }
                else
                {
                    
                    LoadInitialData();
                    foreach (var it in AllItems) db.FoodItems.Add(it);
                    db.SaveChanges();
                }

                
                Cart.Clear();
                foreach (var c in db.CartItems.AsNoTracking().ToList()) Cart.Add(c);

                
                RecentOrders.Clear();
                foreach (var r in db.RecentOrders.OrderByDescending(x => x.CreatedAt).Select(x => x.Text).ToList()) RecentOrders.Add(r);

               
                var lp = db.Settings?.FirstOrDefault(x => x.Key == "LoyaltyPoints")?.Value;
                if (!string.IsNullOrEmpty(lp) && int.TryParse(lp, out var pts)) LoyaltyPoints = pts;

                
                Restaurants.Clear(); Restaurants.Add("All");
                foreach (var s in AllItems.Select(x => x.Restaurant).Distinct().OrderBy(x => x)) Restaurants.Add(s);
                Cuisines.Clear(); Cuisines.Add("All");
                foreach (var c in AllItems.Select(x => x.Cuisine).Distinct().OrderBy(x => x)) Cuisines.Add(c);

                SelectedRestaurant = "All";
                SelectedCuisine = "All";
                _selectedCategory = "All";
                _isDiscountFilter = false;

                FilteredItems.Clear();
                foreach (var it in AllItems) FilteredItems.Add(it);
            }

            
            this.Closing += (s, e) =>
            {
                using var db = new AppDbContext();
                db.Database.EnsureCreated();

                
                if (!db.FoodItems.Any())
                {
                    foreach (var it in AllItems) db.FoodItems.Add(it);
                }

                db.CartItems.RemoveRange(db.CartItems);
                foreach (var c in Cart) db.CartItems.Add(c);

                db.RecentOrders.RemoveRange(db.RecentOrders);
                foreach (var r in RecentOrders) db.RecentOrders.Add(new RecentOrder { Text = r, CreatedAt = DateTime.Now });

                var setting = db.Settings.FirstOrDefault(x => x.Key == "LoyaltyPoints");
                if (setting == null) db.Settings.Add(new Setting { Key = "LoyaltyPoints", Value = LoyaltyPoints.ToString() });
                else setting.Value = LoyaltyPoints.ToString();

                db.SaveChanges();
            };
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
                    
                    var name = $"{baseNames[i % baseNames.Length]} - {r.Name}";
                    var category = categories[i % categories.Length];
                    decimal price = Math.Round(2.50m + (i % 7) * 1.75m + (r.Name.Length % 3) * 0.5m, 2);
                    decimal discount = (i % 6 == 0) ? 10m : ((i % 9 == 0) ? 15m : 0m);

                    
                    var rating = Math.Round(3.0 + (new Random((r.Name + i).GetHashCode()).NextDouble() * 2.0), 1);
                    var tagsPool = new[] { "spicy", "vegan", "gluten-free", "kids", "popular", "recommended", "new" };
                    var tags = new List<string>();
                   
                    if (i % 2 == 0) tags.Add(tagsPool[i % tagsPool.Length]);
                    if (i % 5 == 0) tags.Add(tagsPool[(i + 3) % tagsPool.Length]);

                    var isFav = (i % 11 == 0); 

                    var item = new FoodItem(name, category, $"{category} from {r.Name}", price, r.Name, r.Cuisine, discount, rating, isFav, tags);
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
            OnPropertyChanged(nameof(SubTotal));
            OnPropertyChanged(nameof(Total));
        }

        private void ApplyFilters()
        {
            
            var criteria = new FilterCriteria
            {
                Restaurant = SelectedRestaurant,
                Cuisine = SelectedCuisine,
                Category = _selectedCategory,
                SearchText = SearchText,
                MinPrice = MinPrice,
                MaxPrice = MaxPrice,
                MinRating = MinRating,
                FavoritesOnly = FavoritesOnly,
                HasDiscountOnly = _isDiscountFilter,
                SortBy = SortOption
            };

            var items = FilterService.Apply(AllItems, criteria);
            FilteredItems.Clear();
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

        
        private void NavMyMenu_Click(object sender, RoutedEventArgs e)
        {
            ShowMyMenu();
        }

        private void ShowMyMenu()
        {
            var items = AllItems.Where(i => i.IsFavorite);
            FilteredItems.Clear();
            foreach (var it in items) FilteredItems.Add(it);
            if (PageTitle != null) PageTitle.Text = "Moje menu";
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
                OnPropertyChanged(nameof(SubTotal));
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
                OnPropertyChanged(nameof(SubTotal));
                OnPropertyChanged(nameof(Total));
            }
        }

        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is CartItem item)
            {
                if (Cart.Contains(item)) Cart.Remove(item);
                OnPropertyChanged(nameof(SubTotal));
                OnPropertyChanged(nameof(Total));
            }
        }

        private void ClearCart_Click(object sender, RoutedEventArgs e)
        {
            Cart.Clear();
            OnPropertyChanged(nameof(SubTotal));
            OnPropertyChanged(nameof(Total));
        }

        private async void Pay_Click(object sender, RoutedEventArgs e)
        {
            if (Cart.Count == 0) return;

            decimal amount = Total;
            int usedPoints = 0;
            if (UsePoints && PointsToUse > 0)
            {
                usedPoints = Math.Min(PointsToUse, LoyaltyPoints);
            }
            decimal pointsValue = usedPoints * 0.10m;
            decimal amountAfterPoints = Math.Max(0, amount - pointsValue);

            StatusText.Text = $"Spracovávam platbu ({SelectedPaymentMethod})...";
            await Task.Delay(1000);

            int earnedPoints = (int)Math.Floor(amountAfterPoints);
            LoyaltyPoints = Math.Max(0, LoyaltyPoints - usedPoints) + earnedPoints;
            OnPropertyChanged(nameof(LoyaltyPoints));

            
            RecentOrders.Insert(0, $"{DateTime.Now:HH:mm} - Zaplatené: {amountAfterPoints:N2}€ - Body +{earnedPoints} - Platba: {SelectedPaymentMethod} - Kuriér: {SelectedCourier}");

            StatusText.Text = $"Objednávka prijatá — kuriér: {SelectedCourier} — stav: V PRÍPRAVE";

            Cart.Clear();
            OnPropertyChanged(nameof(Total));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

   
    public class FoodItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Restaurant { get; set; }
        public string Cuisine { get; set; }
        public decimal Discount { get; set; } 
        
        public decimal PriceAfterDiscount => Math.Round(Price * (1 - Discount / 100m), 2);
        public double Rating { get; set; }
        public bool IsFavorite { get; set; }
        public List<string> Tags { get; set; }

                
        public FoodItem()
        {
            Tags = new List<string>();
        }

        public FoodItem(string n, string category, string desc, decimal price, string restaurant, string cuisine = "International", decimal discount = 0m, double rating = 0.0, bool isFavorite = false, List<string> tags = null)
        {
            Name = n; Category = category; Description = desc; Price = price; Restaurant = restaurant; Cuisine = cuisine; Discount = discount;
            Rating = rating; IsFavorite = isFavorite; Tags = tags ?? new List<string>();
        }
    }

    public class CartItem : INotifyPropertyChanged
    {
        public int Id { get; set; }
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

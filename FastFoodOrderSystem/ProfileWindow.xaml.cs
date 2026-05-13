using System.Collections.ObjectModel;
using System.Windows;

namespace FastFoodOrderSystem
{
    public partial class ProfileWindow : Window
    {
        public int LoyaltyPoints { get; private set; }
        public ObservableCollection<string> RecentOrders { get; private set; }

        public ProfileWindow(int points, ObservableCollection<string> recentOrders)
        {
            InitializeComponent();
            LoyaltyPoints = points;
            RecentOrders = recentOrders;
            if (PointsText != null) PointsText.Text = LoyaltyPoints.ToString();
            if (OrdersList != null) OrdersList.ItemsSource = RecentOrders;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

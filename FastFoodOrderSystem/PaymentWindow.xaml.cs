using System;
using System.Windows;

namespace FastFoodOrderSystem
{
    public partial class PaymentWindow : Window
    {
        public decimal Total { get; private set; }
        public int AvailablePoints { get; private set; }
        public int UsedPoints { get; private set; }
        public string PaymentMethod { get; private set; } = string.Empty;

        public PaymentWindow(decimal total, int availablePoints)
        {
            InitializeComponent();
            Total = total;
            AvailablePoints = availablePoints;
            if (TitleText != null) TitleText.Text = "Vyberte spôsob platby";
            if (TotalText != null) TotalText.Text = $"{Total:N2}€";
            if (AvailablePointsText != null) AvailablePointsText.Text = AvailablePoints.ToString();

            // default visibility (guard controls)
            if (CardFields != null && CardRadio != null)
            {
                CardFields.Visibility = CardRadio.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            // Read used points
            if (!int.TryParse(PointsInput.Text, out int pts)) pts = 0;
            if (pts < 0) pts = 0;
            if (pts > AvailablePoints) pts = AvailablePoints;
            UsedPoints = pts;

            PaymentMethod = CardRadio.IsChecked == true ? "Card" : "Cash";

            // In a real app you'd validate card details when PaymentMethod == "Card"

            this.DialogResult = true;
            this.Close();
        }

        private void PaymentMethodChanged(object sender, RoutedEventArgs e)
        {
            if (CardFields == null) return;
            CardFields.Visibility = CardRadio.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}

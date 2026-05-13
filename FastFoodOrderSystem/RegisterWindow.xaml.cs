using System.Windows;

namespace FastFoodOrderSystem
{
    public partial class RegisterWindow : Window
    {
        public User RegisteredUser { get; private set; }
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            if (RegUsername == null || RegPassword == null || RegEmail == null)
            {
                MessageBox.Show("UI nie je inicializované.", "Chyba");
                return;
            }

            var username = RegUsername.Text?.Trim();
            var pwd = RegPassword.Password;
            var email = RegEmail.Text?.Trim();
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vyplňte všetky polia.", "Chýbajúce údaje");
                return;
            }

            RegisteredUser = new User { Username = username, Password = pwd, Email = email };
            this.DialogResult = true;
            this.Close();
        }
    }
}

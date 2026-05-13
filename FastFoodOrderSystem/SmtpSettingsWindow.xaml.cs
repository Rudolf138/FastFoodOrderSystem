using System;
using System.Windows;

namespace FastFoodOrderSystem
{
    public partial class SmtpSettingsWindow : Window
    {
        public SmtpSettings? Settings { get; private set; }
        public SmtpSettingsWindow(SmtpSettings current = null)
        {
            InitializeComponent();
            if (current != null)
            {
                if (SmtpHost != null) SmtpHost.Text = current.Host;
                if (SmtpPort != null) SmtpPort.Text = current.Port.ToString();
                if (SenderEmail != null) SenderEmail.Text = current.SenderEmail;
                if (SenderPassword != null) SenderPassword.Password = current.SenderPassword ?? string.Empty;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            int port = 587;
            if (SmtpPort != null) int.TryParse(SmtpPort.Text, out port);
            Settings = new SmtpSettings
            {
                Host = SmtpHost?.Text?.Trim(),
                Port = port,
                SenderEmail = SenderEmail?.Text?.Trim(),
                SenderPassword = SenderPassword?.Password
            };
            this.DialogResult = true;
            this.Close();
        }

        private async void Test_Click(object sender, RoutedEventArgs e)
        {
            int port = 587;
            if (SmtpPort != null) int.TryParse(SmtpPort.Text, out port);
            var settings = new SmtpSettings
            {
                Host = SmtpHost?.Text?.Trim(),
                Port = port,
                SenderEmail = SenderEmail?.Text?.Trim(),
                SenderPassword = SenderPassword?.Password
            };

            if (string.IsNullOrEmpty(settings.Host) || string.IsNullOrEmpty(settings.SenderEmail) || string.IsNullOrEmpty(settings.SenderPassword))
            {
                MessageBox.Show("Vyplňte hostiteľa, e-mail odosielateľa a heslo pred testovaním.", "Chýbajúce údaje");
                return;
            }

            try
            {
                await SmtpSender.SendEmailAsync(settings, settings.SenderEmail, "PrimePlate - Test SMTP", "Toto je testovacia správa z aplikácie PrimePlate.");
                MessageBox.Show("Testovací e-mail bol odoslaný. Skontrolujte prijímací mailbox.", "Test úspešný");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Test zlyhal: {ex.Message}\n\nTipy:\n- Pre Gmail používajte 'smtp.gmail.com', port 587 (StartTLS) alebo 465 (SSL) a App Password (ak máte 2FA).\n- Skontrolujte, či firewall alebo poskytovateľ neblokuje odchádzajúci SMTP.", "Chyba pri testovaní SMTP");
            }
        }
    }
}

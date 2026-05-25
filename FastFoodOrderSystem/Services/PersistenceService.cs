using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FastFoodOrderSystem
{
    public class AppState
    {
        public List<FoodItem> AllItems { get; set; }
        public List<CartItem> Cart { get; set; }
        public List<string> RecentOrders { get; set; }
        public int LoyaltyPoints { get; set; }
    }

    public static class PersistenceService
    {
        private static readonly string Folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PrimePlate");
        private static readonly string FilePath = Path.Combine(Folder, "state.json");

        public static void SaveState(AppState state)
        {
            try
            {
                if (!Directory.Exists(Folder)) Directory.CreateDirectory(Folder);
                var opts = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(state, opts);
                File.WriteAllText(FilePath, json);
            }
            catch
            {
               
            }
        }

        public static AppState LoadState()
        {
            try
            {
                if (!File.Exists(FilePath)) return null;
                var json = File.ReadAllText(FilePath);
                var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<AppState>(json, opts);
            }
            catch
            {
                return null;
            }
        }
    }
}

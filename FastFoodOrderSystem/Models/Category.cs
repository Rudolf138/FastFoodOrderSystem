namespace FastFoodOrderSystem
{
    // Small helper model for category metadata
    public class Category
    {
        public string Name { get; set; }
        public string Icon { get; set; }

        public Category(string name, string icon = null)
        {
            Name = name;
            Icon = icon;
        }
    }
}

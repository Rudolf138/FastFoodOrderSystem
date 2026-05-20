namespace FastFoodOrderSystem
{
    // Lightweight model representing a restaurant (added for clarity and possible future features)
    public class Restaurant
    {
        public string Name { get; set; }
        public string Cuisine { get; set; }
        public double Rating { get; set; }

        public Restaurant(string name, string cuisine, double rating = 0)
        {
            Name = name;
            Cuisine = cuisine;
            Rating = rating;
        }
    }
}

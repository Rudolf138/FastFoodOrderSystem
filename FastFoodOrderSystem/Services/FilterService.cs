using System.Collections.Generic;
using System.Linq;

namespace FastFoodOrderSystem
{
    // Criteria holder used by the filtering service
    public class FilterCriteria
    {
        public string Restaurant { get; set; }
        public string Cuisine { get; set; }
        public string Category { get; set; }
        public string SearchText { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public double MinRating { get; set; } = 0;
        public bool FavoritesOnly { get; set; } = false;
        public bool HasDiscountOnly { get; set; } = false;
        public string SortBy { get; set; }
    }

    // Small filtering service to keep MainWindow logic tidy and testable
    public static class FilterService
    {
        public static IEnumerable<FoodItem> Apply(IEnumerable<FoodItem> items, FilterCriteria c)
        {
            if (items == null) return Enumerable.Empty<FoodItem>();
            var q = items;
            if (c == null) return q;

            if (!string.IsNullOrEmpty(c.SearchText))
            {
                var s = c.SearchText.Trim().ToLowerInvariant();
                q = q.Where(i => (i.Name ?? string.Empty).ToLowerInvariant().Contains(s)
                                 || (i.Description ?? string.Empty).ToLowerInvariant().Contains(s)
                                 || (i.Tags != null && i.Tags.Any(t => t.ToLowerInvariant().Contains(s))));
            }

            if (!string.IsNullOrEmpty(c.Category) && c.Category != "All") q = q.Where(i => i.Category == c.Category);
            if (!string.IsNullOrEmpty(c.Restaurant) && c.Restaurant != "All") q = q.Where(i => i.Restaurant == c.Restaurant);
            if (!string.IsNullOrEmpty(c.Cuisine) && c.Cuisine != "All") q = q.Where(i => i.Cuisine == c.Cuisine);
            if (c.MinPrice.HasValue) q = q.Where(i => i.Price >= c.MinPrice.Value);
            if (c.MaxPrice.HasValue) q = q.Where(i => i.Price <= c.MaxPrice.Value);
            if (c.MinRating > 0) q = q.Where(i => i.Rating >= c.MinRating);
            if (c.FavoritesOnly) q = q.Where(i => i.IsFavorite);
            if (c.HasDiscountOnly) q = q.Where(i => i.Discount > 0);

            q = c.SortBy switch
            {
                "priceAsc" => q.OrderBy(i => i.PriceAfterDiscount),
                "priceDesc" => q.OrderByDescending(i => i.PriceAfterDiscount),
                "rating" => q.OrderByDescending(i => i.Rating),
                "name" => q.OrderBy(i => i.Name),
                _ => q
            };

            return q;
        }
    }
}

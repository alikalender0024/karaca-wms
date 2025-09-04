using Karaca.Wms.Api.Models;

namespace Karaca.Wms.Api.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Veritabanı oluştur
            context.Database.EnsureCreated();

            // Eğer ürünler zaten varsa seed yapılmasın
            if (context.Products.Any())
                return;

            var products = new Product[]
            {
                new Product { SKU = "P100", Name = "Karaca Tencere 24cm", StockQuantity = 50 },
                new Product { SKU = "P101", Name = "Emsan Çay Seti", StockQuantity = 30 },
                new Product { SKU = "P102", Name = "Jumbo Yastık", StockQuantity = 20 },
                new Product { SKU = "P103", Name = "Kaşmir Halı 120x180", StockQuantity = 10 }
            };
            if (!context.Locations.Any())
            {
                var locations = new Location[]
                {
                    new Location { Code = "A1", Zone = "Kitchen", Capacity = 50, CurrentLoad = 10 },
                    new Location { Code = "B2", Zone = "Living", Capacity = 30, CurrentLoad = 5 }
                };
                context.Locations.AddRange(locations);
                context.SaveChanges();
            }
            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}

namespace Karaca.Wms.Api.Models
{
    public class Inventory
    {
        public int Id { get; set; }

        // Ürün ve Lokasyon ilişkisi
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;

        public int Quantity { get; set; }
        public string? LotNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}

namespace Karaca.Wms.Api.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;  // Raf kodu, örn: A1, B2
        public string Zone { get; set; } = null!;  // Bölge, örn: Kitchen, Living
        public int Capacity { get; set; }          // Maksimum ürün kapasitesi
        public int CurrentLoad { get; set; }       // Mevcut yük
    }
}

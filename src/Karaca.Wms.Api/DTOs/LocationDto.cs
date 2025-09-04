namespace Karaca.Wms.Api.DTOs
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Zone { get; set; } = null!;
        public int Capacity { get; set; }
        public int CurrentLoad { get; set; }
    }
}

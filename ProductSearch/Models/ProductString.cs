using System.ComponentModel.DataAnnotations;

namespace ProductSearch.Models
{
    public class ProductString
    {
        [Key]
        public int Id { get; set; }
        public string RecordId { get; set; }
        public string ParticleSize { get; set; }
        public string IronContent { get; set; }
        public string Reactivitiy { get; set; }
        public double Price { get; set; }
        public string SurfaceArea { get; set; }
        public string Logevity { get; set; }
        public string Media { get; set; }
        public string Location { get; set; }
    }
}
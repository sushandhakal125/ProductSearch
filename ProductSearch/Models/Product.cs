using System;
using System.ComponentModel.DataAnnotations;

namespace ProductSearch.Models
{
    public class Product
    {
        [Key]
        public int RecordId { get; set; }
        public string ProductName { get; set; }
        public double ParticleSize { get; set; }
        public double ParticleSizeMin { get; set; }
        public double ParticleSizeMax { get; set; }
        public double IronContent { get; set; }
        public double Reactivitiy { get; set; }
        public double Price { get; set; }
        public double SurfaceArea { get; set; }
        public double Logevity { get; set; }
        public string Media { get; set; }
        public string Location { get; set; }
        public int Quantity { get; set; }
        public DateTime AvailableDate { get; set; }
    }
}
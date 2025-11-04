using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityWeathers.Data.Entity;

public class CityData
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public double? Temperature { get; set; }
    
    public double? Humidity { get; set; }
    
    public double? WindSpeed { get; set; }
    
    public double? AirQuality { get; set; }
    
    public string Pollutants { get; set; }
    
    public int CityId { get; set; }
    [ForeignKey("CityId")]
    public City City { get; set; }
}
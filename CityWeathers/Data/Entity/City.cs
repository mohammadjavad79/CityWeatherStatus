using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CityWeathers.Data.Entity;

public class City
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(80)]
    public string Name { get; set; }
    
    [Precision(9, 6)]
    
    public decimal Latitude { get; set; }
    [Precision(9, 6)]
    public decimal Longitude { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string Code { get; set; }
    
    public ICollection<WeatherApiRequest> WeatherApiRequests {get; set;} = new List<WeatherApiRequest>();
    
    public ICollection<CityData> CityData { get; set; } = new List<CityData>();
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityWeathers.Data.Entity;

public class WeatherApiRequest
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string? RequestJson { get; set; } = null;

    public string? ResponseJson { get; set; } = null;
    
    [ForeignKey("CityId")]
    public City City { get; set; }
    public int CityId { get; set; }
}
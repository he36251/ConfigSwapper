using System.ComponentModel.DataAnnotations;

namespace ConfigSwapper.Models;

public class ConfigFile
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string FileName { get; set; }
    
    [Required]
    public string Type { get; set; }
    
    [Required]
    public IEnumerable<ConfigEntity> Configs { get; set; }
}
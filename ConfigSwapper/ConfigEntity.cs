using System.ComponentModel.DataAnnotations;

namespace ConfigSwapper;

public class ConfigEntity
{
    [Required]
    public string Path { get; set; }
    
    [Required]
    public ConfigValue Value { get; set; }
}
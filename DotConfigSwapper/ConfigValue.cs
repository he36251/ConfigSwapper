using System.ComponentModel.DataAnnotations;

namespace ConfigSwapper;

public class ConfigValue
{
    [Required]
    public string Key { get; set; }
    
    [Required]
    public string Value { get; set; }
}
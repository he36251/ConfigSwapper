using System.ComponentModel.DataAnnotations;

namespace ConfigSwapper.Models;

public class ConfigValue
{
    [Required]
    public string Key { get; set; }
    
    [Required]
    public string Value { get; set; }
}
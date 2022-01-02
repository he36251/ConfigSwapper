using System.ComponentModel.DataAnnotations;
using ConfigSwapper.Models;

namespace ConfigSwapper;

public class ConfigEntity
{
    [Required]
    public string Path { get; set; }
    
    [Required]
    public ConfigValue Value { get; set; }
}
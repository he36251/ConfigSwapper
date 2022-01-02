using System.ComponentModel.DataAnnotations;

namespace ConfigSwapper.Models;

public class InputConfig
{
    [Required]
    public IEnumerable<ConfigFile> ConfigFiles { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace ConfigSwapper;

public class InputConfig
{
    [Required]
    public IEnumerable<ConfigFile> ConfigFiles { get; set; }
}
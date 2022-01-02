namespace ConfigSwapper.Models;

public class ConfigFile
{
    public string FileName { get; set; }
    
    public string Type { get; set; }
    
    public IEnumerable<ConfigEntity> Configs { get; set; }
}
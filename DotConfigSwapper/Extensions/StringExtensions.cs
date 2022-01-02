namespace ConfigSwapper.Extensions;

public static class StringExtensions
{
    public static string MarkupError(this string str, bool isBold = false)
    {
        var bold = isBold ? "bold " : "";
        
        return $"[{bold}red]{str}[/]";
    }
}
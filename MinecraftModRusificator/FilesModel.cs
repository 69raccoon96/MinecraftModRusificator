namespace MinecraftModRusificator;

public class FilesModel
{
    public string EnglishLanguage { get; set; }
    
    public string RussianLanguage { get; set; }
    
    public Dictionary<string,string> EnglishParsedJson { get; set; }
    
    public Dictionary<string,string> RussianParsedJson { get; set; }
}
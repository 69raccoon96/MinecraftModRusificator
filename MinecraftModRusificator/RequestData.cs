namespace MinecraftModRusificator;

public class RequestData
{
    public string sourceLanguageCode { get; set; }
    
    public string targetLanguageCode { get; set; }
    
    public string format { get; set; }

    public List<string> texts { get; set; }
    
    public GlossaryConfig glossaryConfig { get; set; }
}

public class GlossaryConfig
{
    public GlossaryData glossaryData { get; set; }
}

public class GlossaryData
{
    public List<GlossaryPair> glossaryPairs { get; set; }
}

public class GlossaryPair
{
    public string sourceText { get; set; }
    public string translatedText { get; set; }
    public bool exact { get; set; }
}
namespace MinecraftModRusificator;

public class TranslationResponse
{
    public List<Translation> translations { get; set; } = new();
}

public class Translation
{
    public string text { get; set; }
    public string detectedLanguageCode { get; set; }
}
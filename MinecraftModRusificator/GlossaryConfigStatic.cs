namespace MinecraftModRusificator;

public static class GlossaryConfigStatic
{
    public static GlossaryConfig Config = new()
    {
        glossaryData = new GlossaryData
        {
            glossaryPairs = new List<GlossaryPair>
            {
                new ()
                {
                    sourceText = "%s",
                    translatedText = "%s",
                    exact = false
                },
                new()
                {
                    sourceText = "§b",
                    translatedText = "§b",
                    exact = false
                },
                new()
                {
                    sourceText = "%d",
                    translatedText = "%d",
                    exact = false
                }
            }
        }
    };
}
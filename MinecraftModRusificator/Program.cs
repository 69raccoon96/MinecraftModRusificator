namespace MinecraftModRusificator;

public static class Program
{
    public static void Main()
    {
        var folderPath = @"C:\mods";

        var unpackedPath = $"{folderPath}\\unpacked";
        
        var resultPath = $"{folderPath}\\result";

        Directory.CreateDirectory(unpackedPath);
        Directory.CreateDirectory(resultPath);

        var filesInfos = Directory.GetFiles(folderPath)
            .AsParallel()
            .WithDegreeOfParallelism(12)
            .Select(x => new ModInfo(x))
            .ToArray();
        
        filesInfos.AsParallel().WithDegreeOfParallelism(16).ForAll(x => JarHelper.Unpack(x.FilePath, x.UnpackedFilePath));
        
        filesInfos
            .AsParallel()
            .WithDegreeOfParallelism(12)
            .ForAll(x => x.AddEnglishLanguage());

        var modsWithEnglish = filesInfos
            .Where(x => x.CanTranslate())
            .ToList();
        
        try
        {
            modsWithEnglish
                .ForEach(x =>
                {
                    x.AddRussianLanguage();
                    x.ParseJson();
                    x.TranslateToRussian();
                    x.SaveRuFiles();
                    JarHelper.Pack(x.UnpackedFilePath, x.ResultPath);
                });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
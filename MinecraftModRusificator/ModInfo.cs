using System.Text.Encodings.Web;
using System.Text.Json;

namespace MinecraftModRusificator;

public class ModInfo
{
    public string FilePath { get; set; }
    public string UnpackedFilePath { get; set; }
    public string ModName { get; set; }
    public string AssetsDirs { get; set; }
    
    public string ResultPath { get; set; }
    
    public List<FilesModel> Files { get; set; } = new();
    
    public ModInfo(string path)
    {
        var fileInfo = new FileInfo(path);
        var fileName = Path.GetFileNameWithoutExtension(path);
        var folder = fileInfo.DirectoryName;

        ModName = fileName;
        FilePath = fileInfo.FullName;
        UnpackedFilePath = $"{folder}\\unpacked\\{fileName}";
        AssetsDirs = $"{UnpackedFilePath}\\assets";
        ResultPath = $"{folder}\\result\\{fileName}.jar";
        
        
        Directory.CreateDirectory(UnpackedFilePath);
    }

    public void SaveRuFiles()
    {
        foreach (var file in Files)
        {
            var ruPath = file.RussianLanguage;
            var json = JsonSerializer.Serialize(file.RussianParsedJson, new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true,
            });
            File.WriteAllText(ruPath, json);
        }
    }

    public void TranslateToRussian()
    {
        var yandexTranslator = new YandexTranslate();
        
        foreach (var entry in Files)
        {
            var translated = yandexTranslator.Translate(entry.EnglishParsedJson).GetAwaiter().GetResult();
            if (translated.Count == 0 )
            {
                Console.WriteLine($"Не смогли перести мод: {ModName}");
            }
            entry.RussianParsedJson = translated;
        }
    }

    public void ParseJson()
    {
        foreach (var entry in Files)
        {
            var json = File.ReadAllText(entry.EnglishLanguage);
            var tempData = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            entry.EnglishParsedJson = tempData!;
        }
    }

    public void AddRussianLanguage()
    {
        foreach (var lang in Files)
        {
            var fileInfo = new FileInfo(lang.EnglishLanguage);
            var directoryName = fileInfo.DirectoryName;
            var ruFileDir = $"{directoryName}\\ru_ru.json";
            lang.RussianLanguage = ruFileDir;
        }
    }

    public void AddEnglishLanguage()
    {
        if(!Directory.Exists(AssetsDirs))
            return;
        
        var englishLanguages = Directory.GetFiles(AssetsDirs, "en_us.json", SearchOption.AllDirectories);

        var res = new List<FilesModel>();
        foreach (var englishLanguage in englishLanguages)
        {
            var fileInfo = new FileInfo(englishLanguage);
            var directoryFiles = Directory.GetFiles(fileInfo.Directory!.FullName);
            if(!directoryFiles.Any(x => x.Contains("ru_ru.json")))
            {
                res.Add(new FilesModel
                {
                    EnglishLanguage = englishLanguage,
                });
                continue;
            }

            var ruFile = directoryFiles.First(x => x.Contains("ru_ru.json"));
            var json = File.ReadAllText(ruFile);
            try
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                if (dict.Count == 0)
                {
                    res.Add(new FilesModel
                    {
                        EnglishLanguage = englishLanguage,
                    });
                }
            }
            catch (Exception e)
            {
                continue;
            }
            

        }
        
        Files = res;
    }

    public bool CanTranslate()
    {
        return Files.Count != 0;
    }
}
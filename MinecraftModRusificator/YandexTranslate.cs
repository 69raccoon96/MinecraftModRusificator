using System.Text.Json;

namespace MinecraftModRusificator;

public class YandexTranslate
{
    public async Task<Dictionary<string,string>> Translate(Dictionary<string,string> dictionary)
    {
        var httpClient = new HttpClient();
        var apiKey = File.ReadAllText("yandex-api-key.txt");
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Api-Key {apiKey}");
        var url = "https://translate.api.cloud.yandex.net/translate/v2/translate";

        var texts = dictionary
            .Select(x => x.Value)
            .ToArray();
        
        var resultDict = dictionary
            .ToDictionary(x => x.Key, x => x.Value);

        var results = new List<HttpResponseMessage>();

        foreach (var elem in SplitStrings(texts, 9999))
        {
            var model = new RequestData
            {
                sourceLanguageCode = "en",
                targetLanguageCode = "ru",
                format = "PLAIN_TEXT",
                texts = elem,
                glossaryConfig = GlossaryConfigStatic.Config
            };

            var json = JsonSerializer.Serialize(model);
        
            var res = await httpClient.PostAsync(new Uri(url), new StringContent(json));

            if (!res.IsSuccessStatusCode)
            {
                Console.WriteLine(await res.Content.ReadAsStringAsync());
                return new Dictionary<string, string>();
            }
            
            results.Add(res);
        }

        var translation = new TranslationResponse();

        foreach (var elem in results)
        {
            var text = await elem.Content.ReadAsStringAsync();
            var currentTranslation = JsonSerializer.Deserialize<TranslationResponse>(text);
            translation.translations.AddRange(currentTranslation!.translations);
        }

        try
        {
            
            var keyValuePair = resultDict.ToList();
            for (var i = 0; i < keyValuePair.Count; i++)
            {
                var curPair = keyValuePair[i];
                resultDict[curPair.Key] = translation.translations[i].text;
            }

            return resultDict;

        }
        catch (Exception e)
        {
            return new Dictionary<string, string>();
        }
    }
    
    static List<List<string>> SplitStrings(string[] arr, int maxLength)
    {
        var result = new List<List<string>>();
        var currentSubarray = new List<string>();
        int currentLength = 0;

        foreach (var str in arr)
        {
            int stringLength = str.Length;

            if (currentLength + stringLength <= maxLength)
            {
                currentSubarray.Add(str);
                currentLength += stringLength;
            }
            else
            {
                result.Add(currentSubarray);
                currentSubarray = new List<string> { str };
                currentLength = stringLength;
            }
        }

        if (currentSubarray.Count > 0)
        {
            result.Add(currentSubarray);
        }

        return result;
    }
}
using ICSharpCode.SharpZipLib.Zip;

namespace MinecraftModRusificator;

public class JarHelper
{
    public static void Unpack(string path, string destination)
    {
        var fz = new FastZip();
        fz.ExtractZip(path, destination, "");
    }

    public static void Pack(string path, string destination)
    {
        var fz = new FastZip();
        fz.CreateZip(destination, path, true, "");
    }
}
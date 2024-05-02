using System.IO;
using SwfLib;
using SwfLib.Tags;
using SwfLib.Tags.ActionsTags;

namespace AbcDisassembler;

internal class Sample
{
    public static AbcFile? ReadOneAbcFile(string path)
    {
        SwfFile swf;
        using(FileStream file = new(path, FileMode.Open, FileAccess.Read))
        {
            swf = SwfFile.ReadFrom(file);
        }

        foreach (SwfTagBase tag in swf.Tags)
        {
            if (tag is DoABCDefineTag doAbc)
                return AbcFile.Read(doAbc.ABCData);
        }

        return null;
    }

    public static void Main(string[] args)
    {
        string swfPath = args[0];
        AbcFile? abc = ReadOneAbcFile(swfPath);
    }
}
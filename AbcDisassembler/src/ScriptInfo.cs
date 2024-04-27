using System.Collections.Generic;

namespace AbcDisassembler;

public class ScriptInfo
{
    public uint Init { get; set; } // u30 index into method array of abcfile
    public uint TraitCount { get; set; } // u30
    public List<TraitInfo> Traits { get; set; } = [];

    public static ScriptInfo Read(ByteReader reader)
    {
        ScriptInfo info = new();
        info.Init = reader.ReadU30();
        info.TraitCount = reader.ReadU30();
        for (int i = 0; i < info.TraitCount; i++)
            info.Traits.Add(TraitInfo.Read(reader));
        
        return info;
    }
}
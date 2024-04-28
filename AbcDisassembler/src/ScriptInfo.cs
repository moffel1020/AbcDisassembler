using System.Collections.Generic;

namespace AbcDisassembler;

public class ScriptInfo
{
    public uint Init { get; set; } // u30 index into method array of abcfile
    public List<TraitInfo> Traits { get; set; } = null!; 

    public static ScriptInfo Read(ByteReader reader)
    {
        ScriptInfo info = new();
        info.Init = reader.ReadU30();
        int traitCount = (int)reader.ReadU30();
        info.Traits = new(traitCount);
        for (int i = 0; i < traitCount; i++)
            info.Traits.Add(TraitInfo.Read(reader));
        
        return info;
    }
}
using System.Collections.Generic;
using System.IO;

namespace AbcDisassembler;

public class ClassInfo
{
    public required uint Init { get; set; } // u30 index into method array of abcfile, static initializer
    public required List<TraitInfo> Traits { get; set; }

    internal static ClassInfo Read(BinaryReader reader)
    {
        uint init = reader.ReadAbcUInt30();

        int traitCount = (int)reader.ReadAbcUInt30();
        List<TraitInfo> traits = new(traitCount);
        for (int i = 0; i < traitCount; i++)
            traits.Add(TraitInfo.Read(reader));

        return new()
        {
            Init = init,
            Traits = traits
        };
    }
}
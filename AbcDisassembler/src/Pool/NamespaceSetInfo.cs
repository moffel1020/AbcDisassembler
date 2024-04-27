using System.Collections.Generic;

namespace AbcDisassembler;

public class  NamespaceSetInfo
{
    public uint Count { get; set; } // u30
    public List<uint> Namespaces { get; set; } = []; // indexes into constant pool namespace array

    public static NamespaceSetInfo Read(ByteReader reader)
    {
        NamespaceSetInfo info = new();
        info.Count = reader.ReadU8();
        for (int i = 0; i < info.Count; i++)
            info.Namespaces.Add(reader.ReadU30());

        return info;
    }
}
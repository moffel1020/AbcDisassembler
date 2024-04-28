using System.Collections.Generic;

namespace AbcDisassembler;

public class  NamespaceSetInfo
{
    public List<uint> Namespaces { get; set; } = null!; // indexes into constant pool namespace array

    public static NamespaceSetInfo Read(ByteReader reader)
    {
        NamespaceSetInfo info = new();
        int count = reader.ReadU8();
        info.Namespaces = new(count);
        for (int i = 0; i < count; i++)
            info.Namespaces.Add(reader.ReadU30());

        return info;
    }
}
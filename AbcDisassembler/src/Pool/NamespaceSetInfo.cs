using System.Collections.Generic;

namespace AbcDisassembler;

public class  NamespaceSetInfo
{
    public required List<uint> Namespaces { get; set; } // indexes into constant pool namespace array

    internal static NamespaceSetInfo Read(ByteReader reader)
    {
        int count = reader.ReadU8();
        List<uint> namespaces = new(count);
        for (int i = 0; i < count; i++)
            namespaces.Add(reader.ReadU30());

        return new()
        {
            Namespaces = namespaces
        };
    }
}
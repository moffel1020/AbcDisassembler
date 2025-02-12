using System.Collections.Generic;
using System.IO;

namespace AbcDisassembler;

public class NamespaceSetInfo
{
    public required List<uint> Namespaces { get; set; } // indexes into constant pool namespace array

    internal static NamespaceSetInfo Read(BinaryReader reader)
    {
        int count = reader.ReadByte();
        List<uint> namespaces = new(count);
        for (int i = 0; i < count; i++)
            namespaces.Add(reader.ReadAbcUInt30());

        return new()
        {
            Namespaces = namespaces
        };
    }
}
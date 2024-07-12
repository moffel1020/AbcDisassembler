using System.IO;

namespace AbcDisassembler;

public class NamespaceInfo
{
    public required NamespaceKind Kind { get; set; }
    public required uint Name { get; set; } // u30 index into constant pool string

    internal static NamespaceInfo Read(BinaryReader reader) => new()
    {
        Kind = (NamespaceKind)reader.ReadByte(),
        Name = reader.ReadAbcUInt30()
    };
}
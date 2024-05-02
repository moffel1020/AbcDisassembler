namespace AbcDisassembler;

public class NamespaceInfo
{
    public required NamespaceKind Kind { get; set; }
    public required uint Name { get; set; } // u30 -> index into constant pool string

    internal static NamespaceInfo Read(ByteReader reader) => new()
    {
        Kind = (NamespaceKind)reader.ReadU8(),
        Name = reader.ReadU30()
    };
}
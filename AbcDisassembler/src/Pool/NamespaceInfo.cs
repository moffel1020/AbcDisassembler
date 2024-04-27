namespace AbcDisassembler;

public class NamespaceInfo
{
    public NamespaceKind Kind { get; set; }
    public uint Name { get; set; } // u30 -> index into constant pool string

    public static NamespaceInfo Read(ByteReader reader) => new()
    {
        Kind = (NamespaceKind)reader.ReadU8(),
        Name = reader.ReadU30()
    };
}
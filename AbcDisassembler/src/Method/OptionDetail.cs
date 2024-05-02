namespace AbcDisassembler;

public class OptionDetail
{
    public required uint Value { get; set; }
    public required ConstantKind Kind { get; set; }

    public static OptionDetail Read(ByteReader reader) => new()
    {
        Value = reader.ReadU30(),
        Kind = (ConstantKind)reader.ReadU8()
    };
}
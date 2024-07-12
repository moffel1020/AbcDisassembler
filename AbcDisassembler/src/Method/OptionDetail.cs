using System.IO;

namespace AbcDisassembler;

public class OptionDetail
{
    public required uint Value { get; set; }
    public required ConstantKind Kind { get; set; }

    internal static OptionDetail Read(BinaryReader reader) => new()
    {
        Value = reader.ReadAbcUInt30(),
        Kind = (ConstantKind)reader.ReadByte()
    };
}
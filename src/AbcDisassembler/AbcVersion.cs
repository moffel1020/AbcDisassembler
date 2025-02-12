using System.IO;

namespace AbcDisassembler;

public struct AbcVersion
{
    public required ushort Minor { get; set; }
    public required ushort Major { get; set; }

    public readonly bool SupportsDecimal => Minor >= 17;
    public readonly bool SupportsFloat => Minor >= 16 && Major >= 47;

    internal static AbcVersion Read(BinaryReader reader) => new()
    {
        Minor = reader.ReadUInt16(),
        Major = reader.ReadUInt16(),
    };
}
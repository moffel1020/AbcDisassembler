using System.IO;

namespace AbcDisassembler;

public class ExceptionInfo
{
    public required uint From { get; set; } // u30
    public required uint To { get; set; } // u30
    public required uint Target { get; set; } // u30
    public required uint ExceptionType { get; set; } // u30 index into string array of cpool
    public required uint VarName { get; set; } // u30 index into string array of cpool

    internal static ExceptionInfo Read(BinaryReader reader) => new()
    {
        From = reader.ReadAbcUInt30(),
        To = reader.ReadAbcUInt30(),
        Target = reader.ReadAbcUInt30(),
        ExceptionType = reader.ReadAbcUInt30(),
        VarName = reader.ReadAbcUInt30()
    };
}
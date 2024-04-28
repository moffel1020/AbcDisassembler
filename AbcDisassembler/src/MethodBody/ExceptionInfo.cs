namespace AbcDisassembler;

public class ExceptionInfo
{
    public uint From { get; set; } // u30
    public uint To { get; set; } // u30
    public uint Target { get; set; } // u30
    public uint ExceptionType { get; set; } // u30 index into string array of cpool
    public uint VarName { get; set; } // u30 index into string array of cpool

    public static ExceptionInfo Read(ByteReader reader) => new()
    {
        From = reader.ReadU30(),
        To = reader.ReadU30(),
        Target = reader.ReadU30(),
        ExceptionType = reader.ReadU30(),
        VarName = reader.ReadU30()
    };
}
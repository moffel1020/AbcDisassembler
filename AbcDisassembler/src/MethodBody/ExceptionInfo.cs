namespace AbcDisassembler;

public class ExceptionInfo
{
    public required uint From { get; set; } // u30
    public required uint To { get; set; } // u30
    public required uint Target { get; set; } // u30
    public required uint ExceptionType { get; set; } // u30 index into string array of cpool
    public required uint VarName { get; set; } // u30 index into string array of cpool

    public static ExceptionInfo Read(ByteReader reader) => new()
    {
        From = reader.ReadU30(),
        To = reader.ReadU30(),
        Target = reader.ReadU30(),
        ExceptionType = reader.ReadU30(),
        VarName = reader.ReadU30()
    };
}
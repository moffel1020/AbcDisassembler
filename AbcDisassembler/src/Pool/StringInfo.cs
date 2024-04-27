using System.Text;

namespace AbcDisassembler;

public class StringInfo
{
    public uint Size { get; set; } // u30
    public string Data { get; set; } = null!;

    public static StringInfo Read(ByteReader reader)
    {
        StringInfo str = new();
        str.Size = reader.ReadU30();
        byte[] bytes = reader.ReadBytes(str.Size);
        str.Data = Encoding.UTF8.GetString(bytes, 0, (int)str.Size);

        return str;
    }
}
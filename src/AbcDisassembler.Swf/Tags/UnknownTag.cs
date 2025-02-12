using System.IO;

namespace AbcDisassembler.Swf.Tags;

public class UnknownTag(TagType type, byte[] data) : ITag
{
    public byte[] Data = data;
    public TagType Type { get; } = type;

    internal static UnknownTag Read(Stream stream, TagType type, uint length)
    {
        byte[] buf = new byte[length];
        stream.ReadExactly(buf);  
        return new(type, buf);
    }
}
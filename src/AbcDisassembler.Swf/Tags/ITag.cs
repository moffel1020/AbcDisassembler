using System.IO;

namespace AbcDisassembler.Swf.Tags;

public interface ITag
{
    public TagType Type { get; }

    internal static ITag Read(BinaryReader reader)
    {
        ushort typeAndLength = reader.ReadUInt16();
        TagType type = (TagType)(typeAndLength >> 6);
        uint length = (uint)typeAndLength & 0x3F;
        if (length == 0x3F)
            length = reader.ReadUInt32();

        // ZLibStream doesn't support the Position property for checking the read length
        // so use a MemoryStream to ensure we don't read beyond the end of tag
        byte[] tagBytes = new byte[length];
        reader.BaseStream.ReadExactly(tagBytes, 0, (int)length);
        using MemoryStream tagStream = new(tagBytes);
        BinaryReader tagReader = new(tagStream);

        return type switch
        {
            TagType.DoAbc => DoAbcTag.Read(tagReader),
            TagType.DoAbc2 => DoAbc2Tag.Read(tagReader),
            TagType.End => new EndTag(),
            _ => UnknownTag.Read(tagStream, type, length),
        };
    }
}
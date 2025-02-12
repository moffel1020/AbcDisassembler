using System;
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

        long begin = reader.BaseStream.Position;

        ITag tag = type switch
        {
            TagType.DoAbc => DoAbcTag.Read(reader),
            TagType.DoAbc2 => DoAbc2Tag.Read(reader),
            TagType.End => new EndTag(),
            _ => UnknownTag.Read(reader.BaseStream, type, length),
        };

        long end = reader.BaseStream.Position;
        if (end - begin != length)
            throw new Exception("Read beyond end of tag");

        return tag;
    }
}
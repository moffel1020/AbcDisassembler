using System.IO;

namespace AbcDisassembler.Swf.Tags;

public class DoAbcTag(AbcFile abc) : ITag
{
    public TagType Type => TagType.DoAbc;
    public AbcFile AbcFile { get; set; } = abc;

    internal static DoAbcTag Read(BinaryReader reader) => new(AbcFile.Read(reader.BaseStream));
}
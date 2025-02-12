using System.IO;
using System.Text;

namespace AbcDisassembler.Swf.Tags;

public class DoAbc2Tag(uint flags, string name, AbcFile abc) : ITag
{
    public TagType Type => TagType.DoAbc2;

    public uint Flags = flags;
    public string Name { get; set; } = name;
    public AbcFile AbcFile { get; set; } = abc;

    internal static DoAbc2Tag Read(BinaryReader reader)
    {
        uint flags = reader.ReadUInt32();
        string name = ReadSwfString(reader);
        AbcFile abc = AbcFile.Read(reader.BaseStream);

        return new(flags, name, abc);
    }

    private static string ReadSwfString(BinaryReader reader)
    {
        using MemoryStream stream = new();

        byte b = 1;
        while (b > 0)
        {
            b = reader.ReadByte();
            stream.WriteByte(b);
        }

        return Encoding.UTF8.GetString(stream.ToArray());
    }
}
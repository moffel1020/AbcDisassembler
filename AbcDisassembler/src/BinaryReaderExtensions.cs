using System.IO;

namespace AbcDisassembler;

internal static class BinaryReaderExtensions
{
    internal static uint ReadAbcUInt32(this BinaryReader reader)
    {
        uint number = 0;
        byte offset = 0;

        bool next = true;
        while (next && offset <= 35)
        {
            byte b = reader.ReadByte();
            next = (b >> 7) == 1;
            b &= 0x7F;

            number |= (uint)b << offset;
            offset += 7;
        }

        return number;
    }

    internal static uint ReadAbcUInt30(this BinaryReader reader) => ReadAbcUInt32(reader) & 0x3FFFFFFF;
    internal static int ReadAbcInt32(this BinaryReader reader) => unchecked((int)ReadAbcUInt32(reader));
    internal static int ReadInt24(this BinaryReader reader) => reader.ReadByte() | reader.ReadByte() << 8 | reader.ReadByte() << 24 >> 8;
}
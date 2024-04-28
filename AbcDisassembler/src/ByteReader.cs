using System;

namespace AbcDisassembler;

public class ByteReader(byte[] bytes)
{
    private readonly byte[] _buffer = bytes;
    private int _position = 0;

    public int Position => _position;

    public byte ReadU8()
    {
        if (_position >= _buffer.Length)
            throw new IndexOutOfRangeException("Reached end of byte array");
        
        return _buffer[_position++];
    }

    public ushort ReadU16() => (ushort)(ReadU8() | ReadU8() << 8);

    public int ReadS24() => ReadU8() | ReadU8() << 8 | (int)(ReadU8() << 24) >> 8;

    public long ReadS32() => BitConverter.ToInt64(BitConverter.GetBytes(ReadU32()));

    public uint ReadU30() => (uint)(ReadU32() & 0x3FFFFFFF);

    public ulong ReadU32()
    {
        ulong number = 0;
        byte offset = 0;

        bool next = true;
        while (next && offset <= 35)
        {
            byte b = ReadU8();
            next = (b >> 7) == 1;
            b &= 0x7F;

            number |= (ulong)b << offset;
            offset += 7;
        }

        return number;
    }

    public double ReadD64() => BitConverter.ToDouble(ReadBytes(8), 0);

    public byte[] ReadBytes(uint count)
    {
        if (_position + count > _buffer.Length) 
            throw new IndexOutOfRangeException($"Buffer will reach end by reading {count} bytes");

        byte[] result = new byte[count];
        for (int i = 0; i < count; i++) 
            result[i] = ReadU8();

        return result;
    }
}
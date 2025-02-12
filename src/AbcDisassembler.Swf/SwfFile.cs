using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using AbcDisassembler.Swf.Tags;

namespace AbcDisassembler.Swf;

// This swf implementation is only intended for reading/writing ABC related tags
public class SwfFile
{
    public required SwfHeader Header { get; set; }
    public required List<ITag> Tags { get; set; }

    public static SwfFile Read(Stream stream)
    {
        CompressionType compression = (CompressionType)stream.ReadByte();
        Span<byte> buf = [(byte)stream.ReadByte(), (byte)stream.ReadByte()];
        if (buf[0] != 'W' || buf[1] != 'S')
            throw new Exception("Invalid compression header");

        BinaryReader reader = new(stream);

        byte version = reader.ReadByte();
        uint fileLength = reader.ReadUInt32();

        // TODO
        if (compression != CompressionType.Zlib)
            throw new Exception("Only zlib compression is currently supported");

        byte[] decompressed = DecompressZlib(stream);

        MemoryStream decompressedStream = new(decompressed);
        reader = new(decompressedStream);

        byte[] rectBytes = ReadRectBytes(reader.BaseStream);
        ushort frameRate = reader.ReadUInt16();
        ushort frameCount = reader.ReadUInt16();

        SwfHeader header = new()
        {
            Version = version,
            FileLength = fileLength,
            FrameSizeBytes = rectBytes,
            FrameRate = frameRate,
            FrameCount = frameCount
        };

        List<ITag> tags = [];
        tags.Add(ITag.Read(reader));
        if (tags[0].Type != TagType.FileAttributes)
            throw new Exception($"First tag should be FileAttributes. Found {tags[0].Type}");

        while (tags[^1].Type != TagType.End)
            tags.Add(ITag.Read(reader));
        
        return new()
        {
            Header = header,
            Tags = tags,
        };
    }

    private static byte[] DecompressZlib(Stream input)
    {
        using MemoryStream outputStream = new();
        using (ZLibStream zlibStream = new(input, CompressionMode.Decompress))
        {
            zlibStream.CopyTo(outputStream);
        }

        return outputStream.ToArray();
    }

    private static byte[] ReadRectBytes(Stream stream)
    {
        byte b = (byte)stream.ReadByte();
        uint nBits = (uint)b >> 3;
        uint nBytes = (5 + 4 * nBits + 7) / 8;
        byte[] buffer = new byte[nBytes];
        buffer[0] = b;
        stream.ReadExactly(buffer, 1, (int)nBytes - 1);

        return buffer;
    }
}
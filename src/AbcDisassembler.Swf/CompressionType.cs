namespace AbcDisassembler.Swf;

public enum CompressionType : byte
{
    None = (byte)'F',
    Zlib = (byte)'C',
    Lzma = (byte)'Z',
}
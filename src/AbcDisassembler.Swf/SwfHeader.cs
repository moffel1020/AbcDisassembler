namespace AbcDisassembler.Swf;

public class SwfHeader
{
    public required byte Version { get; set; }
    public required uint FileLength { get; set; }
    public required byte[] FrameSizeBytes { get; set; }
    public required ushort FrameRate { get; set; }
    public required ushort FrameCount { get; set; }
}
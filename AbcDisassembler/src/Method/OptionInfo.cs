using System.Collections.Generic;

namespace AbcDisassembler;

public class OptionInfo
{
    public uint Count { get; set; } // u30
    public List<OptionDetail> Detail { get; set; } = [];

    public static OptionInfo Read(ByteReader reader)
    {
        OptionInfo info = new();
        info.Count = reader.ReadU30();
        for (int i = 0; i < info.Count; i++)
            info.Detail.Add(OptionDetail.Read(reader));

        return info;
    }
}
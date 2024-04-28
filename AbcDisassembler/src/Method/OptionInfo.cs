using System.Collections.Generic;

namespace AbcDisassembler;

public class OptionInfo
{
    public List<OptionDetail> Detail { get; set; } = null!;

    public static OptionInfo Read(ByteReader reader)
    {
        OptionInfo info = new();
        int count = (int)reader.ReadU30();
        info.Detail = new(count);
        for (int i = 0; i < count; i++)
            info.Detail.Add(OptionDetail.Read(reader));

        return info;
    }
}
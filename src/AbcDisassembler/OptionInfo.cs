using System.Collections.Generic;
using System.IO;

namespace AbcDisassembler;

public class OptionInfo
{
    public required List<OptionDetail> Detail { get; set; }

    internal static OptionInfo Read(BinaryReader reader)
    {
        int count = (int)reader.ReadAbcUInt30();
        List<OptionDetail> detail = new(count);
        for (int i = 0; i < count; i++)
            detail.Add(OptionDetail.Read(reader));

        return new()
        {
            Detail = detail
        };
    }
}
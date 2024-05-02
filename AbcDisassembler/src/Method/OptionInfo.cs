using System.Collections.Generic;

namespace AbcDisassembler;

public class OptionInfo
{
    public required List<OptionDetail> Detail { get; set; }

    internal static OptionInfo Read(ByteReader reader)
    {
        int count = (int)reader.ReadU30();
        List<OptionDetail> detail = new(count);
        for (int i = 0; i < count; i++)
            detail.Add(OptionDetail.Read(reader));

        return new()
        {
            Detail = detail
        };
    }
}
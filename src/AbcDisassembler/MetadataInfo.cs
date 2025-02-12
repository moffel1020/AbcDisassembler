using System.Collections.Generic;
using System.IO;

namespace AbcDisassembler;

public class MetadataInfo
{
    public required uint Name { get; set; } // u30 index into string constants
    public required List<ItemInfo> Items;

    internal static MetadataInfo Read(BinaryReader reader)
    {
        uint name = reader.ReadAbcUInt30();
        int itemCount = (int)reader.ReadAbcUInt30();
        List<ItemInfo> items = new(itemCount);
        for (int i = 0; i < itemCount; i++)
            items.Add(new ItemInfo());

        foreach (ItemInfo i in items)
            i.Key = reader.ReadAbcUInt30();
        foreach (ItemInfo i in items)
            i.Value = reader.ReadAbcUInt30();

        return new()
        {
            Name = name,
            Items = items
        };
    }
}
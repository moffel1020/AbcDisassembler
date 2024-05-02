using System.Collections.Generic;

namespace AbcDisassembler;

public class MetadataInfo
{
    public required uint Name { get; set; } // u30 index into string constants
    public required List<ItemInfo> Items;

    internal static MetadataInfo Read(ByteReader reader)
    {
        uint name = reader.ReadU30();
        int itemCount = (int)reader.ReadU30();
        List<ItemInfo> items = new(itemCount);
        for (int i = 0; i < itemCount; i++)
            items.Add(new ItemInfo());

        foreach (ItemInfo i in items)
            i.Key = reader.ReadU30();  
        foreach (ItemInfo i in items)
            i.Value = reader.ReadU30();

        return new()
        {
            Name = name,
            Items = items
        };
    }
}
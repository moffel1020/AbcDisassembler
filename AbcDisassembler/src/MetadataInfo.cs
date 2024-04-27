using System.Collections.Generic;

namespace AbcDisassembler;

public class MetadataInfo
{
    public uint Name { get; set; } // u30 index into string constants
    public uint ItemCount { get; set; } // u30
    public List<ItemInfo> Items = [];

    public static MetadataInfo Read(ByteReader reader)
    {
        MetadataInfo info = new();
        info.Name = reader.ReadU30();
        info.ItemCount = reader.ReadU30();
        for (int i = 0; i < info.ItemCount; i++)
            info.Items.Add(new ItemInfo());

        foreach (ItemInfo i in info.Items)
            i.Key = reader.ReadU30();  
        foreach (ItemInfo i in info.Items)
            i.Value = reader.ReadU30();

        return info;
    }
}
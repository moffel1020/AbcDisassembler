using System;
using System.Collections.Generic;

namespace AbcDisassembler;

public class TraitInfo
{
    public uint Name { get; set; } // u30 index to multiname array
    public TraitAttributes Attributes { get; set; }
    public TraitType Kind { get; set; }
    public BaseTrait Trait { get; set; } = null!;
    public List<uint>? Metadata { get; set; } // u30 index into metadata array of abcfile

    public static TraitInfo Read(ByteReader reader)
    {
        TraitInfo info = new();
        info.Name = reader.ReadU30();
        byte kindAttributes = reader.ReadU8();
        info.Kind = (TraitType)(kindAttributes & 0x0F);
        info.Attributes = (TraitAttributes)(kindAttributes >> 4);

        info.Trait = info.Kind switch
        {
            TraitType.Slot or TraitType.Const => SlotTrait.Read(reader),
            TraitType.Class => new ClassTrait(reader.ReadU30(), reader.ReadU30()),
            TraitType.Function => new FunctionTrait(reader.ReadU30(), reader.ReadU30()),
            TraitType.Method or TraitType.Getter or TraitType.Setter => new MethodTrait(reader.ReadU30(), reader.ReadU30()),
            _ => throw new InvalidOperationException($"Tried to read unkown TraitType {info.Kind}")
        };

        if (info.Attributes.HasFlag(TraitAttributes.Metadata))
        {
            int metadataCount = (int)reader.ReadU30();
            info.Metadata = new(metadataCount);
            for (int i = 0; i < metadataCount; i++)
                info.Metadata.Add(reader.ReadU30());
        }

        return info;
    }
}
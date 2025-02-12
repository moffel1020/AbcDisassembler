using System;
using System.Collections.Generic;
using System.IO;

namespace AbcDisassembler.Traits;

public class TraitInfo
{
    public required uint Name { get; set; } // u30 index to multiname array
    public required TraitAttributes Attributes { get; set; }
    public required TraitType Kind { get; set; }
    public required BaseTrait Trait { get; set; } = null!;
    public List<uint>? Metadata { get; set; } // u30 index into metadata array of abcfile

    internal static TraitInfo Read(BinaryReader reader)
    {
        uint name = reader.ReadAbcUInt30();
        byte kindAttributes = reader.ReadByte();
        TraitType kind = (TraitType)(kindAttributes & 0x0F);
        TraitAttributes attributes = (TraitAttributes)(kindAttributes >> 4);

        BaseTrait trait = kind switch
        {
            TraitType.Slot or TraitType.Const => SlotTrait.Read(reader),
            TraitType.Class => new ClassTrait(reader.ReadAbcUInt30(), reader.ReadAbcUInt30()),
            TraitType.Function => new FunctionTrait(reader.ReadAbcUInt30(), reader.ReadAbcUInt30()),
            TraitType.Method or TraitType.Getter or TraitType.Setter => new MethodTrait(reader.ReadAbcUInt30(), reader.ReadAbcUInt30()),
            _ => throw new InvalidOperationException($"Tried to read unkown TraitType {kind}")
        };

        List<uint>? metadata = null;
        if (attributes.HasFlag(TraitAttributes.Metadata))
        {
            int metadataCount = (int)reader.ReadAbcUInt30();
            metadata = new(metadataCount);
            for (int i = 0; i < metadataCount; i++)
                metadata.Add(reader.ReadAbcUInt30());
        }

        return new()
        {
            Name = name,
            Attributes = attributes,
            Kind = kind,
            Trait = trait,
            Metadata = metadata
        };
    }
}
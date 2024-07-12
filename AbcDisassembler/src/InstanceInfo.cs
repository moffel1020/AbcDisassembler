using System.Collections.Generic;
using System.IO;

namespace AbcDisassembler;

public class InstanceInfo
{
    public required uint Name { get; set; } // u30 index into multinames, name of class
    public required uint SuperName { get; set; } // u30 index into multiname, name of base class
    public required InstanceFlags Flags { get; set; }
    public required uint? ProtectedNs { get; set; } // u30 index into namespace array
    public required List<uint> Interfaces; // u30 index into multiname array
    public required uint Init { get; set; } // u30 index into method array of abcfile, constructor
    public required List<TraitInfo> Traits { get; set; } = null!;

    internal static InstanceInfo Read(BinaryReader reader)
    {
        uint name = reader.ReadAbcUInt30();
        uint superName = reader.ReadAbcUInt30();
        InstanceFlags flags = (InstanceFlags)reader.ReadByte();

        uint? protectedNs = null;
        if (flags.HasFlag(InstanceFlags.ProtectedNs))
            protectedNs = reader.ReadAbcUInt30();

        int interfaceCount = (int)reader.ReadAbcUInt30();
        List<uint> interfaces = new(interfaceCount);
        for (int i = 0; i < interfaceCount; i++)
            interfaces.Add(reader.ReadAbcUInt30());

        uint init = reader.ReadAbcUInt30();
        int traitCount = (int)reader.ReadAbcUInt30();
        List<TraitInfo> traits = new(traitCount);
        for (int i = 0; i < traitCount; i++)
            traits.Add(TraitInfo.Read(reader));

        return new()
        {
            Name = name,
            SuperName = superName,
            Flags = flags,
            ProtectedNs = protectedNs,
            Interfaces = interfaces,
            Init = init,
            Traits = traits
        };
    }
}
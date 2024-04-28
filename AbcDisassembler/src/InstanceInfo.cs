using System.Collections.Generic;

namespace AbcDisassembler;

public class InstanceInfo
{
    public uint Name { get; set; } // u30 index into multinames, name of class
    public uint SuperName { get; set; } // u30 index into multiname, name of base class
    public InstanceFlags Flags { get; set; }
    public uint? ProtectedNs { get; set; } // u30 index into namespace array
    public List<uint> Interfaces = null!; // u30 index into multiname array
    public uint Init { get; set; } // u30 index into method array of abcfile, constructor
    public List<TraitInfo> Traits { get; set; } = null!;

    public static InstanceInfo Read(ByteReader reader)
    {
        InstanceInfo info = new();
        info.Name = reader.ReadU30();
        info.SuperName = reader.ReadU30();
        info.Flags = (InstanceFlags)reader.ReadU8();
        if (info.Flags.HasFlag(InstanceFlags.ProtectedNs))
            info.ProtectedNs = reader.ReadU30();

        int interfaceCount = (int)reader.ReadU30();
        info.Interfaces = new(interfaceCount);
        for (int i = 0; i < interfaceCount; i++)
            info.Interfaces.Add(reader.ReadU30());
        
        info.Init = reader.ReadU30();
        int traitCount = (int)reader.ReadU30();
        info.Traits = new(traitCount);
        for (int i = 0; i < traitCount; i++)
            info.Traits.Add(TraitInfo.Read(reader));

        return info;
    }
}
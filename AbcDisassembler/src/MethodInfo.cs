using System.Collections.Generic;

namespace AbcDisassembler;

public class MethodInfo
{
    public uint ParamCount { get; set; } // u30
    public uint ReturnType { get; set; } // u30 index into ParamType array
    public List<uint> ParamTypes { get; set; } = []; // u30 indices to multiname constant array 
    public uint Name { get; set; } // u30 index into string array
    public MethodFlags Flags { get; set; } 
    public OptionInfo? Options { get; set; }
    public List<uint>? ParamNames { get; set; } // u30 index into cpool string array

    public static MethodInfo Read(ByteReader reader)
    {
        MethodInfo info = new();
        info.ParamCount = reader.ReadU30();
        info.ReturnType = reader.ReadU30();
        for (int i = 0; i < info.ParamCount; i++)
            info.ParamTypes.Add(reader.ReadU30());

        info.Name = reader.ReadU30();
        info.Flags = (MethodFlags)reader.ReadU8();

        if (info.Flags.HasFlag(MethodFlags.HasOptional))
            info.Options = OptionInfo.Read(reader);

        if (info.Flags.HasFlag(MethodFlags.HasParamNames))
        {
            info.ParamNames = [];
            for (int i = 0; i < info.ParamTypes.Count; i++)
                info.ParamNames.Add(reader.ReadU30());
        }

        return info;
    }
}
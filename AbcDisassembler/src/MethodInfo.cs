using System.Collections.Generic;

namespace AbcDisassembler;

public class MethodInfo
{
    public uint ReturnType { get; set; } // u30 index into ParamType array
    public List<uint> ParamTypes { get; set; } = null!; // u30 indices to multiname constant array 
    public uint Name { get; set; } // u30 index into string array
    public MethodFlags Flags { get; set; } 
    public OptionInfo? Options { get; set; }
    public List<uint>? ParamNames { get; set; } // u30 index into cpool string array

    public static MethodInfo Read(ByteReader reader)
    {
        MethodInfo info = new();

        int paramCount = (int)reader.ReadU30();
        info.ReturnType = reader.ReadU30();
        info.ParamTypes = new(paramCount);
        for (int i = 0; i < paramCount; i++)
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
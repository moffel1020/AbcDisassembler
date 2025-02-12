using System.Collections.Generic;
using System.IO;

namespace AbcDisassembler;

public class MethodInfo
{
    public required uint ReturnType { get; set; } // u30 index into ParamType array
    public required List<uint> ParamTypes { get; set; } // u30 indices to multiname constant array 
    public required uint Name { get; set; } // u30 index into string array
    public required MethodFlags Flags { get; set; }
    public OptionInfo? Options { get; set; }
    public List<uint>? ParamNames { get; set; } // u30 index into cpool string array

    internal static MethodInfo Read(BinaryReader reader)
    {
        int paramCount = (int)reader.ReadAbcUInt30();
        uint returnType = reader.ReadAbcUInt30();
        List<uint> paramTypes = new(paramCount);
        for (int i = 0; i < paramCount; i++)
            paramTypes.Add(reader.ReadAbcUInt30());

        uint name = reader.ReadAbcUInt30();
        MethodFlags flags = (MethodFlags)reader.ReadByte();

        OptionInfo? options = null;
        if (flags.HasFlag(MethodFlags.HasOptional))
            options = OptionInfo.Read(reader);

        List<uint>? paramNames = null;
        if (flags.HasFlag(MethodFlags.HasParamNames))
        {
            paramNames = new(paramTypes.Count);
            for (int i = 0; i < paramTypes.Count; i++)
                paramNames.Add(reader.ReadAbcUInt30());
        }

        return new()
        {
            ReturnType = returnType,
            ParamTypes = paramTypes,
            Name = name,
            Flags = flags,
            Options = options,
            ParamNames = paramNames
        };
    }
}
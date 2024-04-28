using System;
using System.Collections.Generic;

namespace AbcDisassembler;

public class MethodBodyInfo
{
    public uint Method { get; set; } // u30 index into methodinfo array
    public uint MaxStack { get; set; } // u30
    public uint LocalCount { get; set; } // u30
    public uint InitScopeDepth { get; set; } // u30
    public uint MaxScopeDepth { get; set; } // u30

    public uint CodeLength { get; set; } // u30
    public List<Instruction> Code { get; set; } = [];

    public List<ExceptionInfo> Exceptions { get; set; } = null!;
    public List<TraitInfo> Traits { get; set; } = null!;

    public static MethodBodyInfo Read(ByteReader reader, CPoolInfo cPool)
    {
        MethodBodyInfo info = new();
        info.Method = reader.ReadU30();
        info.MaxStack = reader.ReadU30();
        info.LocalCount = reader.ReadU30();
        info.InitScopeDepth = reader.ReadU30();        
        info.MaxScopeDepth = reader.ReadU30();

        info.CodeLength = reader.ReadU30();
        int endPos = (int)(reader.Position + info.CodeLength);
        while (reader.Position < endPos)
            info.Code.Add(Instruction.Read(reader, cPool));
        
        if (reader.Position > endPos) 
            throw new IndexOutOfRangeException("ByteReader read past end position while reading instructions");

        int exceptionCount = (int)reader.ReadU30();
        info.Exceptions = new(exceptionCount);
        for (int i = 0; i < exceptionCount; i++)
            info.Exceptions.Add(ExceptionInfo.Read(reader));

        int traitCount = (int)reader.ReadU30();
        info.Traits = new(traitCount);
        for (int i = 0; i < traitCount; i++)
            info.Traits.Add(TraitInfo.Read(reader));

        return info;
    }
}
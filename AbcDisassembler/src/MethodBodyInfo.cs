using System;
using System.Collections.Generic;

namespace AbcDisassembler;

public class MethodBodyInfo
{
    public required uint Method { get; set; } // u30 index into methodinfo array
    public required uint MaxStack { get; set; } // u30
    public required uint LocalCount { get; set; } // u30
    public required uint InitScopeDepth { get; set; } // u30
    public required uint MaxScopeDepth { get; set; } // u30

    public required uint CodeLength { get; set; } // u30
    public required List<Instruction> Code { get; set; }

    public required List<ExceptionInfo> Exceptions { get; set; }
    public required List<TraitInfo> Traits { get; set; }

    internal static MethodBodyInfo Read(ByteReader reader, CPoolInfo cPool)
    {
        uint method = reader.ReadU30();
        uint maxStack = reader.ReadU30();
        uint localCount = reader.ReadU30();
        uint initScopeDepth = reader.ReadU30();        
        uint maxScopeDepth = reader.ReadU30();

        uint codeLength = reader.ReadU30();
        int endPos = (int)(reader.Position + codeLength);
        List<Instruction> code = [];
        while (reader.Position < endPos)
            code.Add(Instruction.Read(reader, cPool));
        
        if (reader.Position > endPos) 
            throw new IndexOutOfRangeException("ByteReader read past end position while reading instructions");

        int exceptionCount = (int)reader.ReadU30();
        List<ExceptionInfo> exceptions = new(exceptionCount);
        for (int i = 0; i < exceptionCount; i++)
            exceptions.Add(ExceptionInfo.Read(reader));

        int traitCount = (int)reader.ReadU30();
        List<TraitInfo> traits = new(traitCount);
        for (int i = 0; i < traitCount; i++)
            traits.Add(TraitInfo.Read(reader));

        return new()
        {
            Method = method,
            MaxStack = maxStack,
            LocalCount = localCount,
            InitScopeDepth = initScopeDepth,
            MaxScopeDepth = maxScopeDepth,
            CodeLength = codeLength,
            Code = code,
            Exceptions = exceptions,
            Traits = traits
        };
    }
}
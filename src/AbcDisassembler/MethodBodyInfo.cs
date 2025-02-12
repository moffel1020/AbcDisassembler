using System;
using System.Collections.Generic;
using System.IO;
using AbcDisassembler.Instructions;
using AbcDisassembler.Traits;

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

    internal static MethodBodyInfo Read(BinaryReader reader, CPoolInfo cPool)
    {
        uint method = reader.ReadAbcUInt30();
        uint maxStack = reader.ReadAbcUInt30();
        uint localCount = reader.ReadAbcUInt30();
        uint initScopeDepth = reader.ReadAbcUInt30();
        uint maxScopeDepth = reader.ReadAbcUInt30();

        uint codeLength = reader.ReadAbcUInt30();
        int endPos = (int)(reader.BaseStream.Position + codeLength);
        List<Instruction> code = [];
        while (reader.BaseStream.Position < endPos)
            code.Add(Instruction.Read(reader, cPool));

        if (reader.BaseStream.Position > endPos)
            throw new IndexOutOfRangeException("BinaryReader read past end position while reading instructions");

        int exceptionCount = (int)reader.ReadAbcUInt30();
        List<ExceptionInfo> exceptions = new(exceptionCount);
        for (int i = 0; i < exceptionCount; i++)
            exceptions.Add(ExceptionInfo.Read(reader));

        int traitCount = (int)reader.ReadAbcUInt30();
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
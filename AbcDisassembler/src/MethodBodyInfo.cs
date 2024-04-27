using System.Collections.Generic;

namespace AbcDisassembler;

public class MethodBodyInfo
{
    public uint Methdod { get; set; } // u30 index into method array
    public uint MaxStack { get; set; } // u30
    public uint LocalCount { get; set; } // u30
    public uint InitScopeDepth { get; set; } // u30
    public uint MaxScopeDepth { get; set; } // u30

    public uint CodeLength { get; set; } // u30
    public List<Instruction> Code { get; set; } = [];

    public uint ExceptionCount { get; set; } // u30
    public List<ExceptionInfo> Exceptions { get; set; } = [];

    public uint TraitCount { get; set; } // u30
    public List<TraitInfo> Traits { get; set; } = [];
}
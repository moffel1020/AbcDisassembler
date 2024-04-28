using System;
using System.Collections.Generic;

namespace AbcDisassembler;

public class Argument(ArgType type)
{
    public object Value { get; set; } = null!;
    public ArgType Type { get; } = type;

    public object ReadValue(ByteReader reader, CPoolInfo cPool) => Type switch
    {
        ArgType.ByteLiteral or ArgType.UByteLiteral => reader.ReadU8(),
        ArgType.IntLiteral => reader.ReadS32(),
        ArgType.UintLiteral => reader.ReadU32(),
        ArgType.Int => cPool.Ints[(int)reader.ReadU30()],
        ArgType.Uint => cPool.Uints[(int)reader.ReadU30()],
        ArgType.Double => cPool.Doubles[(int)reader.ReadU30()],
        ArgType.String => cPool.Strings[(int)reader.ReadU30()],
        ArgType.Namespace => cPool.Namespaces[(int)reader.ReadU30()],
        ArgType.Multiname => cPool.Multinames[(int)reader.ReadU30()],
        ArgType.Class or ArgType.Method => reader.ReadU30(),
        
        // TODO: convert byte offset to instruction offset
        ArgType.JumpTarget => reader.ReadS24(),
        ArgType.SwitchDefaultTarget => reader.ReadS24(),
        ArgType.SwitchTargets => ReadSwitchTargets(reader),
        _ => throw new InvalidOperationException($"Tried to read unknown argument type {Type}")
    };

    private static List<int> ReadSwitchTargets(ByteReader reader)
    {
        int amount = (int)reader.ReadU30() + 1;
        List<int> targets = new(amount);
        for (int i = 0; i < amount; i++)
            targets.Add(reader.ReadS24());

        return targets;
    }
}
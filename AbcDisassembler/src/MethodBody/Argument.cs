using System;
using System.Collections.Generic;

namespace AbcDisassembler;

public class Argument
{
    public required object Value { get; set; }
    public ArgType Type { get; init; }

    internal static Argument Read(ByteReader reader, ArgType type, CPoolInfo cpool) => new()
    {
        Value = ReadValue(reader, type, cpool),
        Type = type
    };

    private static object ReadValue(ByteReader reader, ArgType type, CPoolInfo cpool) => type switch
    {
        ArgType.ByteLiteral or ArgType.UByteLiteral => reader.ReadU8(),
        ArgType.IntLiteral => reader.ReadS32(),
        ArgType.UintLiteral => reader.ReadU32(),
        ArgType.Int => cpool.Ints[(int)reader.ReadU30()],
        ArgType.Uint => cpool.Uints[(int)reader.ReadU30()],
        ArgType.Double => cpool.Doubles[(int)reader.ReadU30()],
        ArgType.String => cpool.Strings[(int)reader.ReadU30()],
        ArgType.Namespace => cpool.Namespaces[(int)reader.ReadU30()],
        ArgType.Multiname => cpool.Multinames[(int)reader.ReadU30()],
        ArgType.Class or ArgType.Method => reader.ReadU30(),
        
        // TODO: convert byte offset to instruction offset
        ArgType.JumpTarget => reader.ReadS24(),
        ArgType.SwitchDefaultTarget => reader.ReadS24(),
        ArgType.SwitchTargets => ReadSwitchTargets(reader),
        _ => throw new InvalidOperationException($"Tried to read unknown argument type {type}")
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
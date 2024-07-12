using System;
using System.Collections.Generic;
using System.IO;

namespace AbcDisassembler;

public class Argument
{
    public required object Value { get; set; }
    public ArgType Type { get; init; }

    internal static Argument Read(BinaryReader reader, ArgType type, CPoolInfo cpool) => new()
    {
        Value = ReadValue(reader, type, cpool),
        Type = type
    };

    private static object ReadValue(BinaryReader reader, ArgType type, CPoolInfo cpool) => type switch
    {
        ArgType.ByteLiteral or ArgType.UByteLiteral => reader.ReadByte(),
        ArgType.IntLiteral => reader.ReadAbcInt32(),
        ArgType.UintLiteral => reader.ReadAbcUInt32(),
        ArgType.Int => cpool.Ints[(int)reader.ReadAbcUInt30()],
        ArgType.Uint => cpool.Uints[(int)reader.ReadAbcUInt30()],
        ArgType.Double => cpool.Doubles[(int)reader.ReadAbcUInt30()],
        ArgType.String => cpool.Strings[(int)reader.ReadAbcUInt30()],
        ArgType.Namespace => cpool.Namespaces[(int)reader.ReadAbcUInt30()],
        ArgType.Multiname => cpool.Multinames[(int)reader.ReadAbcUInt30()],
        ArgType.Class or ArgType.Method => reader.ReadAbcUInt30(),

        // TODO: convert byte offset to instruction offset
        ArgType.JumpTarget => reader.ReadInt24(),
        ArgType.SwitchDefaultTarget => reader.ReadInt24(),
        ArgType.SwitchTargets => ReadSwitchTargets(reader),
        _ => throw new InvalidOperationException($"Tried to read unknown argument type {type}")
    };

    private static List<int> ReadSwitchTargets(BinaryReader reader)
    {
        int amount = (int)reader.ReadAbcUInt30() + 1;
        List<int> targets = new(amount);
        for (int i = 0; i < amount; i++)
            targets.Add(reader.ReadInt24());

        return targets;
    }
}
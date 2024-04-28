using System;
using System.Collections.Generic;
using System.Text;

namespace AbcDisassembler;

public class CPoolInfo
{
    public List<int> Ints { get; set; } = null!; // s32
    public List<uint> Uints { get; set; } = null!; // u32
    public List<double> Doubles { get; set; } = null!; // d64
    public List<string> Strings { get; set; } = null!; 
    public List<NamespaceInfo> Namespaces { get; set; } = null!; 
    public List<NamespaceSetInfo> NamespaceSets { get; set; } = null!; 
    public List<IBaseMultiname> Multinames { get; set; } = null!; 

    public static CPoolInfo Read(ByteReader reader)
    {
        CPoolInfo pool = new();

        int intCount = (int)reader.ReadU30();
        pool.Ints = new(intCount) { 0 };
        for (int i = 0; i < intCount - 1; i++)
            pool.Ints.Add((int)reader.ReadS32());

        int uintCount = (int)reader.ReadU30();
        pool.Uints = new(uintCount) { 0 };
        for (int i  = 0; i < uintCount - 1; i++)
            pool.Uints.Add((uint)reader.ReadU32());

        int doubleCount = (int)reader.ReadU30();
        pool.Doubles = new(doubleCount) { double.NaN };
        for (int i = 0; i < doubleCount - 1; i++)
            pool.Doubles.Add(reader.ReadD64());

        int stringCount = (int)reader.ReadU30();
        pool.Strings = new(stringCount) { "" };
        for (int i = 0; i < stringCount - 1; i++)
            pool.Strings.Add(ReadString(reader));

        int namespaceCount = (int)reader.ReadU30();
        pool.Namespaces = new(namespaceCount) { new NamespaceInfo() };
        for (int i = 0; i < namespaceCount - 1; i++)
            pool.Namespaces.Add(NamespaceInfo.Read(reader));

        int namespaceSetCount = (int)reader.ReadU30();
        pool.NamespaceSets = new(namespaceSetCount) { new NamespaceSetInfo() };
        for (int i = 0; i < namespaceSetCount - 1; i++)
            pool.NamespaceSets.Add(NamespaceSetInfo.Read(reader));

        int multinameCount = (int)reader.ReadU30();
        pool.Multinames = new(multinameCount) { new QName(0, 0) };
        for (int i = 0; i < multinameCount - 1; i++)
            pool.Multinames.Add(ReadMultiname(reader));

        return pool;
    }

    private static string ReadString(ByteReader reader)
    {
        uint length = reader.ReadU30();
        byte[] bytes = reader.ReadBytes(length);
        return Encoding.UTF8.GetString(bytes, 0, (int)length);
    }

    private static IBaseMultiname ReadMultiname(ByteReader reader)
    {
        MultinameKind kind = (MultinameKind)reader.ReadU8();
        IBaseMultiname mn = kind switch
        {
            MultinameKind.QName or MultinameKind.QNameA => new QName(reader.ReadU30(), reader.ReadU30()),
            MultinameKind.RTQName or MultinameKind.RTQNameA => new RTQName(reader.ReadU30()),
            MultinameKind.RTQNameL or MultinameKind.RTQNameLA => new RTQNameL(),
            MultinameKind.Multiname or MultinameKind.MultinameA => new Multiname(reader.ReadU30(), reader.ReadU30()),
            MultinameKind.MultinameL or MultinameKind.MultinameLA => new MultinameL(reader.ReadU30()),
            MultinameKind.TypeName => TypeName.Read(reader),
            _ => throw new InvalidOperationException($"Tried to read unknown MultinameKind {kind}")
        };

        return mn;
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AbcDisassembler;

public class CPoolInfo
{
    public required List<int> Ints { get; set; }
    public required List<uint> Uints { get; set; }
    public required List<double> Doubles { get; set; }
    public required List<string> Strings { get; set; }
    public required List<NamespaceInfo> Namespaces { get; set; }
    public required List<NamespaceSetInfo> NamespaceSets { get; set; }
    public required List<IBaseMultiname> Multinames { get; set; }

    internal static CPoolInfo Read(BinaryReader reader)
    {
        int intCount = (int)reader.ReadAbcUInt30();
        List<int> ints = new(intCount) { 0 };
        for (int i = 0; i < intCount - 1; i++)
            ints.Add(reader.ReadAbcInt32());

        int uintCount = (int)reader.ReadAbcUInt30();
        List<uint> uints = new(uintCount) { 0 };
        for (int i = 0; i < uintCount - 1; i++)
            uints.Add(reader.ReadAbcUInt32());

        int doubleCount = (int)reader.ReadAbcUInt30();
        List<double> doubles = new(doubleCount) { double.NaN };
        for (int i = 0; i < doubleCount - 1; i++)
            doubles.Add(reader.ReadDouble());

        int stringCount = (int)reader.ReadAbcUInt30();
        List<string> strings = new(stringCount) { "" };
        for (int i = 0; i < stringCount - 1; i++)
            strings.Add(ReadString(reader));

        int namespaceCount = (int)reader.ReadAbcUInt30();
        List<NamespaceInfo> namespaces = new(namespaceCount) { new NamespaceInfo() { Name = 0, Kind = NamespaceKind.Namespace } };
        for (int i = 0; i < namespaceCount - 1; i++)
            namespaces.Add(NamespaceInfo.Read(reader));

        int namespaceSetCount = (int)reader.ReadAbcUInt30();
        List<NamespaceSetInfo> namespaceSets = new(namespaceSetCount) { new NamespaceSetInfo() { Namespaces = [] } };
        for (int i = 0; i < namespaceSetCount - 1; i++)
            namespaceSets.Add(NamespaceSetInfo.Read(reader));

        int multinameCount = (int)reader.ReadAbcUInt30();
        List<IBaseMultiname> multinames = new(multinameCount) { new QName(0, 0) };
        for (int i = 0; i < multinameCount - 1; i++)
            multinames.Add(ReadMultiname(reader));

        return new()
        {
            Ints = ints,
            Uints = uints,
            Doubles = doubles,
            Strings = strings,
            Namespaces = namespaces,
            NamespaceSets = namespaceSets,
            Multinames = multinames
        };
    }

    private static string ReadString(BinaryReader reader)
    {
        uint length = reader.ReadAbcUInt30();
        byte[] bytes = reader.ReadBytes((int)length);
        return Encoding.UTF8.GetString(bytes, 0, (int)length);
    }

    private static IBaseMultiname ReadMultiname(BinaryReader reader)
    {
        MultinameKind kind = (MultinameKind)reader.ReadByte();
        IBaseMultiname mn = kind switch
        {
            MultinameKind.QName or MultinameKind.QNameA => new QName(reader.ReadAbcUInt30(), reader.ReadAbcUInt30()),
            MultinameKind.RTQName or MultinameKind.RTQNameA => new RTQName(reader.ReadAbcUInt30()),
            MultinameKind.RTQNameL or MultinameKind.RTQNameLA => new RTQNameL(),
            MultinameKind.Multiname or MultinameKind.MultinameA => new Multiname(reader.ReadAbcUInt30(), reader.ReadAbcUInt30()),
            MultinameKind.MultinameL or MultinameKind.MultinameLA => new MultinameL(reader.ReadAbcUInt30()),
            MultinameKind.TypeName => TypeName.Read(reader),
            _ => throw new InvalidOperationException($"Tried to read unknown MultinameKind {kind}")
        };

        return mn;
    }
}
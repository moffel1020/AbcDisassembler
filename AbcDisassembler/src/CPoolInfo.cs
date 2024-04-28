using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;

namespace AbcDisassembler;

public class CPoolInfo
{
    public uint IntCount { get; set; } // u30
    public List<int> Ints { get; set; } = null!; // s32

    public uint UintCount { get; set; } // u30
    public List<uint> Uints { get; set; } = null!; // u32

    public uint DoubleCount { get; set; } // u30
    public List<double> Doubles { get; set; } = null!; // d64

    public uint StringCount { get; set; } // u30
    public List<StringInfo> Strings { get; set; } = null!; 

    public uint NamespaceCount { get; set; } // u30
    public List<NamespaceInfo> Namespaces { get; set; } = null!; 

    public uint NamespaceSetCount { get; set; } // u30
    public List<NamespaceSetInfo> NamespaceSets { get; set; } = null!; 

    public uint MultinameCount { get; set; } // u30
    public List<MultinameInfo> Multinames { get; set; } = null!; 

    public static CPoolInfo Read(ByteReader reader)
    {
        CPoolInfo pool = new();

        pool.IntCount = reader.ReadU30();
        pool.Ints = new((int)pool.IntCount) { 0 };
        for (int i = 0; i < pool.IntCount - 1; i++)
            pool.Ints.Add((int)reader.ReadS32());

        pool.UintCount = reader.ReadU30();
        pool.Uints = new((int)pool.UintCount) { 0 };
        for (int i  = 0; i < pool.UintCount - 1; i++)
            pool.Uints.Add((uint)reader.ReadU32());

        pool.DoubleCount = reader.ReadU30();
        pool.Doubles = new((int)pool.DoubleCount) { double.NaN };
        for (int i = 0; i < pool.DoubleCount - 1; i++)
            pool.Doubles.Add(reader.ReadD64());

        pool.StringCount = reader.ReadU30();
        pool.Strings = new((int)pool.StringCount) { new StringInfo() { Data = "" } };
        for (int i = 0; i < pool.StringCount - 1; i++)
            pool.Strings.Add(StringInfo.Read(reader));

        pool.NamespaceCount = reader.ReadU30();
        pool.Namespaces = new((int)pool.NamespaceCount) { new NamespaceInfo() };
        for (int i = 0; i < pool.NamespaceCount - 1; i++)
            pool.Namespaces.Add(NamespaceInfo.Read(reader));

        pool.NamespaceSetCount = reader.ReadU30();
        pool.NamespaceSets = new((int)pool.NamespaceSetCount) { new NamespaceSetInfo() };
        for (int i = 0; i < pool.NamespaceSetCount - 1; i++)
            pool.NamespaceSets.Add(NamespaceSetInfo.Read(reader));

        pool.MultinameCount = reader.ReadU30();
        pool.Multinames = new((int)pool.MultinameCount) { new MultinameInfo() };
        for (int i = 0; i < pool.MultinameCount - 1; i++)
            pool.Multinames.Add(MultinameInfo.Read(reader));

        return pool;
    }
}
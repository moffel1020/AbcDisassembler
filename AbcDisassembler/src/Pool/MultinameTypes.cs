using System.Collections.Generic;
using System.IO;

namespace AbcDisassembler;

public interface IBaseMultiname
{
    public MultinameKind Kind { get; }
}

public interface INamedMultiname : IBaseMultiname
{
    public uint Name { get; set; }
}

public class QName(uint ns, uint name) : INamedMultiname
{
    public uint Namespace { get; set; } = ns; // u30 index into namespace constants
    public uint Name { get; set; } = name; // u30 index into string constants
    public MultinameKind Kind => MultinameKind.QName;
}

public class RTQName(uint name) : INamedMultiname
{
    public uint Name { get; set; } = name; // u30 index into string constants
    public MultinameKind Kind => MultinameKind.RTQName;
}

public class RTQNameL : IBaseMultiname
{
    public MultinameKind Kind => MultinameKind.RTQNameL;
}

public class Multiname(uint name, uint nsSet) : INamedMultiname
{
    public uint Name { get; set; } = name; // u30 index into string constants
    public uint NamespaceSet { get; set; } = nsSet; // u30 index into namespace_set constants
    public MultinameKind Kind => MultinameKind.Multiname;
}

public class MultinameL(uint nsSet) : IBaseMultiname
{
    public uint NamespaceSet { get; set; } = nsSet; // u30 index into namespace_sets
    public MultinameKind Kind => MultinameKind.MultinameL;
}

public class TypeName() : INamedMultiname
{
    public required uint Name { get; set; }
    public required List<uint> Params { get; set; }
    public MultinameKind Kind => MultinameKind.TypeName;

    internal static TypeName Read(BinaryReader reader)
    {
        uint name = reader.ReadAbcUInt30();

        uint length = reader.ReadAbcUInt30();
        List<uint> parameters = new((int)length);
        for (int i = 0; i < length; i++)
            parameters.Add(reader.ReadAbcUInt30());

        return new()
        {
            Name = name,
            Params = parameters
        };
    }
}
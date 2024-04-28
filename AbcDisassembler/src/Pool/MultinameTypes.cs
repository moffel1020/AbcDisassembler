using System.Collections.Generic;

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
    public uint Name { get; set; }
    public List<uint> Params { get; set; } = [];
    public MultinameKind Kind => MultinameKind.TypeName;

    public static TypeName Read(ByteReader reader)
    {
        TypeName tn = new();
        tn.Name = reader.ReadU30();

        uint length = reader.ReadU30();
        for (int i = 0; i < length; i++)
            tn.Params.Add(reader.ReadU30());

        return tn;
    }
}
using System.Collections.Generic;

namespace AbcDisassembler;

public abstract class BaseMultiname { }

public class QName(uint ns, uint name) : BaseMultiname
{
    public uint Namespace { get; set; } = ns; // u30 index into namespace constants
    public uint Name { get; set; } = name; // u30 index into string constants
}

public class RTQName(uint name) : BaseMultiname
{
    public uint Name { get; set; } = name; // u30 index into string constants
}

public class RTQNameL : BaseMultiname
{

}

public class Multiname(uint name, uint nsSet) : BaseMultiname
{
    public uint Name { get; set; } = name; // u30 index into string constants
    public uint NamespaceSet { get; set; } = nsSet; // u30 index into namespace_set constants
}

public class MultinameL(uint nsSet) : BaseMultiname
{
    public uint NamespaceSet { get; set; } = nsSet; // u30 index into namespace_sets
}

public class TypeName() : BaseMultiname
{
    public uint Name { get; set; }
    public List<uint> Params { get; set; } = [];

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
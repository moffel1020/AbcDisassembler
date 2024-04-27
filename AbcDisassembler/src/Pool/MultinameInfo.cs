using System;

namespace AbcDisassembler;

public class MultinameInfo
{
    public MultinameKind Kind { get; set; }
    public  BaseMultiname Multiname { get; set; } = default!;

    public static MultinameInfo Read(ByteReader reader)
    {
        MultinameInfo info = new();
        info.Kind = (MultinameKind)reader.ReadU8();
        info.Multiname = info.Kind switch
        {
            MultinameKind.QName or MultinameKind.QNameA => new QName(reader.ReadU30(), reader.ReadU30()),
            MultinameKind.RTQName or MultinameKind.RTQNameA => new RTQName(reader.ReadU30()),
            MultinameKind.RTQNameL or MultinameKind.RTQNameLA => new RTQNameL(),
            MultinameKind.Multiname or MultinameKind.MultinameA => new Multiname(reader.ReadU30(), reader.ReadU30()),
            MultinameKind.MultinameL or MultinameKind.MultinameLA => new MultinameL(reader.ReadU30()),
            MultinameKind.TypeName => TypeName.Read(reader),
            _ => throw new InvalidOperationException($"Tried to read unknown MultinameKind {info.Kind}")
        };

        return info;
    }
}
namespace AbcDisassembler;

public abstract class BaseTrait { }

public class SlotTrait() : BaseTrait
{
    public uint Id { get; set; } // u30
    public uint TypeName { get; set; } // u30 index into multiname array
    public uint VIndex { get; set; } // u30
    public ConstantKind? VKind { get; set; }

    internal static SlotTrait Read(ByteReader reader)
    {
        uint id = reader.ReadU30();
        uint typeName = reader.ReadU30();
        uint vIndex = reader.ReadU30();
        ConstantKind? vKind = vIndex > 0 ? (ConstantKind)reader.ReadU8() : null;

        return new()
        {
            Id = id,
            TypeName = typeName,
            VIndex = vIndex,
            VKind = vKind
        };
    }
}

public class ClassTrait(uint id, uint index ) : BaseTrait
{
    public uint Id { get; set; } = id; // u30
    public uint Index { get; set; } = index; // u30 index into class array of abcfile
}

public class FunctionTrait(uint id, uint function) : BaseTrait
{
    public uint Id { get; set; } = id; // u30
    public uint Function { get; set; } = function; // u30 into methdo array of abcfile
}

public class MethodTrait(uint id, uint method) : BaseTrait
{
    public uint Id { get; set; } = id; // u30
    public uint Method { get; set; } = method; // u30 into method array of abcfile
}
namespace AbcDisassembler.Instructions;

public enum ArgType
{
    ByteLiteral,
    UByteLiteral,
    IntLiteral,
    UintLiteral,

    Int,
    Uint,
    Double,
    Float,
    Decimal,
    String,
    Namespace,
    Multiname,
    Class,
    Method,

    JumpTarget,
    SwitchDefaultTarget,
    SwitchTargets,
}
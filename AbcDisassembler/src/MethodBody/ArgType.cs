namespace AbcDisassembler;

public enum ArgType
{
    ByteLiteral,
    UByteLiteral,
    IntLiteral,
    UintLiteral,

    Int,
    Uint,
    Double,
    String,
    Namespace,
    Multiname,
    Class,
    Method,

    JumpTarget,
    SwitchDefaultTarget,
    SwitchTargets,
}
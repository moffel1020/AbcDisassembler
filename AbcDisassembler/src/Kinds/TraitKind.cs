namespace AbcDisassembler;

public enum TraitType
{
    Slot = 0,
    Method = 1,
    Getter = 2,
    Setter = 3,
    Class = 4,
    Function = 5,
    Const = 6    
}

public enum TraitAttributes
{
    None = 0x0,
    Final = 0x1,
    Override = 0x2,
    Metadata = 0x4
}
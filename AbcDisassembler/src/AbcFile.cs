using System.Collections.Generic;

namespace AbcDisassembler;

public class AbcFile()
{
    public uint MinorVersion { get; set; } // u16
    public uint MajorVersion { get; set; } // u16

    public CPoolInfo ConstantPool { get; set; } = null!;

    public uint MethodCount { get; set; } // u30
    public List<MethodInfo> Methods { get; set; } = [];

    public uint MetadataCount { get; set; } // u30
    public List<MetadataInfo> Metadata { get; set; } = [];

    public uint ClassCount { get; set; } // u30
    public List<InstanceInfo> Instances { get; set; } = [];
    public List<ClassInfo> Classes { get; set;} = [];

    public uint ScriptCount { get; set; } // u30
    public List<ScriptInfo> Scripts { get; set; } = [];

    public uint MethodBodyCount { get; set; } // u30
    public List<MethodBodyInfo> MethodBodies { get; set; } = [];

    public static AbcFile Read(byte[] bytes)
    {
        ByteReader reader = new(bytes);
        AbcFile abc = new();

        abc.MinorVersion = reader.ReadU16();
        abc.MajorVersion = reader.ReadU16();

        abc.ConstantPool = CPoolInfo.Read(reader);

        abc.MethodCount = reader.ReadU30();
        for (int i = 0; i < abc.MethodCount; i++)
            abc.Methods.Add(MethodInfo.Read(reader));

        abc.MetadataCount = reader.ReadU30();
        for (int i = 0; i < abc.MetadataCount; i++)
            abc.Metadata.Add(MetadataInfo.Read(reader));

        abc.ClassCount = reader.ReadU30();
        for (int i = 0; i < abc.ClassCount; i++)
            abc.Instances.Add(InstanceInfo.Read(reader));
        
        for (int i = 0; i < abc.ClassCount; i++)
            abc.Classes.Add(ClassInfo.Read(reader));

        abc.ScriptCount = reader.ReadU30(); 
        for (int i = 0; i < abc.ScriptCount; i++)
            abc.Scripts.Add(ScriptInfo.Read(reader));

        // TODO: MethodBody
        
        return abc;
    }
}
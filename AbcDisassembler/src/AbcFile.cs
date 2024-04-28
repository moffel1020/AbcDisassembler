using System.Collections.Generic;

namespace AbcDisassembler;

public class AbcFile()
{
    public uint MinorVersion { get; set; } // u16
    public uint MajorVersion { get; set; } // u16

    public CPoolInfo ConstantPool { get; set; } = null!;
    public List<MethodInfo> Methods { get; set; } = null!;
    public List<MetadataInfo> Metadata { get; set; } = null!;
    public List<InstanceInfo> Instances { get; set; } = null!;
    public List<ClassInfo> Classes { get; set;} = null!;
    public List<ScriptInfo> Scripts { get; set; } = null!;
    public List<MethodBodyInfo> MethodBodies { get; set; } = null!;

    public static AbcFile Read(byte[] bytes)
    {
        ByteReader reader = new(bytes);
        AbcFile abc = new();

        abc.MinorVersion = reader.ReadU16();
        abc.MajorVersion = reader.ReadU16();

        abc.ConstantPool = CPoolInfo.Read(reader);

        int methodCount = (int)reader.ReadU30();
        abc.Methods = new(methodCount);
        for (int i = 0; i < methodCount; i++)
            abc.Methods.Add(MethodInfo.Read(reader));

        int metadataCount = (int)reader.ReadU30();
        abc.Metadata = new(metadataCount);
        for (int i = 0; i < metadataCount; i++)
            abc.Metadata.Add(MetadataInfo.Read(reader));

        int classCount = (int)reader.ReadU30();
        abc.Instances = new(classCount);
        for (int i = 0; i < classCount; i++)
            abc.Instances.Add(InstanceInfo.Read(reader));
        
        abc.Classes = new(classCount);
        for (int i = 0; i < classCount; i++)
            abc.Classes.Add(ClassInfo.Read(reader));

        int scriptCount = (int)reader.ReadU30(); 
        abc.Scripts = new(scriptCount);
        for (int i = 0; i < scriptCount; i++)
            abc.Scripts.Add(ScriptInfo.Read(reader));

        // TODO: MethodBody
        
        return abc;
    }
}
using System.Collections.Generic;
using System.IO;

namespace AbcDisassembler;

public class AbcFile
{
    public required AbcVersion Version { get; set; }
    public required CPoolInfo ConstantPool { get; set; }
    public required List<MethodInfo> Methods { get; set; }
    public required List<MetadataInfo> Metadata { get; set; }
    public required List<InstanceInfo> Instances { get; set; }
    public required List<ClassInfo> Classes { get; set; }
    public required List<ScriptInfo> Scripts { get; set; }
    public required List<MethodBodyInfo> MethodBodies { get; set; }

    public static AbcFile Read(Stream stream)
    {
        BinaryReader reader = new(stream);

        AbcVersion version = AbcVersion.Read(reader);

        CPoolInfo cpool = CPoolInfo.Read(reader, version);

        int methodCount = (int)reader.ReadAbcUInt30();
        List<MethodInfo> methods = new(methodCount);
        for (int i = 0; i < methodCount; i++)
            methods.Add(MethodInfo.Read(reader));

        int metadataCount = (int)reader.ReadAbcUInt30();
        List<MetadataInfo> metadata = new(metadataCount);
        for (int i = 0; i < metadataCount; i++)
            metadata.Add(MetadataInfo.Read(reader));

        int classCount = (int)reader.ReadAbcUInt30();
        List<InstanceInfo> instances = new(classCount);
        for (int i = 0; i < classCount; i++)
            instances.Add(InstanceInfo.Read(reader));

        List<ClassInfo> classes = new(classCount);
        for (int i = 0; i < classCount; i++)
            classes.Add(ClassInfo.Read(reader));

        int scriptCount = (int)reader.ReadAbcUInt30();
        List<ScriptInfo> scripts = new(scriptCount);
        for (int i = 0; i < scriptCount; i++)
            scripts.Add(ScriptInfo.Read(reader));

        int methodBodyCount = (int)reader.ReadAbcUInt30();
        List<MethodBodyInfo> methodBodies = new(methodBodyCount);
        for (int i = 0; i < methodBodyCount; i++)
            methodBodies.Add(MethodBodyInfo.Read(reader, cpool));

        return new()
        {
            Version = version,
            ConstantPool = cpool,
            Methods = methods,
            Metadata = metadata,
            Instances = instances,
            Classes = classes,
            Scripts = scripts,
            MethodBodies = methodBodies
        };
    }
}
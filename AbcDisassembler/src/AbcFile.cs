﻿using System.Collections.Generic;

namespace AbcDisassembler;

public class AbcFile()
{
    public ushort MinorVersion { get; set; }
    public ushort MajorVersion { get; set; }

    public required CPoolInfo ConstantPool { get; set; }
    public required List<MethodInfo> Methods { get; set; }
    public required List<MetadataInfo> Metadata { get; set; }
    public required List<InstanceInfo> Instances { get; set; }
    public required List<ClassInfo> Classes { get; set; }
    public required List<ScriptInfo> Scripts { get; set; }
    public required List<MethodBodyInfo> MethodBodies { get; set; }

    public static AbcFile Read(byte[] bytes)
    {
        ByteReader reader = new(bytes);

        ushort minorVersion = reader.ReadU16();
        ushort majorVersion = reader.ReadU16();

        CPoolInfo cpool = CPoolInfo.Read(reader);

        int methodCount = (int)reader.ReadU30();
        List<MethodInfo> methods = new(methodCount);
        for (int i = 0; i < methodCount; i++)
            methods.Add(MethodInfo.Read(reader));

        int metadataCount = (int)reader.ReadU30();
        List<MetadataInfo> metadata = new(metadataCount);
        for (int i = 0; i < metadataCount; i++)
            metadata.Add(MetadataInfo.Read(reader));

        int classCount = (int)reader.ReadU30();
        List<InstanceInfo> instances = new(classCount);
        for (int i = 0; i < classCount; i++)
            instances.Add(InstanceInfo.Read(reader));

        List<ClassInfo> classes = new(classCount);
        for (int i = 0; i < classCount; i++)
            classes.Add(ClassInfo.Read(reader));

        int scriptCount = (int)reader.ReadU30();
        List<ScriptInfo> scripts = new(scriptCount);
        for (int i = 0; i < scriptCount; i++)
            scripts.Add(ScriptInfo.Read(reader));

        int methodBodyCount = (int)reader.ReadU30();
        List<MethodBodyInfo> methodBodies = new(methodBodyCount);
        for (int i = 0; i < methodBodyCount; i++)
            methodBodies.Add(MethodBodyInfo.Read(reader, cpool));

        return new()
        {
            MinorVersion = minorVersion,
            MajorVersion = majorVersion,
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
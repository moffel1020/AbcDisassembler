using System;
using System.IO;
using SwfLib;
using SwfLib.Tags;
using SwfLib.Tags.ActionsTags;
using AbcDisassembler.Multinames;

namespace AbcDisassembler.Sample;

public class Program
{
    private static AbcFile? ReadOneAbcFile(string path)
    {
        SwfFile swf;
        using (FileStream file = new(path, FileMode.Open, FileAccess.Read))
        {
            swf = SwfFile.ReadFrom(file);
        }

        foreach (SwfTagBase tag in swf.Tags)
        {
            if (tag is DoABCDefineTag doAbc)
                return AbcFile.Read(new MemoryStream(doAbc.ABCData));
        }

        return null;
    }

    private static void PrintClassNames(AbcFile abc)
    {
        foreach (InstanceInfo instance in abc.Instances)
        {
            IMultiname mn = abc.ConstantPool.Multinames[(int)instance.Name];
            if (mn is INamedMultiname named)
            {
                string name = abc.ConstantPool.Strings[(int)named.Name];
                Console.WriteLine(name);
            }
        }
    }

    public static void Main(string[] args)
    {
        string swfPath = args[0];
        AbcFile abc = ReadOneAbcFile(swfPath) ?? throw new Exception();
        PrintClassNames(abc);
    }
}
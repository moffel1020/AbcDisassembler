using System;
using System.Collections.Generic;

namespace AbcDisassembler;

public class ClassInfo
{
     public uint Init { get; set; } // u30 index into method array of abcfile, static initializer
     public uint TraitCount { get; set; } // u30
     public List<TraitInfo> Traits { get; set; } = [];

     public static ClassInfo Read(ByteReader reader)
     {
          ClassInfo info = new();
          info.Init = reader.ReadU30();
          info.TraitCount = reader.ReadU30();
          for (int i = 0; i < info.TraitCount; i++)
               info.Traits.Add(TraitInfo.Read(reader));
          
          return info;
     }
}
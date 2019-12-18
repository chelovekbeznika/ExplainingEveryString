using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExplainingEveryString.Core.Music
{
    internal static class DeltaSamplesLibraryLoader
    {
        internal static List<Byte[]> Load(String path)
        {
            List<Byte[]> result = new List<Byte[]>();
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                Int32 samplesInFile = fileStream.ReadByte();
                foreach (Int32 sampleNumber in Enumerable.Range(0, samplesInFile))
                {
                    Int32 highByte = fileStream.ReadByte();
                    Int32 lowerByte = fileStream.ReadByte();
                    Int32 bytesInSample = (highByte << 8) + lowerByte;
                    Byte[] sample = new Byte[bytesInSample];
                    fileStream.Read(sample, 0, bytesInSample);
                    result.Add(sample);
                }
            }
            return result;
        }
    }
}

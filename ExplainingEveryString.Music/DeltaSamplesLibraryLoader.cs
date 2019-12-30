using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExplainingEveryString.Music
{
    internal static class DeltaSamplesLibraryLoader
    {
        internal static List<Byte[]> Load(String path)
        {
            var result = new List<Byte[]>();
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var samplesInFile = fileStream.ReadByte();
                foreach (var sampleNumber in Enumerable.Range(0, samplesInFile))
                {
                    var highByte = fileStream.ReadByte();
                    var lowerByte = fileStream.ReadByte();
                    var bytesInSample = (highByte << 8) + lowerByte;
                    var sample = new Byte[bytesInSample];
                    fileStream.Read(sample, 0, bytesInSample);
                    result.Add(sample);
                }
            }
            return result;
        }
    }
}

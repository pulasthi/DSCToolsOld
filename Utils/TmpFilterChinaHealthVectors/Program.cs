using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common;

namespace TmpFilterChinaHealthVectors
{
    class Program
    {
        static void Main(string[] args)
        {
            var vectorFile = @"E:\Sali\InCloud\IUBox\Box Sync\SalsaBio\chinahealth\vectors\paired\train.test.max.txt";
            var minDefCompCount = 40; // i.e. remove any vector with < 40 defined components
            var filteredVectorFile = Path.Combine(Path.GetDirectoryName(vectorFile) ?? string.Empty,
                Path.GetFileNameWithoutExtension(vectorFile) + ".mindefcompcount." + minDefCompCount + ".txt");
            var filteredVectorMetaFile = Path.Combine(Path.GetDirectoryName(vectorFile) ?? string.Empty,
                Path.GetFileNameWithoutExtension(vectorFile) + ".mindefcompcount." + minDefCompCount + "meta.txt");
            using (var reader = new VectorPointsReader(vectorFile, true, true))
            {
                using (StreamWriter writer = new StreamWriter(filteredVectorFile), metaWriter = new StreamWriter(filteredVectorMetaFile))
                {
                    writer.WriteLine(reader.Header);
                    metaWriter.WriteLine("idx\tglobalidx\tname");
                    var globalIdx = 0;
                    var count = 0;
                    while (!reader.EndOfStream)
                    {
                        var vector = reader.ReadVectorPoint();
                        if (vector.NonZerComponentCount >= minDefCompCount)
                        {
                            writer.WriteLine(vector.Name +"\t" + String.Join("\t",vector.Vec));
                            metaWriter.WriteLine(count++ + "\t" + globalIdx + "\t" + vector.Name);
                        }
                        ++globalIdx;
                    }
                }
            }
        }
    }
}

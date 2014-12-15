using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RawBlastToTrans11
{
    class Program
    {
        static void Main(string[] args)
        {
            string rawBlastDistanceFile =
                @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\correlation\matrices\cog_95672_bitscore_refined_c#_selected_7_wo_cog4608.bin";

            int size = 794;

            string trans11DoubleDistanceFile =
                @"G:\SugarSyncSharedByMe\SalsaBio\cog\100k\correlation\matrices\cog_95672_bitscore_refined_c#_selected_7_wo_cog4608_trans11_distance_double.bin";

            using (BinaryReader reader = new BinaryReader(File.OpenRead(rawBlastDistanceFile)))
            {
                using (BinaryWriter writer = new BinaryWriter(File.Create(trans11DoubleDistanceFile)))
                {
                    size = size*size;
                    double d;
                    for (int i = 0; i < size; ++i)
                    {
                        d = ((double) reader.ReadInt16())/Int16.MaxValue;
                        d = 1.0 - (Math.Pow((1 - d), 0.125));
                        writer.Write(d);
                    }
                }
            }
        }
    }
}

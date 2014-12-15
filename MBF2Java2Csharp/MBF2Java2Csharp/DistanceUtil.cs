using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBF2Java2Csharp
{
    public class DistanceUtil
    {
        public static float ComputePercentIdentityDistance(AlignedData ad)
        {
            Sequence alignedSeqA = ad.FirstAlignedSequence;
            Sequence alignedSeqB = ad.SecondAlignedSequence;
            float identity = 0.0f;
            for (int i = 0; i < alignedSeqA.Count; i++)
            {
                if (alignedSeqA.Get(i) == alignedSeqB.Get(i))
                {
                    identity++;
                }

            }
            return (1.0f - (identity / alignedSeqA.Count));
        }

        public static short ComputePercentIdentityDistanceAsShort(AlignedData ad)
        {
            return (short)(short.MaxValue * ComputePercentIdentityDistance(ad));
        }
    }
}

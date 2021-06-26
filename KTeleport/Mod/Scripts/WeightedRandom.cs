using System;

namespace KTeleport
{
    public class WeightedRandom : Random
    {
            // adjusts the normal curve toward the right (higher) end depending on number of chargesdused. 
            public float Weight { get; set; }

            protected override double Sample()
            {
                return Math.Pow(base.Sample(),Weight);
            }
            public override int Next(int MaxValue)
            {
                return (int) (Sample() * MaxValue);
            }
            
    }
}
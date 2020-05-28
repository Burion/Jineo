using System;

namespace Jineo.Logic
{
    public class MildnessBlockAnalizer : IBlockAnalizer
    {
        public float Analize(DataBlock block)
        {
            var pressure = block.Data[(int)MeteringType.Pressure];
            var density = block.Data[(int)MeteringType.Density];
            var waterproof = block.Data[(int)MeteringType.Waterproof];

            var value = (3*density + 2*pressure) * waterproof;
            return value;
        }
    }
}
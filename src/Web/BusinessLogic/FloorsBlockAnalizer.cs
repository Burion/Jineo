using System;

namespace Jineo.Logic
{
    public class FloorsBlockAnalizer : IBlockAnalizer
    {
        public float Analize(DataBlock block)
        {
            MildnessBlockAnalizer mildnessBlock = new MildnessBlockAnalizer();
            var mildness = mildnessBlock.Analize(block);
            var density = block.Data[(int)MeteringType.Density];

            var value = mildness/density;
            return (float)value;

            
        }
    }
}
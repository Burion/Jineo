using System;

namespace Jineo.Logic
{
    public class BlockTypeBlockAnalizer : IBlockAnalizer
    {
        public float Analize(DataBlock block)
        {
            MildnessBlockAnalizer mildnessBlock = new MildnessBlockAnalizer();
            var mildness = mildnessBlock.Analize(block);

            var pressure = block.Data[(int)MeteringType.Pressure];
            
            var value = pressure*4 - mildness > 0 ? 1 : 2;

            return value;

        }
    }
}
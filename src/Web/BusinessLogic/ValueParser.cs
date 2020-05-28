using System;

namespace Jineo.Logic
{
    public static class ValueParser
    {
        public static string MildnessParser(float value)
        {
            if(value < 1)
                return "ExtremelyMild";

            if(value < 4.2f)
                return "Mild";

            if(value < 8.4f)
                return "Middle";

            if(value < 9f)
                return "Solid";

            if(value >= 9f)
                return "ExtremelySolid";
            throw new Exception("Logic not implemented for this values");
        }
    }
}
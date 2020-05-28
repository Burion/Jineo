using System;
using System.Collections.Generic;
using System.Linq;

namespace Jineo.Logic
{
    public static class DataAnalizer
    {
        public static List<CellModel> AnalizeData(List<DataBlock> blocks) 
        {
            List<CellModel> cells = new List<CellModel>();
            foreach(var b in blocks) 
            {
                MildnessBlockAnalizer mildnessBlock = new MildnessBlockAnalizer();
                BlockTypeBlockAnalizer blockAnalizer = new BlockTypeBlockAnalizer();
                FloorsBlockAnalizer floors = new FloorsBlockAnalizer();
                var mildness = mildnessBlock.Analize(b);
                var blocktype = blockAnalizer.Analize(b);
                var FloorsCount = (int)floors.Analize(b);
                cells.Add(new CellModel() { x = b.X, y = b.Y, blockType = (int)blocktype, mildness = ValueParser.MildnessParser(mildness), floorsCount = FloorsCount });
                Console.WriteLine($"Cell data: Mildness: {ValueParser.MildnessParser(mildness)}, BlockType: {blocktype}, Floors Count: {FloorsCount}");
            }

            return cells;
        }

        public static List<CellModel> AnalizeData(List<DataBlock> blocks, int preferrableFloors) 
        {
            List<CellModel> cells = new List<CellModel>();
            foreach(var b in blocks) 
            {
                MildnessBlockAnalizer mildnessBlock = new MildnessBlockAnalizer();
                BlockTypeBlockAnalizer blockAnalizer = new BlockTypeBlockAnalizer();
                FloorsBlockAnalizer floors = new FloorsBlockAnalizer();
                var mildness = mildnessBlock.Analize(b);
                var blocktype = blockAnalizer.Analize(b);
                var floorsCount = (int)floors.Analize(b);
                var celltype = mildness < 1 ? 2 : mildness > 9 ? 3 : 1;
                cells.Add(new CellModel() { x = b.X, y = b.Y, blockType = (int)blocktype, mildness = ValueParser.MildnessParser(mildness), floorsCount = floorsCount, celltype = celltype});
                Console.WriteLine($"Cell data: Mildness: {ValueParser.MildnessParser(mildness)}, BlockType: {blocktype}, Floors Count: {floorsCount}");
                foreach(var c in cells.Where(c => c.floorsCount < preferrableFloors))
                {
                    c.floorsCount = 0;
                }
            }

            return cells;
        }

    }
}
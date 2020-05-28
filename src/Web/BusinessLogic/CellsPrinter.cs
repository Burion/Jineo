using System;
using System.Collections.Generic;

namespace Jineo.Logic
{
    public static class CellsPrinter
    {
        public static void Print(List<CellModel> cells)
        {
            foreach(var cell in cells)
            {
                Console.SetCursorPosition(cell.x, cell.y);
                switch(cell.mildness)
                {
                    case "Mild":
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                }
                if(cell.celltype == 2)
                    Console.Write('w');
                else
                if(cell.celltype == 3)
                    Console.Write('r');
                else
                    Console.Write(cell.floorsCount);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
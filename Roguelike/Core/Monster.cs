using RLNET;
using System;

namespace Roguelike.Core
{
    public class Monster : Actor
    {
        public void DrawStats(RLConsole statConsole, int position)
        {
            int yPosition = 13 + (position * 2);

            statConsole.Print(1, yPosition, Symbol.ToString(), Color);

            int width = Convert.ToInt32(((double)Health / (double)MaxHealth) * 16.0);
            int remainingWidth = 16 - width;

            

            statConsole.SetBackColor(3, yPosition, width, 1, width > 8 ? Swatch.PrimaryDarker : Swatch.DbBlood);
            statConsole.SetBackColor(3 + width, yPosition, remainingWidth, 1, Swatch.PrimaryDarkest);

            statConsole.Print(2, yPosition, $": {Name}", Swatch.DbDark);
        }
    }
}

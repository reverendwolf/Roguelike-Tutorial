
using RLNET;

namespace Roguelike.Core
{
    public class Player : Actor
    {
        public Player()
        {
            Attack = 2;
            AttackChance = 50;
            Awareness = 15;
            Defense = 2;
            DefenseChance = 40;
            Gold = 0;
            Health = 100;
            MaxHealth = 100;
            Speed = 10;
            Name = "Brogue";
            Color = Colors.Player;
            Symbol = '@';
        }

        public void DrawStats ( RLConsole statConsole)
        {
            statConsole.SetBackColor(0, 0, statConsole.Width, statConsole.Height, Swatch.DbDark);
            statConsole.Print(1, 1, $"Name:    {Name}", Colors.TextHeading);
            statConsole.Print(1, 3, $"Health:  {Health}/{MaxHealth}", Colors.Text);
            statConsole.Print(1, 5, $"Attack:  {Attack} ({AttackChance}%)", Colors.Text);
            statConsole.Print(1, 7, $"Defense: {Defense} ({DefenseChance}%)", Colors.Text);
            statConsole.Print(1, 9, $"Gold:    {Gold}", Colors.Gold);
        }
    }
}

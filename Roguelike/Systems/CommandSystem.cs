
using Roguelike.Core;
using RogueSharp.DiceNotation;
using System.Text;

namespace Roguelike.Systems
{
    public class CommandSystem
    {
        public bool MovePlayer(Direction direction)
        {
            int x = Game.Player.X;
            int y = Game.Player.Y;

            switch(direction)
            {
                case Direction.Up:
                    y = Game.Player.Y - 1;
                    break;
                case Direction.Down:
                    y = Game.Player.Y + 1;
                    break;
                case Direction.Left:
                    x = Game.Player.X - 1;
                    break;
                case Direction.Right:
                    x = Game.Player.X + 1;
                    break;
                default:
                    return false;
            }

            if(Game.DungeonMap.SetActorPosition(Game.Player, x, y))
            {
                return true;
            }

            Monster monster = Game.DungeonMap.GetMonsterAt(x, y);
            if(monster != null)
            {
                Attack(Game.Player, monster);
                return true;
            }

            return false;
        }

        public void Attack(Actor attacker, Actor defender)
        {
            StringBuilder attackMessage = new StringBuilder();
            StringBuilder defenseMessage = new StringBuilder();

            int hits = ResolveAttack(attacker, defender, attackMessage);

            int blocks = ResolveDefense(defender, hits, attackMessage, defenseMessage);

            Game.MessageLog.Add(attackMessage.ToString());
            if(!string.IsNullOrWhiteSpace(defenseMessage.ToString()))
            {
                Game.MessageLog.Add(defenseMessage.ToString());
            }

            int damage = hits - blocks;

            ResolveDamage(defender, damage);
        }

        private static int ResolveAttack(Actor attacker, Actor defender, StringBuilder attackMessage)
        {
            int hits = 0;

            attackMessage.AppendFormat("{0} attacks {1} and rolls: ", attacker.Name, defender.Name);
            DiceExpression attackDice = new DiceExpression().Dice(attacker.Attack, 100);
            DiceResult attackResult = attackDice.Roll();

            foreach (TermResult termResult in attackResult.Results)
            {
                if(termResult.Value >= 100 - attacker.AttackChance)
                {
                    hits++;
                }
            }

            return hits;
        }

        private static int ResolveDefense(Actor defender, int hits, StringBuilder attackMessage, StringBuilder defenseMessage)
        {
            int blocks = 0;

            if(hits > 0)
            {
                attackMessage.AppendFormat("scoring {0} hits!", hits);
                defenseMessage.AppendFormat("  {0} defends and rolls: ", defender.Name);

                DiceExpression defenseDice = new DiceExpression().Dice(defender.Defense, 100);
                DiceResult defenseRoll = defenseDice.Roll();

                foreach(TermResult termResults in defenseRoll.Results)
                {
                    if(termResults.Value >= 100 - defender.DefenseChance)
                    {
                        blocks++;
                    }
                }
                defenseMessage.AppendFormat("resulting in {0} blocks!", blocks);
            }
            else
            {
                attackMessage.Append("and misses completely.");
            }

            return blocks;
        }

        private static void ResolveDamage(Actor defender, int damage)
        {
            if (damage > 0)
            {
                defender.Health -= damage;

                Game.MessageLog.Add($"{defender.Name} was hit for {damage} damage!");

                if (defender.Health <= 0)
                {
                    ResolveDeath(defender);
                }
            }
        }

        private static void ResolveDeath(Actor defender)
        {
            if(defender is Player)
            {
                Game.MessageLog.Add($"{defender.Name} got his shit rocked for the last time. Game Over!");
            }
            else if(defender is Monster)
            {
                Game.DungeonMap.RemoveMonster((Monster)defender);

                Game.MessageLog.Add($"{defender.Name} died and dropped {defender.Gold} gold.");
            }
        }
    }
}

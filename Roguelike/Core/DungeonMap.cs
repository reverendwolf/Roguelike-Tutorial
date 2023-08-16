using RogueSharp;
using RLNET;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.Core
{
    public class DungeonMap : Map
    {
        public List<Rectangle> Rooms;
        private List<Monster> _monsters;

        public DungeonMap()
        {
            Rooms = new List<Rectangle>();
            _monsters = new List<Monster>();
        }

        public void Draw(RLConsole mapConsole, RLConsole statConsole)
        {
            mapConsole.SetBackColor(0, 0, mapConsole.Width, mapConsole.Height, Swatch.DbDark);
            foreach(Cell cell in GetAllCells())
            {
                SetConsoleSymbolForCell(mapConsole, cell);
            }
            int i = 0;

            foreach(Monster monster in _monsters)
            {
                monster.Draw(mapConsole, this);

                if(IsInFov(monster.X, monster.Y))
                {
                    monster.DrawStats(statConsole, i);
                    i++;
                }
            }
        }

        public void SetConsoleSymbolForCell( RLConsole console, Cell cell)
        {
            if(!cell.IsExplored)
            {
                return;
            }

            if( IsInFov(cell.X, cell.Y))
            {
                if(cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.WallBackgroundFov, Colors.WallBackgroundFov, '#');
                }
            }
            else
            {
                if (cell.IsWalkable)
                {
                    console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '.');
                }
                else
                {
                    console.Set(cell.X, cell.Y, Colors.WallBackground, Colors.WallBackground, '#');
                }
            }
        }

        public void UpdatePlayerFieldOfView()
        {
            Player player = Game.Player;

            ComputeFov(player.X, player.Y, player.Awareness, true);

            foreach(Cell cell in GetAllCells())
            {
                if(IsInFov(cell.X, cell.Y))
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        public bool SetActorPosition(Actor actor, int x, int y)
        {
            if(GetCell(x,y).IsWalkable)
            {
                SetIsWalkable(actor.X, actor.Y, true);

                actor.X = x;
                actor.Y = y;

                SetIsWalkable(actor.X, actor.Y, false);

                if(actor is Player)
                {
                    UpdatePlayerFieldOfView();
                }
                return true;
            }
            return false;
        }

        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            Cell cell = GetCell(x, y) as Cell;
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
        }

        public void AddPlayer(Player player)
        {
            Game.Player = player;
            SetIsWalkable(player.X, player.Y, false);
            UpdatePlayerFieldOfView();
        }

        public void AddMonster(Monster monster)
        {
            _monsters.Add(monster);

            SetIsWalkable(monster.X, monster.Y, false);
        }

        public void RemoveMonster(Monster monster)
        {
            _monsters.Remove(monster);
            SetIsWalkable(monster.X, monster.Y, true);
        }

        public Monster GetMonsterAt(int x, int y)
        {
            return _monsters.FirstOrDefault(m => m.X == x && m.Y == y);
        }

        public Point GetRandomWalkableLocationInRoom(Rectangle room)
        {
            if(DoesRoomHaveWalkableSpace(room))
            {
                for (int i = 0; i < 100; i++)
                {
                    int x = Game.Random.Next(1, room.Width - 2) + room.X;
                    int y = Game.Random.Next(1, room.Height - 2) + room.Y;

                    if(IsWalkable(x,y))
                    {
                        return new Point(x, y);
                    }
                }
            }
            return default(Point);
        }

        public bool DoesRoomHaveWalkableSpace(Rectangle room)
        {
            for (int x = 0; x < room.Width; x++)
            {
                for (int y = 0; y < room.Height; y++)
                {
                    if(IsWalkable(x + room.X, y + room.Y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

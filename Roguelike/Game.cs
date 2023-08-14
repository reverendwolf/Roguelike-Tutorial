﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using Roguelike.Core;
using Roguelike.Systems;
using RogueSharp.Random;

namespace Roguelike
{
    public static class Game
    {
        private static bool _renderRequired = true;

        private static readonly int _screenWidth = 100;
        private static readonly int _screenHeight = 70;
        private static RLRootConsole _rootConsole;

        private static readonly int _mapWidth = 80;
        private static readonly int _mapHeight = 48;
        private static RLConsole _mapConsole;

        private static readonly int _messageWidth = 80;
        private static readonly int _messageHeight = 11;
        private static RLConsole _messageConsole;

        private static readonly int _statWidth = 20;
        private static readonly int _statHeight = 70;
        private static RLConsole _statConsole;

        private static readonly int _inventoryWidth = 80;
        private static readonly int _inventoryHeight = 11;
        private static RLConsole _inventoryConsole;

        public static CommandSystem CommandSystem { get; private set; }

        public static DungeonMap DungeonMap { get; private set; }

        public static Player Player { get; set; }

        public static MessageLog MessageLog { get; private set; }

        public static IRandom Random { get; private set; }

        static void Main()
        {
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);

            string fontFileName = "terminal8x8.png";

            string consoleTitle = $"RogueLike Tutorial: Seed {seed}";

            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1.5f, consoleTitle);

            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _messageHeight);
            _statConsole = new RLConsole(_statWidth, _statHeight);
            _inventoryConsole = new RLConsole(_inventoryWidth, _inventoryHeight);

            _rootConsole.Update += OnRootConsoleUpdate;
            _rootConsole.Render += OnRootConsoleRender;

            CommandSystem = new CommandSystem();

            MessageLog = new MessageLog();
            MessageLog.Add("Good morning yes hello there you are.");
            MessageLog.Add($"Created level with seed {seed}");

            MapGenerator mapGenerator = new MapGenerator(_mapWidth, _mapHeight,20, 13, 7);
            DungeonMap = mapGenerator.CreateMap();

            RogueSharp.Point start = DungeonMap.Rooms[0].Center;
            Player.X = start.X;
            Player.Y = start.Y;
            DungeonMap.UpdatePlayerFieldOfView();



            _inventoryConsole.SetBackColor(0, 0, _inventoryWidth, _inventoryHeight, Swatch.DbDark);
            _inventoryConsole.Print(1, 1, "Inventory", RLColor.White);

            _rootConsole.Run();
        }

        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            bool didPlayerAct = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();

            if(keyPress != null)
            {
                if(keyPress.Key == RLKey.Up)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                }
                else if(keyPress.Key == RLKey.Down)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                }
                else if (keyPress.Key == RLKey.Left)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                }
                else if (keyPress.Key == RLKey.Right)
                {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                }
                else if (keyPress.Key == RLKey.Escape)
                {
                    _rootConsole.Close();
                }
            }

            if(didPlayerAct)
            {
                _renderRequired = true;
            }
        }

        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            if(_renderRequired)
            {
                DungeonMap.Draw(_mapConsole);

                Player.Draw(_mapConsole, DungeonMap);

                MessageLog.Draw(_messageConsole);

                Player.DrawStats(_statConsole);

                RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, 0, _inventoryHeight);
                RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, _screenHeight - _messageHeight);
                RLConsole.Blit(_statConsole, 0, 0, _statWidth, _statHeight, _rootConsole, _mapWidth, 0);
                RLConsole.Blit(_inventoryConsole, 0, 0, _inventoryWidth, _inventoryHeight, _rootConsole, 0, 0);

                _rootConsole.Draw();

                _renderRequired = false;
            }
        }
    }
}

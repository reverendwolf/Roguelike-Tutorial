﻿using RLNET;
using Roguelike.Interfaces;
using RogueSharp;

namespace Roguelike.Core
{
    public class Actor : IActor, IDrawable
    {
        //actor
        private int _attack;
        private int _attackChance;
        private int _awareness;
        private int _defense;
        private int _defenseChance;
        private int _gold;
        private int _health;
        private int _maxHealth;
        private string _name;
        private int _speed;

        //drawable
        public RLColor Color { get; set; }
        public char Symbol { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Attack { get => _attack; set => _attack = value; }
        public int AttackChance { get => _attackChance; set => _attackChance = value; }
        public int Awareness { get => _awareness; set => _awareness = value; }
        public int Defense { get => _defense; set => _defense = value; }
        public int DefenseChance { get => _defenseChance; set => _defenseChance = value; }
        public int Gold { get => _gold; set => _gold = value; }
        public int Health { get => _health; set => _health = value; }
        public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
        public string Name { get => _name; set => _name = value; }
        public int Speed { get => _speed; set => _speed = value; }

        public void Draw(RLConsole console, IMap map)
        {
            if (!map.GetCell(X, Y).IsExplored) return;

            if (map.IsInFov(X, Y))
            {
                console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
            }
            else
            {
                console.Set(X, Y, Colors.Floor, Colors.FloorBackground, '.');
            }
        }
    }
}

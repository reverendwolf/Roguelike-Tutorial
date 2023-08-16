﻿using RLNET;
using Roguelike.Core;
using System.Collections.Generic;

namespace Roguelike.Systems
{
    public class MessageLog
    {
        private static readonly int _maxLines = 9;

        private readonly Queue<string> _lines;

        public MessageLog()
        {
            _lines = new Queue<string>();
        }

        public void Add(string message)
        {
            _lines.Enqueue(message);

            if(_lines.Count > _maxLines)
            {
                _lines.Dequeue();
            }
        }

        public void Draw(RLConsole console)
        {
            console.SetBackColor(0, 0, console.Width, console.Height, Swatch.DbDark);
            string[] lines = _lines.ToArray();
            for (int i = 0; i < lines.Length; i++)
            {
                console.Print(1, i + 1, lines[i], Colors.TextHeading);
            }
        }
    }
}

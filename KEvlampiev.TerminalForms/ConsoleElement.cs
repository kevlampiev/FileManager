using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEvlampiev.TerminalForms
{
    abstract public class ConsoleElement
    {
        
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Right { get => (Left + Width); }
        public int Bottom { get => (Top + Height); }
        public int MiddleHorizontal { get => (Left + Width / 2); }
        public int MiddleVertical { get => (Top - Height / 2); }
        public ConsoleColor Color { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        

        public ConsoleElement(int left, int top, int width, int height, ConsoleColor color, ConsoleColor backgroundColor)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
            Color = color;
            BackgroundColor = backgroundColor;
        
        }

        public ConsoleElement(int left, int top, int width, int height)
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
            Color = ConsoleColor.Cyan;
            BackgroundColor = ConsoleColor.Blue;
        }

        public abstract void Repaint();

        /// <summary>
        /// Получает козицию курсора
        /// </summary>
        /// <returns> Пара значений номера колонки и номера строки экрана</returns>
        public (int col, int row) GetCursorPos()
        { 
            return (Console.CursorLeft, Console.CursorTop);
        }
    }
}

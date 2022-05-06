using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEvlampiev.TerminalForms
{
    public class ErrorWindow : ConsoleWindow
    {
        private string _message;
        public ErrorWindow(string message) : base(35, 5, 50, 10, "Error")
        {
            Left = 35;
            Top = 5;
            Width = 50;
            Height = 10;
            
            Title = "Error";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkRed;
            _message = message;
        }

        private void DrawMessage() 
        {
            int innerWidth = Width - 2;
            string[] messageWords = _message.Split(' ');
            string lineExtension = "";
            int currentLine = Top + 1;

            
            foreach (string word in messageWords) 
            {
                if ((word.Length + lineExtension.Length + 1) >= innerWidth) 
                {
                    if (lineExtension.Length > innerWidth) lineExtension = lineExtension.Substring(0, innerWidth-1);
                    Console.SetCursorPosition(Left + 1, currentLine);
                    Console.ForegroundColor = Color;
                    Console.BackgroundColor = BackgroundColor;
                    Console.Write(lineExtension);
                    lineExtension = word;
                    currentLine++;
                }
                if (currentLine >= Height) { break;  }
                lineExtension +=  (" " + word);
            }
            Console.SetCursorPosition(Left + 1, currentLine);
            Console.ForegroundColor = Color;
            Console.BackgroundColor = BackgroundColor;
            Console.Write(lineExtension);

        }
        override public void Repaint()
        {
            Color = ConsoleColor.Yellow;
            BackgroundColor = ConsoleColor.DarkRed;
            base.Repaint();
            DrawMessage();
        }
        


    }
}

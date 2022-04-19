using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEvlampiev.TerminalForms
{

    public class BashTerminal:ConsoleElement
    {
        /// <summary>
        /// Директория в которой находится система
        /// </summary>
        public string CurrentDirectory { get; set; }
        /// <summary>
        /// Максимальная длина приветственной строки
        /// </summary>
        public int MaxPromptLength { get; set; }
        /// <summary>
        /// Команда, которая введена в bash
        /// </summary>
        public StringBuilder Command { get; set; }
        /// <summary>
        /// Начальная позиция с которой начинается набор комманды
        /// </summary>
        public int StartCommandLinePos { get=>GetStartCommandPosition(); }
        
        public BashTerminal(int left, int top, int width, string currentDir):base(left, top, width, 1)
        {
            CurrentDirectory = currentDir;
            MaxPromptLength = 15;
            Command = new StringBuilder("");
        }

        /// <summary>
        /// Вспомогательная функция формирующая приветственную строку 
        /// </summary>
        /// <returns>Строка приветствия</returns>
        private string GetPrompt()
        {
            string result = CurrentDirectory;
            if (result.Length >= (MaxPromptLength-1)) 
            {
                result = "~" + Path.GetDirectoryName(result);
            }
            return result + "$";
        }

        private string GetVisiblePartOfTheCommand() 
        {
            int availableSize = Math.Max(Width - StartCommandLinePos, 0);
            string result = Command.ToString();
            return (result.Length<=availableSize)?result:(result.Substring(0, availableSize - 1) + "►");
        }

        private int GetStartCommandPosition()
        { 
            return Left + GetPrompt().Length;
        }

        public override void Repaint()
        {
            Console.ResetColor();
            Console.SetCursorPosition(Left, Top);
            Console.Write(GetPrompt());
            Console.Write(GetVisiblePartOfTheCommand());
        }
    }
}

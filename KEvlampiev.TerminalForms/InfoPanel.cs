using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEvlampiev.TerminalForms
{
    public class InfoPanel:ConsoleWindow
    {
        //текущая директория
        private DirectoryInfo _currentDirectory;

        /// <summary>
        /// Текущая директория
        /// </summary>
        public DirectoryInfo CurrentDirectory { get => _currentDirectory; set { _currentDirectory = value; Repaint(); } }


        public InfoPanel(int left, int top, int width, int height, string title, string currentDir) : base(left, top, width, height, title)
        {
            CurrentDirectory = new DirectoryInfo(currentDir);
        }

        public InfoPanel(int left, int top, int width, int height, string title) : base(left, top, width, height, title)
        {
            CurrentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
        }

        public override void Repaint()
        {
            base.Repaint();
            
            Console.ForegroundColor = Color;
            Console.BackgroundColor = BackgroundColor;

            string pathName = "Path: "+CurrentDirectory.FullName;
            if (pathName.Length > Width-2) pathName = pathName.Substring(Width-5)+"...";

            
            Console.SetCursorPosition(Left + 1, Top + 1);
            Console.Write(pathName);

            string timeCreated = CurrentDirectory.CreationTime.ToString();
            Console.SetCursorPosition(Left + 1, Top + 2);
            Console.Write("Time created: "+timeCreated);

            string timeAccessed = CurrentDirectory.LastAccessTime.ToString();
            Console.SetCursorPosition(Left + 1, Top + 3);
            Console.Write("Last AccessTime: " + timeCreated);

        }

    }
}

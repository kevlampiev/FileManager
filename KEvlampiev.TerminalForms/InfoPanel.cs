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
            Console.SetCursorPosition(Left+1, Top+1);
            string pathName = "Path: "+CurrentDirectory.FullName;
            if (pathName.Length > Width-2) pathName = pathName.Substring(Width-2);
            Console.Write(pathName);
        }

    }
}

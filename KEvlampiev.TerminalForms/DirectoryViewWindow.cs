using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEvlampiev.TerminalForms
{
    internal class DirectoryViewWindow:ConsoleWindow
    {

        //Поле того, чтобы понять, какая поддиректория/файл отображается на самом верху экрана окна
        private int _startDirectoryIndex = 0;

        public string CurrentDirectory { get; set; }
        

        public DirectoryViewWindow(int left, int top, int width, int height, string title, string currentDir):base(left,top,width,height,title)
        { 
            CurrentDirectory = currentDir;
        }

        public override void Repaint()
        {
            base.Repaint();


        }

        //Вспомогательная функция, которая выдает все подкаталоги текущего каталога 
        private string[] GetTree(string[] tree, DirectoryInfo dir) 
        {
            return new string[1];
        }

        //Вспомогательная функция. Заполняет внутренность окна 
        private void DrawDirectoryTree() 
        {
            
        }
    }
}

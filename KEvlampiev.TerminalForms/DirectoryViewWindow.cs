using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEvlampiev.TerminalForms
{
    public class DirectoryViewWindow:ConsoleWindow
    {
        // Псевдографика для отображения узлов директорий/файлов в списке
        private string _lastDirTreeSign = "└─";
        private string _dirTreeSign = "├─";
        private string _anotherLevelSign = "│ ";

        //текущая страница
        private int _page=0;

        /// <summary>
        /// Общее количество строк во внутреннем пространстве окна
        /// </summary>
        public int InnerLines { get =>(Height-2); }

        /// <summary>
        /// Общее количество столбцова 
        /// </summary>
        public int InnerColumns { get => (Width - 2); }

        public int Page { get => _page; set { _page = value; } }


        //Поле того, чтобы понять, какая поддиректория/файл отображается на самом верху экрана окна
        //Пока на паузе
        private int _startDirectoryIndex = 0;

        public DirectoryInfo CurrentDirectory { get; set; }

        /// <summary>
        /// Список поддиректорий текущей директории
        /// </summary>
        public DirectoryInfo[] DirectoryTree { get; set; }
        

        public DirectoryViewWindow(int left, int top, int width, int height, string title, string currentDir):base(left,top,width,height,title)
        { 
            CurrentDirectory = new DirectoryInfo(currentDir);
        }

        public DirectoryViewWindow(int left, int top, int width, int height, string title) : base(left, top, width, height, title)
        {
            CurrentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
        }



        public override void Repaint()
        {
            base.Repaint();
            DrawDirectoryTree(CurrentDirectory, 0);

        }

        //Вспомогательная функция строит великую строку в которой вся информация о директориях и файлах
        private void GetTree(StringBuilder tree, DirectoryInfo dir, string indent, bool lastDirectoryInList)
        {
            tree.Append(indent);
            if (lastDirectoryInList) 
            {
                tree.Append(_lastDirTreeSign);
                indent += "  ";
            } else {
                tree.Append(_dirTreeSign);
                indent += _anotherLevelSign;
            };
            tree.Append($"{dir.Name}\n");
            FileInfo[] innerFiles = dir.GetFiles();
            foreach (FileInfo innerFile in innerFiles) 
            {
                if (innerFile == innerFiles.Last())
                {
                    tree.Append($"{indent}{_lastDirTreeSign}{innerFile.Name} \n");
                }
                else
                {
                    tree.Append($"{indent}{_dirTreeSign}{innerFile.Name} \n");
                }
            }
            DirectoryInfo[] innerDirectories = dir.GetDirectories();
            foreach (DirectoryInfo innerDirectory in innerDirectories) 
            { 
                GetTree(tree, innerDirectory, indent, innerDirectory == innerDirectories.Last());
            }
        }

        /*
        //Вспомогательная функция, которая выдает все подкаталоги текущего каталога 
        //ЭТА ИДЕЯ НА ЗАМОРОЗКЕ
        private DirectoryInfo[] GetTree(DirectoryInfo curDir, DirectoryInfo[] tree) 
        {
            curDir.GetDirectories();
            return new DirectoryInfo[1];
        }
        */

        //Вспомогательная функция. Заполняет внутренность окна 
        private void DrawDirectoryTree(DirectoryInfo dir, int page) 
        {
            StringBuilder tree = new StringBuilder();
            GetTree(tree, dir, "", true);
            string[] fsObjectImages = tree.ToString().Split("\n");

            //На всякий пожарный случай
            page = Math.Min(page,-(- fsObjectImages.Length/InnerColumns));
            
            int startDiapason = page*InnerLines;
            int endDiapason = Math.Min((page+1)*InnerLines - 1, fsObjectImages.Length -1);

            for (int pos = startDiapason, currentLine = Top+1; pos <= endDiapason; pos++, currentLine++)
            {
                Console.SetCursorPosition(Left + 1, currentLine);
                Console.ForegroundColor = Color;
                Console.BackgroundColor = BackgroundColor;
                Console.Write(fsObjectImages[pos].Substring(0, Math.Min(InnerColumns,fsObjectImages[pos].Length)));
            }
           
        }
    }
}

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

        //максимальное количество объектов на 1 страницу 
        private int _linesPerPage;

        //контент (уже готовый к отрисовке согласно данной идеологии)
        private string[] _content;

        //Поле того, чтобы понять, какая поддиректория/файл отображается на самом верху экрана окна
        //Пока на паузе
        private int _startDirectoryIndex = 0;

        //текущая директория
        private DirectoryInfo _currentDirectory;

        /// <summary>
        /// Общее количество строк во внутреннем пространстве окна
        /// </summary>
        public int InnerLines { get =>(Height-2); }

        /// <summary>
        /// Количество объектов на 1 страницу
        /// </summary>
        public int LinesPerPage { get=>_linesPerPage; set { SetLinesPerPage(value); } }

        /// <summary>
        /// Общее количество столбцов
        /// </summary>
        public int InnerColumns { get => (Width - 2); }

        /// <summary>
        /// Текущая страница отображения списка директорий 
        /// </summary>
        public int Page { get => _page; set { SetPage(value); } }
     

        /// <summary>
        /// Текущая директория
        /// </summary>
        public DirectoryInfo CurrentDirectory { get=>_currentDirectory; set { SetCurrentDirectory(value); } }

        /// <summary>
        /// Список поддиректорий текущей директории
        /// </summary>
        public DirectoryInfo[] DirectoryTree { get; set; }

        public DirectoryViewWindow(int left, int top, int width, int height, string title, string currentDir, int linesPerPage) : base(left, top, width, height, title)
        {
            CurrentDirectory = new DirectoryInfo(currentDir);
            if (linesPerPage <= 0 || linesPerPage > InnerLines) 
            {
                _linesPerPage = InnerLines;
            } 
            else 
            {
                _linesPerPage = linesPerPage;
            }
            
        }

        public DirectoryViewWindow(int left, int top, int width, int height, string title, string currentDir):base(left,top,width,height,title)
        { 
            CurrentDirectory = new DirectoryInfo(currentDir);
            _linesPerPage = InnerLines;
        }

        public DirectoryViewWindow(int left, int top, int width, int height, string title) : base(left, top, width, height, title)
        {
            CurrentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            _linesPerPage = InnerLines;
        }



        public override void Repaint()
        {
            base.Repaint();
            DrawDirectoryTree(CurrentDirectory);

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
            try { 
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
            }
            catch (Exception ex) {
                
            }

            try {
                DirectoryInfo[] innerDirectories = dir.GetDirectories();
                foreach (DirectoryInfo innerDirectory in innerDirectories)
                {
                    GetTree(tree, innerDirectory, indent, innerDirectory == innerDirectories.Last());
                }
            } catch (Exception ex) {
                
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


        //Вспомогательная функция, которая заготовляет контент для последующей отрисовки
        private void GetContent() 
        {
            DirectoryInfo dir = new DirectoryInfo(CurrentDirectory.FullName); //о то он черт знает что попанишет по адресу CurrentDirectory
            StringBuilder tree = new StringBuilder();
            GetTree(tree, dir, "", true);
            _content = tree.ToString().Split("\n");
        }

        private void SetCurrentDirectory(DirectoryInfo dir) 
        {
            _currentDirectory = dir;
            _page = 0; 
            _startDirectoryIndex = 0;
            GetContent();
            Repaint();
        }

        private void SetLinesPerPage(int lpp) 
        {
            if (lpp <= 0 || lpp > InnerLines)
            {
                _linesPerPage = InnerLines;
            }
            else 
            { 
                _linesPerPage = lpp;
            }
            Repaint();
        } 

        private void SetPage(int page) 
        {

            //int totalPages=(_content.Length + InnerLinesInnerLines-2)/(InnerLines);
            int totalPages = (_content.Length + LinesPerPage - 2) / (LinesPerPage);
            if (page >= totalPages) { _page = 0; }
            else if (page < 0) 
            { _page = totalPages - 1; }
            else 
            { _page = page; }
            
            Repaint();
        }

        //Вспомогательная функция. Заполняет внутренность окна 
        private void DrawDirectoryTree(DirectoryInfo dir) 
        {
            //int startDiapason = _page*InnerLines;
            //int endDiapason = Math.Min((_page+1)*InnerLines - 1, _content.Length -1);

            int startDiapason = _page * LinesPerPage;
            int endDiapason = Math.Min((_page + 1) * LinesPerPage - 1, _content.Length - 1);

            for (int pos = startDiapason, currentLine = Top+1; pos <= endDiapason; pos++, currentLine++)
            {
                Console.SetCursorPosition(Left + 1, currentLine);
                Console.ForegroundColor = Color;
                Console.BackgroundColor = BackgroundColor;
                Console.Write(_content[pos].Substring(0, Math.Min(InnerColumns,_content[pos].Length)));
            }
           
        }
    }
}

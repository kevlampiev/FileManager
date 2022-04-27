using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KEvlampiev.TerminalForms;

namespace FileManager
{

    /// <summary>
    /// Создает объект - оператор, внутри сидят ничего не умеющие делать объекты для отображения, 
    /// вся работа выполняется им. Пока такая идеология
    /// </summary>
    public class FMController
    {
        //размер окна и буфера для отображения
        private int _windowHeight = 30;
        private int _windowWidth = 120;

        //Внутренние элементы для отображения
        private DirectoryViewWindow? _dirTree;
        private BashTerminal? _bash;
        private InfoPanel? _infoPanel;

        public DirectoryInfo CurrentDirectory { get; set; }

        public FMController(int windowHeight, int windowWidth) 
        {
            _windowHeight = windowHeight;
            _windowWidth = windowWidth;
            Console.Title = "File manager";

            //Эти две функции в Linux не работают
            Console.SetWindowSize(_windowWidth, _windowHeight);
            Console.SetBufferSize(_windowWidth, _windowHeight);
            //подумать чем их заменить 
            
            CurrentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            _dirTree = new DirectoryViewWindow(0, 0, _windowWidth, _windowHeight - 10, "Directory tree", CurrentDirectory.FullName);
            _infoPanel = new InfoPanel(0, _windowHeight - 10, _windowWidth, 8, "Info", CurrentDirectory.FullName);
            _bash = new BashTerminal(0, _windowHeight - 1, _windowWidth, CurrentDirectory.FullName);
            
            Repaint();
        }

        /// <summary>
        /// Перерисовка всех внутренних элеменов
        /// </summary>
        public void Repaint() 
        {
            Console.Clear();
            _dirTree.Repaint();
            _infoPanel.Repaint();
            _bash.Repaint();
        }


        private void ParseCommandString(string command)
        {
            string[] commandParams = command.ToLower().Split(' ');
            if (commandParams.Length > 0)
            {
                switch (commandParams[0])
                {
                    case "cd":
                        if (commandParams.Length > 1 && Directory.Exists(commandParams[1]))
                        {
                            _bash.CurrentDirectory = commandParams[1];
                            _dirTree.CurrentDirectory = new DirectoryInfo(commandParams[1]);
                            _bash.Command.Clear();
                            _bash.Repaint();
                            _infoPanel.CurrentDirectory = new DirectoryInfo(commandParams[1]);
                        }

                        break;
                        /*
                    case "ls":
                        if (commandParams.Length > 1 && Directory.Exists(commandParams[1]))
                        {
                            if (commandParams.Length > 3 && commandParams[2] == "-p" && int.TryParse(commandParams[3], out int n))
                            {
                                DrawTree(new DirectoryInfo(commandParams[1]), n);
                            }
                            else
                            {
                                DrawTree(new DirectoryInfo(commandParams[1]), 1);
                            }
                        }
                        break;
                        */
                }
            }
            //UpdateConsole();
        }

        public void Run()
        {
            (int row, int col) = Utils.GetCursorPosition();
            StringBuilder command = new StringBuilder();
            char key;
            while (true)
            {
                key = Console.ReadKey().KeyChar;
                if (key == (byte)ConsoleKey.Backspace)
                {
                    if (command.Length > 0)
                    {
                        command.Remove(command.Length - 1, 1);
                    }
                    else
                    {
                        command = new StringBuilder();
                    }
                    _bash.Command = command;
                    _bash.Repaint();
                    continue;
                }
                if (key == (byte)ConsoleKey.Enter)
                {
                    ParseCommandString(command.ToString());
                    command.Clear();
                    _bash.Command = command;
                    _bash.Repaint();
                    //Здесь что-то отсылается на исполнение и очищается command и записывается в лог
                    continue;
                }
                command.Append(key);
                _bash.Command = command;
                _bash.Repaint();
                if (command.ToString() == "exit" || command.ToString() == "quit") { break; }
            }

        }

        public void Done()
        {
            Console.ResetColor();
            Console.Clear();
        }

    }
}

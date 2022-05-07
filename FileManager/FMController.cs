using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KEvlampiev.TerminalForms;
using System.Text.Json;

namespace FileManager
{

   


    /// <summary>
    /// Создает объект - оператор, внутри сидят ничего не умеющие делать объекты для отображения, 
    /// вся работа выполняется им. Пока такая идеология
    /// </summary>
    public class FMController
    {
        private Settings _settings;

        //Внутренние элементы для отображения
        private DirectoryViewWindow? _dirTree;
        private BashTerminal? _bash;
        private InfoPanel? _infoPanel;

        public DirectoryInfo CurrentDirectory { get=>new DirectoryInfo(_settings.CurrentDirectoryStr); set { ChangeDir(value); } }

        public FMController(int windowHeight, int windowWidth) 
        {
            _settings = new Settings(windowHeight, windowWidth, Directory.GetCurrentDirectory(), 0 );
            Init();
            
        }
        public FMController(Settings settings)
        {
            _settings = settings;
            Init();
        }
        /// <summary>
        /// Инициализирует основные переменные при запуске. Сделано, чтобы не раздувать сильно конструкторы
        /// </summary>
        private void Init() 
        {
            Console.Title = "File manager";

            //Эти две функции в Linux не работают
            Console.SetWindowSize(_settings.WindowWidth, _settings.WindowHeight);
            Console.SetBufferSize(_settings.WindowWidth, _settings.WindowHeight);
            //подумать чем их заменить 


            _dirTree = new DirectoryViewWindow(0, 0, _settings.WindowWidth, _settings.WindowHeight - 10, "Directory tree", _settings.CurrentDirectoryStr, _settings.LinesPerDirTreePage);
            _infoPanel = new InfoPanel(0, _settings.WindowHeight - 10, _settings.WindowWidth, 8, "Info", _settings.CurrentDirectoryStr);
            _bash = new BashTerminal(0, _settings.WindowHeight - 1, _settings.WindowWidth, _settings.CurrentDirectoryStr);
            //CurrentDirectory = CurrentDirectory;

            Repaint();
        }


        /// <summary>
        /// Перерисовка всех внутренних элеменов
        /// </summary>
        public void Repaint() 
        {
            Console.ResetColor();
            Console.Clear();
            _dirTree.Repaint();
            _infoPanel.Repaint();
            _bash.Repaint();
        }



        private void CancelCommand(string message) 
        { 
            Utils.DisplayError(message);
            Repaint();
            _bash.Command = new StringBuilder();
        }
 

        //Вспомогательная функция, поскольку просто так и не угадаешь, что введет пользователь после "cd", "cp" или "rm"
        private string GuessDirname(string targetDir) 
        {
            if (targetDir == "..") { 
                targetDir = CurrentDirectory.Parent.ToString();
            }
            else if (!targetDir.Contains(Path.DirectorySeparatorChar))
            { 
                targetDir = CurrentDirectory.FullName + Path.DirectorySeparatorChar +targetDir;
            };
            if (Directory.Exists(targetDir)) 
            { 
                return targetDir; 
            } 
            else 
            { 
                return CurrentDirectory.FullName; 
            }
        }

        //Вспомогательная функция, только уже для папки назначения
        private string GuessDestinationDirname(string destinationDir) 
        {
            if (Directory.Exists(destinationDir)) 
            {
                return destinationDir;
            }
            if (!destinationDir.Contains(Path.DirectorySeparatorChar))
            {
               return CurrentDirectory.FullName + Path.DirectorySeparatorChar + destinationDir;
            }
            return CurrentDirectory.FullName;
            
        }


        //Копирование файла при условии, что исходный файл есть 100%
        private void CopyFile(string sourceFilename, string destinationFilename)
        {
            try
            {
                File.Copy(sourceFilename, destinationFilename, true);
            }
            catch (Exception e)
            {
                CancelCommand(e.Message);
            }
        }

        private void CopyDirectory(string sourceDirectory, string destinationDirectory) 
        { 
             string[] subDirs = Directory.GetDirectories(sourceDirectory, "*", SearchOption.AllDirectories);
            foreach (string subDir in subDirs)
            {
                try
                {
                    Directory.CreateDirectory(subDir.Replace(sourceDirectory, destinationDirectory));
                } catch (Exception e) {
                    CancelCommand(e.Message);
                }
            }

            string[] files = Directory.GetFiles(sourceDirectory, "*.*", SearchOption.AllDirectories);
            foreach (string file in files) 
            {
                try { 
                    File.Copy(file, file.Replace(sourceDirectory, destinationDirectory), true);
                } catch (Exception e) {
                    CancelCommand(e.Message);
                }
            }
            
        }

        private void CopyObjects(string source, string destination) 
        {
            Console.CursorVisible = false;
            if (File.Exists(source)) 
            {
                CopyFile(source, destination);
                return;
            } 
            source = GuessDirname((string)source);
            destination= GuessDestinationDirname((string)destination);

            if (Directory.Exists(source)) 
            { 
                CopyDirectory(source, destination);    
            } 
            else 
            {
                CancelCommand($"Объект {source} не обнаружен");
            }
           CurrentDirectory = CurrentDirectory ;
            Console.CursorVisible = true;
        }

        //Вспомогательная функция по удалению файла
        private void RemoveFile(string fileName) 
        {
            try 
            { 
                File.Delete(fileName);
            } 
            catch( Exception e) 
            { 
                CancelCommand(e.Message);
            }
        
        }

        //Вспомогательная функция удаления директории
        private void RemoveDirectory(string directoryName) 
        { 
            string[] directories = Directory.GetDirectories(directoryName);
            string[] files = Directory.GetFiles(directoryName);
            foreach (string d in directories) {
                try
                {
                    RemoveDirectory(d);
                }
                catch (Exception e)
                { 
                    CancelCommand(e.Message);
                    return;
                }
            }

            foreach(string f in files) { RemoveFile(f); }
            Directory.Delete(directoryName); //а достаточно было добавить true вторым параметром
        
        }


        private void RemoveObject(string objName) 
        {
            if (File.Exists(objName)) 
            { 
               RemoveFile(objName);
                CurrentDirectory = CurrentDirectory;
                return ;  
            }
            if (Directory.Exists(GuessDirname(objName))) 
            { 
                RemoveDirectory(GuessDirname(objName));
                CurrentDirectory = CurrentDirectory;
                return; 
            }
            CancelCommand($"Объект {objName}не существует");
        }
 
        //Вспомогательная функция чтобы все синхронизировалось при изменении свойства CurrentDirectory
        private void ChangeDir(DirectoryInfo targetDir) 
        {
            Console.CursorVisible = false; 
            try
            {
                if (Directory.Exists(targetDir.ToString()))
                {
                    _settings.CurrentDirectoryStr = targetDir.FullName;
                    _dirTree.CurrentDirectory = targetDir;
                    _bash.CurrentDirectory = targetDir.FullName;
                    _infoPanel.CurrentDirectory = targetDir;
                }
            }
            catch (Exception e) 
            {
                CancelCommand(e.Message);
            }
            Console.CursorVisible = true;
        }


        private void ParseCommandString(string command)
        {
            string[] commandParams = command.ToLower().Split(' ');
            if (commandParams.Length > 0)
            {
                switch (commandParams[0])
                {
                    case "cd":
                        if (commandParams.Length > 1 )
                        { 
                            CurrentDirectory = new DirectoryInfo(GuessDirname(commandParams[1]));
                        }

                        break;
                    /*    
                     *    Мне показалось как-то нелогично делать вывод директории в которой не находишься в окне File manager'а
                     *    поэтому буду использовать комманду ls просто чтобы менять страницы просмотра списка текущей директории
                     */
                    case "ls":
                        if (commandParams.Length > 1)
                        {
                            if (commandParams[1] == "-p" && int.TryParse(commandParams[2], out int pageNumber)) 
                            { 
                                _dirTree.Page = pageNumber;
                            }
                            if (commandParams[1] == "--") 
                            {
                                _dirTree.Page--;
                            }
                            if (commandParams[1] == "++")
                            {
                                _dirTree.Page++;
                            }
                        }
                        else 
                        {
                            _dirTree.Page++;
                        }
                        break;
                    case "cp":
                        if (commandParams.Length > 2)
                        {
                            CopyObjects(commandParams[1], commandParams[2]);
                        }
                        else 
                        {
                            CancelCommand("Неправильный формат команды");
                        }
                        break ;
                    case "rm":
                        if (commandParams.Length > 1)
                        {
                            RemoveObject(commandParams[1]);
                        }
                        else
                        {
                            CancelCommand("Не задано имя удаляемого объекта");
                        }
                        break;
                    default: CancelCommand("Неизвестная команда"); break;

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
            string jsonText = JsonSerializer.Serialize(this._settings);
            File.WriteAllText("FileManager.settings.json", jsonText);
        }

    }
}

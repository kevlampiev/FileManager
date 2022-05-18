using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    /// <summary>
    /// Настройки программы
    /// </summary>
    public class Settings
    {
        //размер окна и буфера для отображения
        public int WindowHeight { get; set; }
        public int WindowWidth { get; set; }
        //Текущая директория
        public string CurrentDirectoryStr { get; set; }

        /// <summary>
        /// Максимальное количество файлов и директорий на 1 странице листа просмотра
        /// </summary>
        public int LinesPerDirTreePage { get; set; }


        /// <summary>
        /// Имя файла для хранения логов
        /// </summary>
        public string LogFilename { get; set; }
        /// <summary>
        /// Имя файла для хранения истории команд
        /// </summary>
        public string CommandHistoryFilename { get; set; }


        public Settings(int wHeight, int wWidth, string dir, int linesPerPage, string _commandHistory, string _logFile)
        { 
            WindowHeight = wHeight;
            WindowWidth = wWidth;   
            CurrentDirectoryStr = dir;
            LinesPerDirTreePage = Math.Max(linesPerPage,5);
            LogFilename = _logFile??"FileManager.err.log";
            CommandHistoryFilename = _commandHistory??"FileManager.cmdhistory.txt";

        }

        public Settings() { }
    }
}

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

        public Settings(int wHeight, int wWidth, string dir, int linesPerPage)
        { 
            WindowHeight = wHeight;
            WindowWidth = wWidth;   
            CurrentDirectoryStr = dir;
            LinesPerDirTreePage = Math.Max(linesPerPage,5);
        }

        public Settings() { }
    }
}
